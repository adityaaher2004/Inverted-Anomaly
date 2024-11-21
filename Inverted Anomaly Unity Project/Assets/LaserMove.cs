using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMove : MonoBehaviour
{

    private float direction = 1f;
    float minZ = -23f;
    float maxZ = 15f;
    float step = 0.05f;

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
        if (!globalRewinder.fireStartRewind)
        {
            MoveLaser();
        }
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

    private void FixedUpdate()
    {
        // Need this line to apply rewinding
        // --------------------------------
        rewinder.physUpdate();
        // --------------------------------
    }

    void MoveLaser()
    {
        if (transform.position.z <= minZ)
        {
            direction = 1f;
        }
        if (transform.position.z >= maxZ)
        {
            direction = -1f;
        }

        transform.position += new Vector3(0f, 0f, step * direction);
    }
}
