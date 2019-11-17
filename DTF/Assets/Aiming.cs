using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    public Contoller player;
    private Vector2 start_coord;
    private bool status_wait;
    private bool status_aiming;
    private bool status_fire;

    public float FireDelay = 1;
    private float fire_timer;

    // Start is called before the first frame update
    void Start()
    {
        status_wait = true;
        status_aiming = false;
        status_fire = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && status_wait)
        {
            start_coord =  new Vector2 (Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            this.transform.position = new Vector2((Random.Range(-8.0f, 8.0f)), Random.Range(-5.0f, 5.0f));
            status_wait = false;
            status_aiming = true;
        }
        if (Input.GetMouseButtonUp(0) && status_aiming)
        {
            status_aiming = false;
            status_fire = true;
            player.Fire(this.transform.position);
        }

        if (status_wait)
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, new Color(1, 1, 1, 0), Time.deltaTime * 3.0f);
        }
        if (status_aiming)
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, Color.white, Time.deltaTime * 3.0f);
            Vector2 pos = new Vector2(Mathf.Clamp(this.transform.position.x + Input.GetAxis("Mouse X") * player.displace_aiming.x, -8.0f, 8.0f), Mathf.Clamp(this.transform.position.y + Input.GetAxis("Mouse Y") * player.displace_aiming.y, -5.0f, 5.0f));
            this.transform.position = pos;
        }
        if (status_fire)
        {
            fire_timer += Time.deltaTime;
            if (fire_timer > FireDelay)
            {
                fire_timer = 0;
                status_fire = false;
                status_wait = true;
            }
        }
    }
}
