using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstMove : MonoBehaviour
{

    public Vector3[] points;
    public int pointNo;
    private Vector3 currentTarget;

    public float tolerance;
    public float speed;
    public float delay;

    private float delayStart;

    public bool auto;

    // Start is called before the first frame update
    void Start()
    {
     
        if (points.Length > 0)
        {
            currentTarget = points[0];
        }
        tolerance = speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != currentTarget)
        {
            MoveObst();
        }
        else
        {
            UpdateTarget();
        }
    }

    void MoveObst()
    {
        Vector3 heading = currentTarget - transform.position;
        transform.position += (heading / heading.magnitude) * speed * Time.deltaTime;
        if (heading.magnitude < tolerance)
        {
            transform.position = currentTarget;
            delayStart = Time.time;
        }
    }

    void UpdateTarget()
    {
        if (auto)
        {
            if (Time.time - delayStart > delay)
            {
                NextDest();
            }
        }
    }

    void NextDest()
    {
        pointNo++;
        if (pointNo >= points.Length)
        {
            pointNo = 0;
        }
        currentTarget = points[pointNo];
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController otherPlayer = other.gameObject.GetComponent<PlayerController>();
        otherPlayer.transform.parent = transform;
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController otherPlayer = other.gameObject.GetComponent<PlayerController>();
        otherPlayer.transform.parent = null;
    }

}
