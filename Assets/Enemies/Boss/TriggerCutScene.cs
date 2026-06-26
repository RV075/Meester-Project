using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCutScene : MonoBehaviour
{
    [SerializeField] private Door door;
    [SerializeField] private List<GameObject> lasers;

    [SerializeField] private GameObject LookTarget;
    [SerializeField] private List<GameObject> toDisable;

    [SerializeField] private GameObject gun;
    [SerializeField] private Boss boss;

    [SerializeField] private AudioClip musicClip;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.canMove = false;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            collision.gameObject.TryGetComponent<Dash>(out var dash); dash.CancelDash();
            if (PlayerData.data.gameData.CheckPoint != 3)
                StartCoroutine(door.Close());
            StartCoroutine(Cutscene());
        }
    }

    private IEnumerator Cutscene()
    {
        if (MusicPLayer.instance != null)
            MusicPLayer.instance.SwitchMusic(musicClip, 1);

        yield return new WaitForSeconds(2.5f);
        foreach (GameObject go in toDisable)
            go.SetActive(false);

        Vector3 origin = Camera.main.transform.position;
        yield return StartCoroutine(FocusCamera.instance.Focus(5, LookTarget.transform, 2));
        yield return StartCoroutine(FocusCamera.instance.UnFocus(2, origin, 3));

        Camera.main.transform.SetParent(null);
        Player.canMove = true;
        gun.SetActive(true);
        boss.enabled = true;
        foreach (GameObject laser in lasers) laser.SetActive(true);
        Destroy(GetComponent<BoxCollider2D>());
    }
}  