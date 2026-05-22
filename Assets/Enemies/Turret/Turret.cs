using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public int health = 100;

    [SerializeField] private float angle = 0;
    [SerializeField] private float pauzeTimer = 0;
    [SerializeField] private float interestTimer = 0;
    private bool state = true; // true = rotating right, false = rotating left

    [Header("Lasers")]
    private GameObject target;
    public List<LineRenderer> laserLR;

    [Header("Materials")]
    public Material redMat;
    public Material greenMat;

    [Header("Shooting")]
    public GameObject barrel;
    public GameObject bulletPrefab;
    public float shootTimer = 1f;

    void Update()
    {
        Idle();
        Track();
        LoseInterest();
        CheckSurrounding();
        Shoot();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(transform.root.gameObject);
        }
    }

    private void Shoot()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer > 0) return;

        if (target != null)
        {
            if (DataToLoad.bulletObjects.Count > 0)
            {
                DataToLoad.bulletObjects[0].transform.SetPositionAndRotation(barrel.transform.position, transform.rotation);
                DataToLoad.bulletObjects[0].SetActive(true);
                DataToLoad.bulletObjects.Remove(DataToLoad.bulletObjects[0]);
            }
            else
            {
                Instantiate(bulletPrefab, barrel.transform.position, transform.rotation);
            }
        }
        Debug.Log(DataToLoad.bulletObjects.Count);
        shootTimer = 1f;
    }

    private void Idle()
    {
        if (target != null || interestTimer > 0) return;

        pauzeTimer -= Time.deltaTime;
        if (pauzeTimer <= 0)
        {
            if (state)
            {
                angle += 20 * Time.deltaTime;
            }
            else
            {
                angle -= 20 * Time.deltaTime;
            }
            angle = Mathf.Clamp(angle, -75, 75);

            if (angle >= 75)
            {
                state = false;
                pauzeTimer = 5;
            }
            if (angle <= -75)
            {
                state = true;
                pauzeTimer = 5;
            }
        }
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Track()
    {
        if (target == null) return;

        Vector2 dir = target.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, targetAngle), 100 * Time.deltaTime);
    }

    private void LoseInterest()
    {
        interestTimer -= Time.deltaTime;

        if (interestTimer <= 0)
        {
            target = null;
            angle = transform.rotation.eulerAngles.z;
            if (angle > 180)
                angle -= 360;

        }
    }
    private void CheckSurrounding()
    {
        Vector2 origin = transform.position;
        Vector2 dir = -transform.up;
        float distance = 10f;

        laserLR[0].SetPosition(0, origin);
        //laserLR[1].SetPosition(0, origin);
        //laserLR[2].SetPosition(0, origin);

        RaycastHit2D middleHit = Physics2D.Raycast(origin, dir, distance);
        RaycastHit2D leftHit = Physics2D.Raycast(origin, Quaternion.Euler(0, 0, 7.5f) * dir, distance);
        RaycastHit2D rightHit = Physics2D.Raycast(origin, Quaternion.Euler(0, 0, -7.5f) * dir, distance);

        bool detected1 = Laser(middleHit, laserLR[0], origin, dir, distance, false, true);
        bool detected2 = Laser(leftHit, laserLR[1], origin, Quaternion.Euler(0, 0, 7.5f) * dir, distance, detected1, false);
        Laser(rightHit, laserLR[2], origin, Quaternion.Euler(0, 0, -7.5f) * dir, distance, detected1 || detected2, false);
    }

    public bool Laser(RaycastHit2D hit, LineRenderer laserLR, Vector2 origin, Vector2 dir, float distance, bool detected, bool debug)
    {
        bool hitInThisRayCast = false;
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player") && !Player.isInvisible)
            {
                target = hit.collider.gameObject;
                interestTimer = 999999;
                laserLR.material = greenMat;
                hitInThisRayCast = true;
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Block Raycast") && target != null && !detected)
            {
                target = null;
                interestTimer = 5;
                laserLR.material = redMat;
            }
            else
            {
                laserLR.material = redMat;
            }

            if (debug)
                laserLR.SetPosition(1, hit.point);
        }
        else
        {
            laserLR.material = redMat;

            if (debug)
                laserLR.SetPosition(1, origin + dir * distance);
        }
        return hitInThisRayCast;
    }
}
