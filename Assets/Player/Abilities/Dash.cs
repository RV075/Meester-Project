using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private Coroutine dashCoroutine;
    private SpriteRenderer playerSR;
    private Rigidbody2D playerRB;

    public static int dashAmount = 0;
    private float timer = 0;

    private readonly List<SpriteRenderer> dashObjectsToFade = new();

    private void Start()
    {
        playerSR = GetComponent<SpriteRenderer>();
        playerRB = GetComponent<Rigidbody2D>();
        dashAmount = 0;
    }
    void Update()
    {
        timer += Time.deltaTime;

        bool pressedAKey = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S);
        if (Input.GetKeyDown(KeyCode.LeftShift) && Player.canMove && pressedAKey && dashAmount > 0)
        {
            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) return;
            else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) return;

            if (timer > 0.5f)
                dashCoroutine ??= StartCoroutine(DashCoroutine());
        }

        foreach (var dashObject in dashObjectsToFade)
        {
            Color color = dashObject.color;
            color.a -= Time.deltaTime;
            dashObject.color = color;
        }

        if (dashObjectsToFade.Count == 0) return;

        for (int i = dashObjectsToFade.Count - 1; i >= 0; i--)
        {
            if (dashObjectsToFade[i].color.a <= 0)
            {
                dashObjectsToFade.RemoveAt(i);
            }
        }
    }
    private IEnumerator DashCoroutine()
    {
        if (Player.moveDirectionX == 0 && Player.moveDirectionY == 0) yield break;

        Change();

        for (int i = 0; i < 10; i++)
        {
            if (DataToLoad.dashSpritesRenderer[i] == null) continue;

            DataToLoad.dashSpritesRenderer[i].color = new Color(1, 1, 1, 1);
            DataToLoad.dashObjects[i].transform.position = transform.position;
            DataToLoad.dashSpritesRenderer[i].sprite = playerSR.sprite;
            DataToLoad.dashSpritesRenderer[i].flipX = playerSR.flipX;

            dashObjectsToFade.Add(DataToLoad.dashSpritesRenderer[i]);
            yield return new WaitForSeconds(0.03f);
        }

        Reset_(true);
    }

    public void CancelDash()
    {
        if (dashCoroutine != null)
        {
            StopCoroutine(dashCoroutine);
            Reset_(false);
        }
    }

    private void Change()
    {
        playerRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * 17.5f, Input.GetAxisRaw("Vertical") * 7.5f);
        playerRB.gravityScale = 0;
        Player.canMove = false; Player.isInvisible = true;
    }

    private void Reset_(bool canMove)
    {
        playerRB.gravityScale = 1; playerRB.velocity = new Vector2(playerRB.velocity.x, playerRB.velocity.y / 2);
        Player.canMove = canMove; Player.isInvisible = false;
        dashAmount -= 1; timer = 0;
        dashCoroutine = null;
    }
}
