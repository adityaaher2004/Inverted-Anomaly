using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleCube : MonoBehaviour
{

    Rigidbody rb;

    Rewindable rewinder;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rewinder = new Rewindable(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
        // Need this snippet to check if rewinding
        // --------------------------------
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            StartRewind();
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            StopRewind();
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

    void StartRewind()
    {
        rewinder.StartRewind();
    }

    void StopRewind()
    {
        rewinder.StopRewind();
    }

}
