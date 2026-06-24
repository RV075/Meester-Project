using UnityEngine;
using UnityEngine.UI;

public class BossShield : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 500;
    [HideInInspector] public float health;

    [SerializeField] private GameObject shieldBarGO;
    [SerializeField] private Image shieldBar;
    [SerializeField] private GameObject rotatingPart;
    void Start()
    {
        health = maxHealth;
    }
    public void Activate(int multiplayer)
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<PolygonCollider2D>().enabled = true;
        shieldBarGO.SetActive(true);
        health = maxHealth *= multiplayer;
    }

    public void TakeDamage(int damage, bool _)
    {
        health -= damage;

        if (health <= 0)
        {
            shieldBar.fillAmount = 0;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            shieldBarGO.SetActive(false);
        }
    }

    void Update()
    {
        Rotate();
        shieldBar.fillAmount = Mathf.Clamp01(health / maxHealth);
    }

    public void Rotate()
    {
        if (DataToLoad.player == null) return;

        Vector2 dir = DataToLoad.player.transform.position - rotatingPart.transform.position;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle + 90);

        rotatingPart.transform.rotation = Quaternion.Lerp(
            rotatingPart.transform.rotation,
            targetRotation,
            Time.deltaTime * 5f
        );
    }
}