using UnityEngine;

public class gun_pickup : MonoBehaviour
{
    [SerializeField] private Door door;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(door.Open());
            enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
