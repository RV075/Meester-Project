using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public partial class Boss : MonoBehaviour, IDamageable
{
    [SerializeField] private Door door;

    [SerializeField] private Image healthBar;
    [SerializeField] private float maxHealth = 10000;
    private float health;

    [SerializeField] private BossShield shield;
    private readonly Dictionary<string, bool> phases = new()
    {
        { "phase1", false },
        { "phase2", false },
        { "phase3", false }
    };

    [SerializeField] private Coroutine newPhase;
    [SerializeField] private Transform lookPoint;

    private string currentGun = "minigun";

    [Header("minigun")]
    private Coroutine minigunCoroutine;
    [SerializeField] private Transform minigunBarrel;
    [SerializeField] private GameObject bulletPrefab;
    private int minigunChance = 7;
    [Header("rocket")]
    private Coroutine rocketCoroutine;
    [SerializeField] private Transform rocketBarrel;
    [SerializeField] private GameObject rocketPrefab;
    private int rocketChance = 3;
    [Header("laser")]
    private Coroutine laserCoroutine;
    [SerializeField] private LineRenderer laserLR;
    [SerializeField] private Transform laserRotatePart;
    [SerializeField] private Transform laserBarrel;
    private int laserChance = 0;

    private Quaternion minigunStartRot;
    private Quaternion rocketStartRot;
    private Quaternion laserStartRot;

    // ---Guns---
    // laser
    // minigun
    // rockets

    // ---Attacks---
    // minigun - shoots bullets in patterns
    // rockets - shoots homing missles
    // laser = shoots a powerfull beam across the room

    // ---Variables---
    // remember last attack

    // ---Phase 1---
    // attack speed = 1
    // minigun - 7 / 10
    // rockets - 3 / 10
    // laser - 0 / 10

    // ---Phase 2---
    // attack speed = 1.5
    // start: laser
    // minigun - 5 / 10
    // rockets - 3 / 10
    // laser - 2 / 10

    // ---Phase 3---
    // attack speed = 2
    // minigun - 4 / 10
    // rockets - 3 / 10
    // laser - 3 / 10

    // ---Main loop---

    private int multiplier = 1;
    public float gunInterval = 5; // seconds!
    private readonly int bulletsPerLaunch = 15;
    private readonly int rocketsPerLaunch = 4;

    public void TakeDamage(int damage, bool _)
    {
        if (health <= 0 || newPhase != null) return;

        health -= damage;

        healthBar.fillAmount = Mathf.Clamp01(health / maxHealth);

        if (health <= 0)
        {
            // explode animation;
            enabled = false;
            StopAllCoroutines();
            StartCoroutine(door.Open());
            ResetAndCLean();
        }
    }
    void Start()
    {
        health = maxHealth;
        StartCoroutine(ChooseNewGun());

        minigunStartRot = minigunBarrel.rotation;
        rocketStartRot = rocketBarrel.rotation;
        laserStartRot = laserRotatePart.rotation;
    }
    private void Update()
    {
        Phases();
    }
    private string GetRandomGun()
    {
        int total = minigunChance + rocketChance + laserChance;
        int roll = Random.Range(0, total);

        if (roll < minigunChance)
            return "minigun";

        roll -= minigunChance;

        if (roll < rocketChance)
            return "rocket";
        return "laser";
    }
    private void Phases()
    {
        if (health == maxHealth && !phases["phase1"])
        {
            multiplier = 1;
            shield.Activate(multiplier);
            phases["phase1"] = true;
        }
        else if (health <= maxHealth / 3 * 2 && !phases["phase2"])
        {
            minigunChance = 5;
            rocketChance = 3;
            laserChance = 2;
            StopAllCoroutines();
            newPhase ??= StartCoroutine(EnterNewFase(2));
        }
        else if (health <= maxHealth / 3 && !phases["phase3"])
        {
            minigunChance = 4;
            rocketChance = 3;
            laserChance = 3;
            StopAllCoroutines();
            newPhase ??= StartCoroutine(EnterNewFase(3));
        }
    }
    private IEnumerator EnterNewFase(int multi)
    {
        phases["phase" + multi] = true;
        Player.canMove = false;
        DataToLoad.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        DataToLoad.player.GetComponent<Dash>().CancelDash();

        ResetAndCLean();

        Vector3 origin = Camera.main.transform.position;
        yield return StartCoroutine(FocusCamera.instance.Focus(2, lookPoint, 2));
        yield return new WaitForSeconds(5);
        shield.Activate(multiplier);
        yield return StartCoroutine(FocusCamera.instance.UnFocus(1, origin, 3));

        StartCoroutine(ChooseNewGun());
        laserCoroutine ??= StartCoroutine(ShootLaser());
        gunInterval -= 2f;
        Player.canMove = true;
        multiplier = multi;
        newPhase = null;
    }
    private void ResetAndCLean()
    {
        Bullet[] bullets = FindObjectsOfType<Bullet>();
        BossRockets[] rockets = FindObjectsOfType<BossRockets>();

        foreach (Bullet bullet in bullets)
            bullet.DisableOnCall();
        foreach (BossRockets rocket in rockets)
            rocket.DisableOnCall();

        minigunCoroutine = null;
        rocketCoroutine = null;
        laserCoroutine = null;

        minigunBarrel.rotation = minigunStartRot;
        rocketBarrel.rotation = rocketStartRot;
        laserRotatePart.rotation = laserStartRot;
        laserLR.enabled = false;
    }
}