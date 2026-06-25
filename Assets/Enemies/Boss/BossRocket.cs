using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BossRockets : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D bc;
    [SerializeField] private SpriteRenderer childSR;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float startSpeed = 1;
    private float currentSpeed;
    [SerializeField] private float speedIncrement = 1;

    [SerializeField] private float maxHomingStrength = 3f;
    [SerializeField] private float startHomingStrenght = 1;
    float currentHomingStrenght;
    [SerializeField] private float homingIncrement = 0.2f; 

    void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();
        if (bc == null)
            bc = GetComponent<BoxCollider2D>();

        currentSpeed = startSpeed;
        currentHomingStrenght = startHomingStrenght;

        currentHomingStrenght += Random.Range(-0.1f, 0.1f);
        transform.rotation *= Quaternion.Euler(0, 0, Random.Range(-60f, 60f));

        rb.velocity = -transform.up * startSpeed;
    }


    void Update()
    {
        currentSpeed = Mathf.Min(currentSpeed + speedIncrement *Time.deltaTime, maxSpeed);
        currentHomingStrenght = Mathf.Min(currentHomingStrenght + homingIncrement * Time.deltaTime, maxHomingStrength);
        Rotate();

        rb.velocity = -transform.up * currentSpeed;
    }

    private void Rotate()
    {
        Vector3 dir = DataToLoad.player.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0, 0, angle + 90),
            Time.deltaTime * currentHomingStrenght
            );
    }

    void ResetAfterExplode()
    {
        if (!DataToLoad.rocketObjects.Contains(gameObject))
            DataToLoad.rocketObjects.Add(gameObject);
        else
        {
            DataToLoad.rocketObjects.Remove(gameObject);
            Destroy(gameObject);
        }

        Disable();
    }

    public void Disable()
    {
        enabled = false;
        sr.enabled = false;
    }

    public void Enable()
    {
        enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;

        currentSpeed = startSpeed;
        currentHomingStrenght = startHomingStrenght;
        rb.gravityScale = 1;
        rb.velocity = -transform.up * startSpeed;
        sr.enabled = false;
        childSR.enabled = true;
        bc.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ignoreTrigger")) return;

        if (collision.gameObject.CompareTag("Enemy"))
            return;

        if (collision.gameObject.CompareTag("Player") && !Player.isInvisible)
        {
            collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(50, false);
        }
        else if (collision.gameObject.CompareTag("Player") && Player.isInvisible) return;

        enabled = false;
        sr.enabled = true;
        childSR.enabled = false;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        bc.enabled = false;
        Invoke(nameof(ResetAfterExplode), 0.5f);
    }

    public void DisableOnCall()
    {
        if (!DataToLoad.rocketObjects.Contains(gameObject))
            DataToLoad.rocketObjects.Add(gameObject);
        gameObject.SetActive(false);
    }
}
