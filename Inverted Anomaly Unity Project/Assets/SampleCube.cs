using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleCube : MonoBehaviour
{

    public bool isRewinding;
    List<PointInTime> points;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        points = new List<PointInTime>();
        isRewinding = false;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            StartRewind();
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            StopRewind();
        }

    }

    void FixedUpdate()
    {
        if (isRewinding)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }

    public void Record()
    {
        points.Insert(0, new PointInTime(transform.position, transform.rotation));
    }
    
    public void Rewind()
    {
        if (points.Count > 0)
        {
            PointInTime latest = points[0];
            transform.position = latest.position;
            transform.rotation = latest.rotation;
            points.RemoveAt(0);

        }
        else
        {
            StopRewind();
        }
    }

    void StartRewind()
    {
        isRewinding = true;
        rb.isKinematic = true;
    }

    void StopRewind()
    {
        isRewinding = false;
        rb.isKinematic = false;
    }

}
