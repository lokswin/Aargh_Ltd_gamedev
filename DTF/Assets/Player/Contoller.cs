using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contoller : MonoBehaviour
{
    private bool is_enable2fly;
    private bool is_XY;
    private Vector2 displace_X;
    private Vector2 displace_Y;
    public float speed = 5;
    private Rigidbody2D Player;
    private BoxCollider2D P_Collider;
    private Vector2 ColliderVertical;
    private Vector2 ColliderHorizontal;
    private Animator Motions;
    private SpriteRenderer Sprite;

    // Start is called before the first frame update
    void Start()
    {
        is_enable2fly = false;
        is_XY = false;
        Player = GetComponent<Rigidbody2D>();
        P_Collider = GetComponent<BoxCollider2D>();
        ColliderVertical = P_Collider.size;
        ColliderHorizontal = new Vector2(ColliderVertical.y, ColliderVertical.x);
        Motions = GetComponent<Animator>();
        Sprite = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        float MoveX = Input.GetAxis("Horizontal");
        float MoveY = Input.GetAxis("Vertical");

        Vector2 velocity = Vector2.zero;

        displace_X = new Vector2(1, -0.2f);
        displace_Y = new Vector2(0.5f, -1);

        Motions.SetFloat("X_Speed", Mathf.Abs(MoveX));
        if (!is_enable2fly && is_XY)
        {
            Motions.SetBool("is_swiming", true);
            P_Collider.size = ColliderHorizontal;
        }
        else
        { 
            Motions.SetBool("is_swiming", false);
            P_Collider.size = ColliderVertical;
        }
        if (MoveX < 0)
            Sprite.flipX = true;
        else if (MoveX > 0)
            Sprite.flipX = false;
        if (Mathf.Abs(MoveX) > 0.0f)
        {
            velocity += displace_X * MoveX * speed;
        }
        if (Mathf.Abs(MoveY) > 0.0f && is_XY)
        {
            velocity += displace_Y * MoveY * speed;
        }
        Player.velocity = velocity;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            is_XY = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            is_XY = false;
        }
    }

}
