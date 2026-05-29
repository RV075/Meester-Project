using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IDamageable
{
    public int health = 1;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float jumpForce = 5;

    public static int maxJumpCount = 1;
    private int jumpCount = 0;

    public static float moveDirectionX;
    public static float moveDirectionY;

    public static bool canMove = true;
    public static bool isInvisible = false;
    void Update()
    {
        if (!canMove) return;

        moveDirectionX = Input.GetAxis("Horizontal");
        moveDirectionY = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        else if (Input.GetKeyDown(KeyCode.RightShift))
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
        //else if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    PlayerGuns.instance.Toggle();
        //}
        Flip();
    }

    public void TakeDamage(int damage)
    {
        if (isInvisible) return;
        
        health -= damage;

        if (health <= 0)
        {
            //GetComponent<SpriteRenderer>().enabled = false;
            //Destroy(gameObject);
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
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveDirectionX < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void FixedUpdate()
    {
        if (!canMove) return;

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
