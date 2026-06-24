using System.Collections;
using UnityEngine;

public partial class Boss : MonoBehaviour
{
    private IEnumerator ChooseNewGun()
    {
        while (true)
        {
            yield return new WaitUntil(() => newPhase == null);

            Shoot();
            yield return new WaitForSeconds(gunInterval);
        }
    }

    private void Shoot()
    {
        for (int i = 0; i < 10; i++)
        {
            currentGun = GetRandomGun();

            bool gunFree =
                (currentGun == "minigun" && minigunCoroutine == null) ||
                (currentGun == "rocket" && rocketCoroutine == null) ||
                (currentGun == "laser" && laserCoroutine == null);

            if (gunFree)
                break;
        }

        switch (currentGun)
        {
            case "minigun":
                minigunCoroutine ??= StartCoroutine(ShootMinigun());
                break;

            case "rocket":
                rocketCoroutine ??= StartCoroutine(ShootRockets());
                break;

            case "laser":
                laserCoroutine ??= StartCoroutine(ShootLaser());
                break;
        }
    }

    private IEnumerator ShootMinigun()
    {
        int number = Random.Range(0, 4);

        switch (number)
        {
            case 0:
                yield return StartCoroutine(Shotgun());
                break;
            case 1:
                yield return StartCoroutine(Spinner());
                break;
            case 2:
                Explosion();
                break;
            case 3:
                yield return StartCoroutine(SpinnerMulti());
                break;
            case 4:
                yield return StartCoroutine(SpinnerMulti());
                break;
        }

        minigunCoroutine = null;
    }


    private IEnumerator Shotgun()
    {
        foreach (int angle in new int[] { -60, 0, 60 })
        {
            yield return StartCoroutine(RotateMinigun(angle, 1.5f - 0.25f * (multiplier - 1)));
            for (int i = 0; i < multiplier; i++)
            {
                for (int j = 0; j < bulletsPerLaunch * multiplier; j++)
                    MinigunShootHelper(Quaternion.Euler(0, 0, minigunBarrel.eulerAngles.z - 30f + j * (60f / (bulletsPerLaunch * multiplier - 1))));
                yield return new WaitForSeconds(0.33f);
            }
            yield return new WaitForSeconds(1);
        }

        yield return StartCoroutine(RotateMinigun(0, 1.5f));
    }

    private IEnumerator Spinner()
    {
        float duration = 5f;
        float timer = 0f;
        float shootInterval = 0.75f / bulletsPerLaunch;
        float shootTimer = 0f;

        while (timer < duration)
        {
            minigunBarrel.Rotate(0, 0, 360f * Time.deltaTime);

            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                shootTimer = 0f;
                MinigunShootHelper(Quaternion.Euler(0, 0, minigunBarrel.eulerAngles.z));
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

    private void Explosion()
    {
        for (int i = 0; i < 180; i++)
            MinigunShootHelper(Quaternion.Euler(0, 0, i * 2));
    }
    private IEnumerator SpinnerMulti()
    {
        float duration = 7.5f;
        float timer = 0f;
        float shootInterval = 1.5f / bulletsPerLaunch;
        float shootTimer = 0f;
        int counter = 0;

        while (timer < duration)
        {
            minigunBarrel.rotation = Quaternion.Euler(0, 0, counter);

            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                shootTimer = 0f;
                MinigunShootHelper(Quaternion.Euler(0, 0, 45 + minigunBarrel.eulerAngles.z));
                MinigunShootHelper(Quaternion.Euler(0, 0, 135 + minigunBarrel.eulerAngles.z));
                MinigunShootHelper(Quaternion.Euler(0, 0, 225 + minigunBarrel.eulerAngles.z));
                MinigunShootHelper(Quaternion.Euler(0, 0, 315 + minigunBarrel.eulerAngles.z));
            }
            counter++;
            timer += Time.deltaTime;
            yield return null;
        }
        minigunBarrel.rotation = Quaternion.Euler(0, 0, 0);
    }


    private IEnumerator RotateMinigun(float angle, float duration)
    {
        Quaternion startRot = minigunBarrel.rotation;
        Quaternion endRot = Quaternion.Euler(0, 0, angle);

        float timer = 0;

        while (timer < duration)
        {
            float t = timer / duration;

            minigunBarrel.rotation = Quaternion.Lerp(startRot, endRot, t);

            timer += Time.deltaTime;
            yield return null;
        }
    }

    private void MinigunShootHelper(Quaternion angle)
    {
        if (newPhase != null) return;

        if (DataToLoad.bulletObjects.Count > 0 && DataToLoad.bulletObjects[0] != null)
        {
            DataToLoad.bulletObjects[0].transform.SetPositionAndRotation(minigunBarrel.position, angle);
            DataToLoad.bulletObjects[0].SetActive(true);
            DataToLoad.bulletObjects.Remove(DataToLoad.bulletObjects[0]);
        }
        else
        {
            Instantiate(bulletPrefab, minigunBarrel.position, angle);

        }
    }

    private IEnumerator ShootRockets()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < rocketsPerLaunch * multiplier; i++)
        {
            Quaternion angle = rocketBarrel.rotation * Quaternion.Euler(0, 0, -30f + i * (330f / (rocketsPerLaunch * multiplier - 1)));
            if (DataToLoad.rocketObjects.Count > 0 && DataToLoad.rocketObjects[0] != null)
            {
                DataToLoad.rocketObjects[0].transform.SetPositionAndRotation(rocketBarrel.position, angle);
                DataToLoad.rocketObjects[0].GetComponent<BossRockets>().Enable();
                DataToLoad.rocketObjects.Remove(DataToLoad.rocketObjects[0]);
            }
            else
            {
                Instantiate(rocketPrefab, rocketBarrel.position, angle);
            }
            yield return new WaitForSeconds(3 / rocketsPerLaunch * multiplier);
        }
        yield return new WaitForSeconds(5);

        rocketCoroutine = null;
    }

    private string laserDir = "left";
    private IEnumerator ShootLaser()
    {
        float angleToRotateTo = laserDir == "left" ? -90 : 90;

        yield return StartCoroutine(RotateLaser(angleToRotateTo));

        float opposite = -angleToRotateTo;
        yield return new WaitForSeconds(1f);

        Quaternion startRot = laserRotatePart.rotation;

        float timer = 0;

        foreach (int angle in new int[] { (int)opposite, (int)angleToRotateTo })
        {
            Quaternion endRot = Quaternion.Euler(0, 0, angle);

            while (timer < gunInterval)
            {
                float t = timer / gunInterval;
                laserRotatePart.rotation = Quaternion.Lerp(startRot, endRot, t);

                laserLR.SetPosition(0, laserBarrel.position);
                laserLR.enabled = true;

                RaycastHit2D hit = Physics2D.Raycast(laserBarrel.position, -laserBarrel.up, 100);
                if (hit.collider != null)
                {
                    laserLR.SetPosition(1, hit.point);

                    if (hit.collider.CompareTag("Player"))
                        hit.collider.GetComponent<IDamageable>()?.TakeDamage(999999, false);
                }
                else
                {
                    laserLR.SetPosition(1, laserBarrel.position + -laserBarrel.up * 100);
                }

                timer += Time.deltaTime;
                yield return null;
            }
            timer = 0;
            startRot = laserRotatePart.rotation;
            if (multiplier < 3) break;
        }

        laserLR.enabled = false;
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(RotateLaser(0));

        laserDir = laserDir == "left" ? "right" : "left";
        laserCoroutine = null;
    }

    private IEnumerator RotateLaser(float angle)
    {
        while (Quaternion.Angle(laserRotatePart.rotation, Quaternion.Euler(0, 0, angle)) > 1f)
        {
            laserRotatePart.rotation = Quaternion.Lerp(
                laserRotatePart.rotation,
                Quaternion.Euler(0, 0, angle),
                Time.deltaTime * 2
            );
            yield return null;
        }
    }
}
