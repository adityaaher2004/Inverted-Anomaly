using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleCube : MonoBehaviour
{

    // Simplest implementation of Rewindable

    Rigidbody rb;

    Rewindable rewinder;

    GlobalIsRewindingScript globalRewinder;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rewinder = new Rewindable(gameObject);
        globalRewinder = GameObject.FindFirstObjectByType<GlobalIsRewindingScript>();
    }

    // Update is called once per frame
    void Update()
    {

        // Need this snippet to check if rewinding
        // --------------------------------
        if (globalRewinder.fireStartRewind)
        {
            rewinder.StartRewind();
        }
        if (globalRewinder.fireStopRewind)
        {
            rewinder.StopRewind();
        }
        // --------------------------------

    }

    void FixedUpdate()
    {
        // Need this line to apply rewinding
        // --------------------------------
        rewinder.physUpdate();
        // --------------------------------
    }

}
