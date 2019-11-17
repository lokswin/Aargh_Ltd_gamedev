using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contoller : MonoBehaviour
{
    public Transform EyeRay;
    public Vector2 displace_aiming;
    private bool is_enable2fly;
    private bool is_XY;
    private bool is_swiming;

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
        Player = GetComponent<Rigidbody2D>();
        P_Collider = GetComponent<BoxCollider2D>();
        ColliderVertical = P_Collider.size;
        ColliderHorizontal = new Vector2(ColliderVertical.y, ColliderVertical.x);
        Motions = GetComponent<Animator>();
        Sprite = GetComponent<SpriteRenderer>();
        Reload();
    }

    public void Reload()
    {
        float random = Random.Range(-1.0f, 1.0f);

        if (random > 0)
        {
            is_enable2fly = true;
            is_XY = true;
            Debug.Log("I can Fly!");
        }
        else
        {
            is_enable2fly = false;
            is_XY = false;
            Debug.Log("I can Swim!");
        }

        displace_X = new Vector2(1, Random.Range(-0.4f, -0.1f));
        displace_Y = new Vector2(Random.Range(0.1f, 0.6f), -1);
        displace_aiming = new Vector2(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f));
    }

    void FixedUpdate()
    {
        float MoveX = Input.GetAxis("Horizontal");
        float MoveY = Input.GetAxis("Vertical");

        Vector2 velocity = Vector2.zero;

        Motions.SetFloat("X_Speed", Mathf.Abs(MoveX));

        if (MoveX < 0) Sprite.flipX = true;
        else if (MoveX > 0) Sprite.flipX = false;

        if (Mathf.Abs(Player.velocity.y) > 0.0f)
        {
            P_Collider.size = ColliderHorizontal;
            if (is_enable2fly) Motions.SetBool("is_flying", true);
        }
        else
        {
            P_Collider.size = ColliderVertical;
            if (is_enable2fly) Motions.SetBool("is_flying", false);
        }

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
            Motions.SetBool("is_swiming", true);
            is_XY = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            Motions.SetBool("is_swiming", false);
            if (!is_enable2fly)
                is_XY = false;
        }
    }

    public void Fire(Vector3 aim)
    {
        Debug.Log("Fire!");
        EyeRay.rotation = Quaternion.FromToRotation(Vector3.right, this.transform.position - aim);
        Vector3 scale = new Vector3(Vector3.Distance(this.transform.position, aim) / this.transform.localScale.x, EyeRay.localScale.y, 1);
        EyeRay.localScale = scale;
    }

}
