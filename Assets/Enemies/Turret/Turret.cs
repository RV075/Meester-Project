using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, IDamageable
{
    // ADD: zodra de speler niet gehit wordt door de lasers, dan onthouden we voor paar seconden dat we de speler hebben gezien, zodat de turret niet meteen terug gaat naar idle mode. (interestTimer)
    [Header("Stats")]
    public int health = 100;

    [SerializeField] private float angle = 0;
    [SerializeField] private float pauzeTimer = 0;
    [SerializeField] private float interestTimer = 0;
    private bool state = true; // true = rotating right, false = rotating left

    [Header("Lasers")]
    private GameObject target;
    [SerializeField] private List<LineRenderer> laserLR;

    [Header("Materials")]
    [SerializeField] private Material redMat;
    [SerializeField] private Material greenMat;

    [Header("Shooting")]
    [SerializeField] private GameObject barrel;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootCooldown = 1f;
    [SerializeField] private float shootTimer = 1f;
    [SerializeField] private float distance = 10;

    void Update()
    {
        Idle();
        Track();
        LoseInterest();
        CheckSurrounding();
        Shoot();
        LosePlayer();
    }

    private void LosePlayer()
    {
        if (target == null) return;

        float dist = Vector2.Distance(transform.position, target.transform.position);

        if (dist >= distance)
        {
            target = null;
            interestTimer = 0;
            pauzeTimer = 1;
        }
    }

    public void TakeDamage(int damage, bool ignoreInvisibility)
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

        shootCooldown -= 0.1f;
        shootCooldown = Mathf.Clamp(shootCooldown, 0.15f, 1f);
        shootTimer = shootCooldown;
    }

    private void Idle()
    {
        if (target != null || interestTimer > 0) return;

        shootTimer = shootCooldown = 1f;
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
        angle = Mathf.Clamp(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90, -75, 75);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), 100 * Time.deltaTime);
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
