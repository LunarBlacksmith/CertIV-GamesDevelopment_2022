using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        Vector2 moveDir = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveDir.y += speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDir.y -= speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDir.x += speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDir.x -= speed * Time.deltaTime;
        }

        transform.position += (Vector3)moveDir;
    }
}
