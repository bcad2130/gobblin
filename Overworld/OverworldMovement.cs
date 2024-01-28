// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class OverworldMovement : MonoBehaviour
{
    public float speed = 1f;
    private StatusTracker tracker;

    public GameObject StatusTrackerObj;

    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey(KeyCode.W))
        {
            pos.y += speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            pos.y -= speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            pos.x += speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            pos.x -= speed * Time.deltaTime;
        }

        transform.position = pos;
    }

    void Awake()
    {
        tracker = GameObject.FindObjectOfType<StatusTracker>();

        if (tracker == null) {
            Instantiate(StatusTrackerObj);
        }
    }
}
