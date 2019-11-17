using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contoller : MonoBehaviour
{
    private GameObject[] waters;
    [HideInInspector]
    public Vector2 displace_aiming;
    [HideInInspector]
    public Vector3 eyes_position;
    public Transform EyePivotStand;
    public Transform EyePivotFly;
    public Transform EyePivotSwim;
    private bool is_enable2fly;
    [HideInInspector]
    public bool is_enable2tele;
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
        waters = GameObject.FindGameObjectsWithTag("Water");
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
        GameObject[] go2destroy = GameObject.FindGameObjectsWithTag("Destroy");
        foreach (GameObject go in go2destroy)
        {
            Destroy(go);
        }
        GameObject[] interactive = GameObject.FindGameObjectsWithTag("Interactive");
        foreach(GameObject go in interactive)
        {
            go.GetComponent<SpriteRenderer>().color = Color.white; 
            go.GetComponent<Collider2D>().enabled = true;
        }
        float random = Random.Range(-1.0f, 1.0f);

        if (random > 0)
        {
            is_enable2fly = true;
            foreach (GameObject go in waters)
                go.SetActive(false);
            is_XY = true;
            Debug.Log("I can Fly!");
        }
        else
        {
            is_enable2fly = false;
            foreach (GameObject go in waters)
                go.SetActive(true);
            is_XY = false;
            Debug.Log("I can Swim!");
        }
        random = Random.Range(-1.0f, 1.0f);
        if (random > 0)
        {
            is_enable2tele = true;
            Debug.Log("I have Telecinesis!");
        }
        else
        {
            is_enable2tele = false;
            Debug.Log("I have Laser!");
        }
        displace_X = new Vector2(1, Random.Range(-0.4f, -0.1f));
        displace_Y = new Vector2(Random.Range(0.1f, 0.6f), -1);
        displace_aiming = new Vector2(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f));
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        float MoveX = Input.GetAxis("Horizontal");
        float MoveY = Input.GetAxis("Vertical");

        Vector2 velocity = Vector2.zero;

        Motions.SetFloat("X_Speed", Mathf.Abs(MoveX));



        if (Mathf.Abs(Player.velocity.y) > 0.0f)
        {
            P_Collider.size = ColliderHorizontal;
            if (is_enable2fly)
            {
                Motions.SetBool("is_flying", true);
                eyes_position = EyePivotFly.position;
            }
        }
        else if (!is_swiming)
        {
            P_Collider.size = ColliderVertical;
            if (is_enable2fly) Motions.SetBool("is_flying", false);
            eyes_position = EyePivotStand.position;
        }
        if (is_swiming)
        {
            eyes_position = EyePivotSwim.position;
        }
        if (MoveX < 0)
        {
            Sprite.flipX = true;
            //eyes_position = new Vector3(eyes_position.x * -1.0f, eyes_position.y, eyes_position.z);
        }
        else if (MoveX > 0)
        {
            Sprite.flipX = false;

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
        if (collision.gameObject.tag == "Exit")
        {
            Debug.Log("Exit");
            //Camera.GetComponent<Transition>().nextLevel();
        }
        if (collision.gameObject.tag == "Water")
        {
            is_swiming = true;
            Motions.SetBool("is_swiming", true);
            is_XY = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            is_swiming = false;
            Motions.SetBool("is_swiming", false);
            if (!is_enable2fly)
                is_XY = false;
        }
    }

    public void Fire(Vector3 aim, float distance)
    {
        Debug.Log("Fire!");
        Vector2 direction = aim - eyes_position;

        RaycastHit2D hit = Physics2D.Raycast(eyes_position, direction, distance);
        //If something was hit, the RaycastHit2D.collider will not be null.
        if (hit.collider != null && (hit.collider.gameObject.tag == "Interactive" || hit.collider.gameObject.tag == "Destroy"))
        {
            GameObject RigidGO;
            if (is_enable2tele && hit.collider.gameObject.tag != "Destroy")
            {
                RigidGO = (GameObject)Instantiate(hit.collider.gameObject);
                RigidGO.AddComponent<Rigidbody2D>();
                RigidGO.tag = "Destroy";
                hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.clear;
                hit.collider.enabled = false;
            }
            else
            {
                RigidGO = hit.collider.gameObject;
            }
            Vector2 force = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * 5.0f;
            RigidGO.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        }
    }

}
