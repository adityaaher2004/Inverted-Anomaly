using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleCube : MonoBehaviour
{

    public bool isRewinding;
    List<PointInTime> points;

    Rigidbody rb;

    Rewindable rewinder;

    // Start is called before the first frame update
    void Start()
    {
        points = new List<PointInTime>();
        isRewinding = false;
        rb = GetComponent<Rigidbody>();
        rewinder = new Rewindable(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            rewinder.StartRewind();

        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            rewinder.StopRewind();
        }

    }

    void FixedUpdate()
    {
        rewinder.physUpdate();
    }
}
