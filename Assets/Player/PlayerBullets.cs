using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBullets : MonoBehaviour
{
    private Rigidbody2D rb;

    public float speed = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = -transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ignoreTrigger")) return;
        else if (collision.gameObject.name.Contains("PlayerBullet")) return;

        if (collision.gameObject.CompareTag("Player"))
            return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(25, false);
        }

        if (!DataToLoad.playerLaserBulletObjects.Contains(gameObject))
        DataToLoad.playerLaserBulletObjects.Add(gameObject);
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        rb.velocity = -transform.up * speed;
    }
}
