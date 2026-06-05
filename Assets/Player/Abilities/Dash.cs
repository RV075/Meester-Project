using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dash : MonoBehaviour
{
    private Coroutine dashCoroutine;
    private SpriteRenderer playerSR;
    private Rigidbody2D playerRB;

    private readonly List<SpriteRenderer> dashObjectsToFade = new();

    private void Start()
    {
        playerSR = GetComponent<SpriteRenderer>();
        playerRB = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
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

        playerRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * 17.5f, Input.GetAxisRaw("Vertical") * 7.5f);
        playerRB.gravityScale = 0;
        Player.canMove = false; Player.isInvisible = true;

        for (int i = 0; i < 10; i++)
        {
            DataToLoad.dashSpritesRenderer[i].color = new Color(1, 1, 1, 1);

            DataToLoad.dashObjects[i].transform.position = transform.position;
            DataToLoad.dashSpritesRenderer[i].sprite = playerSR.sprite;
            DataToLoad.dashSpritesRenderer[i].flipX = playerSR.flipX;

            dashObjectsToFade.Add(DataToLoad.dashSpritesRenderer[i]);
            yield return new WaitForSeconds(0.03f);
        }

        playerRB.gravityScale = 1; playerRB.velocity = new Vector2(playerRB.velocity.x, playerRB.velocity.y / 2);
        Player.canMove = true; Player.isInvisible = false;
        dashCoroutine = null;
    }
}
