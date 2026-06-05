using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fallbuilding : MonoBehaviour
{
    public bool isFalling = false;
    public Rigidbody2D fallingrb;
    public float fallSpeed = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        fallingrb.isKinematic = false;
        fallingrb.simulated = true;
        fallingrb.gravityScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFalling)
        {
            if (fallingrb != null)
            {
                fallingrb.MovePosition(fallingrb.position + Vector2.down * fallSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isFalling)
        {
            Debug.Log("Collided with player");
            isFalling = true;
        }
    }   
}