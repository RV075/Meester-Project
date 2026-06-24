using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Texture2D cursor;

    [SerializeField] private float fireRate = 0.5f;
    private float cooldown = 0;

    private void Start()
    {
        Cursor.SetCursor(cursor, new Vector2(50, 50), CursorMode.Auto);
    }


    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector2 dir = mousePos - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        cooldown -= Time.deltaTime;

        if (cooldown <= 0 && Input.GetMouseButtonDown(0))
        {
            Shoot(angle);
            cooldown = fireRate;
        }
    }


    private void Shoot(float angle)
    {

        if (DataToLoad.playerLaserBulletObjects.Count > 0)
        {
            DataToLoad.playerLaserBulletObjects[0].transform.SetPositionAndRotation(transform.position + transform.right, Quaternion.Euler(0, 0, angle + 90));
            DataToLoad.playerLaserBulletObjects[0].SetActive(true);
            DataToLoad.playerLaserBulletObjects.Remove(DataToLoad.playerLaserBulletObjects[0]);
        }
        else
        {
            Instantiate(bulletPrefab, transform.position + transform.right, Quaternion.Euler(0, 0, angle + 90));
        }
    }
}
