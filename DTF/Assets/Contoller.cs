using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contoller : MonoBehaviour
{
    private Vector2 displace;
    public float speed = 5;
    private Rigidbody2D Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GetComponent<Rigidbody2D>();       
    }

    void FixedUpdate()
    {
        float MoveX = Input.GetAxis("Horizontal");
        float MoveY = Input.GetAxis("Vertical");
        Vector2 velocity = Vector2.zero;
        displace = new Vector2(0.5f * MoveY, 1);

        if (Mathf.Abs(MoveX) > 0.0f)
        {
            velocity += Vector2.right * MoveX * speed;
        }
        if (Mathf.Abs(MoveY) > 0.0f)
        {
            velocity += displace * MoveY * speed;
        }
        Player.velocity = velocity;
    }
}
