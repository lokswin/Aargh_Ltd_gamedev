using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contoller : MonoBehaviour
{
    private Vector2 displace;
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
        displace = new Vector2(0.5f * MoveY, 1);
        Motions.SetFloat("X_Speed", Mathf.Abs(MoveX));
        if (MoveX < 0)
            Sprite.flipX = true;
        else if (MoveX > 0)
            Sprite.flipX = false;
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
