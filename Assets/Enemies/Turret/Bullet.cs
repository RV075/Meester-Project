using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5f;
    public int damage = 10;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = -transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ignoreTrigger")) return;

        if (collision.gameObject.CompareTag("Enemy"))
            return;

        if (collision.gameObject.CompareTag("Player") && !Player.isInvisible)
        {
            collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(damage, false);
        }
        else if (collision.gameObject.CompareTag("Player") && Player.isInvisible) return;

        if (!DataToLoad.bulletObjects.Contains(gameObject))
            DataToLoad.bulletObjects.Add(gameObject);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        rb.velocity = -transform.up * speed;
    }

    public void DisableOnCall()
    {
        if (!DataToLoad.bulletObjects.Contains(gameObject))
            DataToLoad.bulletObjects.Add(gameObject);
        gameObject.SetActive(false);
    }
}
