using UnityEngine;

public class PlayerGuns : MonoBehaviour
{
    public static PlayerGuns instance;
    public GameObject crossHair;
    public GameObject bulletPrefab;

    [SerializeField] private float fireRate = 0.5f;
    private float cooldown = 0;
    private void Awake()
    {
        instance = this;
        gameObject.SetActive(true);
    }


    void Update()
    {
        cooldown -= Time.deltaTime;

        if (cooldown <= 0)
        {
            Shoot();
            cooldown = fireRate;
        }
    }

    private void Shoot()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        crossHair.transform.position = new Vector3(mousePos.x, mousePos.y, 0f);

        Vector2 dir = mousePos - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetMouseButtonDown(0))
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
}
