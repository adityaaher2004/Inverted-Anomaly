using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turnstile : MonoBehaviour
{
    Stack<Quaternion> rotations;
    bool isRewinding;

    GlobalIsRewindingScript globalRewinder;

    // Start is called before the first frame update
    void Start()
    {
        rotations = new Stack<Quaternion>();
        isRewinding = false;

        globalRewinder = GameObject.FindFirstObjectByType<GlobalIsRewindingScript>();

    }

    // Update is called once per frame
    void Update()
    {
        if (globalRewinder.fireStartRewind)
        {
            StartRewind();
        }
        if (globalRewinder.fireStopRewind)
        {
            StopRewind();
        }
    }

    private void FixedUpdate()
    {
        physUpdate();
    }

    void Record()
    {
        transform.Rotate(0, 1, 0);
        rotations.Push(transform.rotation);
    }

    void Rewind()
    {
        if (rotations.Count > 0)
        {
            transform.rotation = rotations.Pop();
        }
        else
        {
            StopRewind();
        }
    }

    void StartRewind()
    {
        isRewinding = true;
    }

    void StopRewind()
    {
        isRewinding = false;
    }

    void physUpdate()
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

}
