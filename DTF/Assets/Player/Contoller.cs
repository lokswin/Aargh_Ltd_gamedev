using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contoller : MonoBehaviour
{
    private Vector2 displace_X;
    private Vector2 displace_Y;
    public float speed = 5;
    private Rigidbody2D Player;
    private Animator Motions;
    private SpriteRenderer Sprite;

    // Start is called before the first frame update
    void Start()
    {
        Player = GetComponent<Rigidbody2D>();
        Motions = GetComponent<Animator>();
        Sprite = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        float MoveX = Input.GetAxis("Horizontal");
        float MoveY = Input.GetAxis("Vertical");

        Vector2 velocity = Vector2.zero;

        displace_X = new Vector2(1, 0);
        displace_Y = new Vector2(0, 1);

        Motions.SetFloat("X_Speed", Mathf.Abs(MoveX));
        Motions.SetFloat("Y_Speed", Mathf.Abs(MoveY));
        if (MoveX < 0)
            Sprite.flipX = true;
        else if (MoveX > 0)
            Sprite.flipX = false;
        if (Mathf.Abs(MoveX) > 0.0f)
        {
            velocity += displace_X * MoveX * speed;
        }
        if (Mathf.Abs(MoveY) > 0.0f)
        {
            velocity += displace_Y * MoveY * speed;
        }
        Player.velocity = velocity;
    }
}
