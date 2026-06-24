using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 200;
    private int health;

    public Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Image healthBar;
    [SerializeField] private float moveSpeed = 5;
    private readonly float jumpForce = 7.5f;

    public static int maxJumpCount = 1;
    private int jumpCount = 0;

    public static float moveDirectionX;
    public static float moveDirectionY;

    public static bool canMove = true;
    public static bool isInvisible = false;

    void Start()
    {
        canMove = true;
        isInvisible = false;
        health = maxHealth;
    }
    void Update()
    {
        if (!canMove)
            return;

        moveDirectionX = Input.GetAxis("Horizontal");
        moveDirectionY = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        Flip();
    }

    public void TakeDamage(int damage, bool ignoreInvisibility)
    {
        if (!ignoreInvisibility)
            if (isInvisible) return;

        health -= damage;

        if (healthBar != null)
            healthBar.fillAmount = Mathf.Clamp01((float)health / (float)maxHealth);

        if (health <= 0)
        {
            SceneLoader.LoadScene(PlayerData.data.gameData.Level);
        }
    }
    private void Jump()
    {
        if (jumpCount <= 0) return;

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpCount--;
    }

    private void Flip()
    {
        if (moveDirectionX > 0)
            sr.flipX = false;
        else if (moveDirectionX < 0)
            sr.flipX = true;
    }

    private void FixedUpdate()
    {
        if (!canMove)
            return;

        rb.velocity = new Vector2(moveDirectionX * moveSpeed, rb.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                jumpCount = maxJumpCount;
                break;
            }
        }
    }
}
