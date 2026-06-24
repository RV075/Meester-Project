using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ChangeCameraView : MonoBehaviour
{
    [SerializeField] private Transform endPos;

    private GameObject player;
    private float distanceToObjects;
    private Camera cam;
    private float cameraZOrigin;

    private void Start()
    {
        BoxCollider2D col = endPos.GetComponent<BoxCollider2D>();
        distanceToObjects = Vector2.Distance(transform.position, new Vector2(col.bounds.center.x, col.bounds.min.y));
        cam = Camera.main;
        cameraZOrigin = cam.transform.position.z;
    }
    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(player.transform.position, endPos.position);
        float percentage = Mathf.Abs((distanceToPlayer / distanceToObjects * 100) - 100);

        Vector3 camera = cam.transform.localPosition;
        camera = new Vector3(camera.x, percentage * 5 / 40, cameraZOrigin * ((100 + percentage * 1.5f) / 100));
        cam.transform.localPosition = camera;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            player = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            player = null;
    }
}
