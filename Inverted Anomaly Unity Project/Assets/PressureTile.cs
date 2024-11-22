using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PressureTile : MonoBehaviour
{

    float maxHeight = 5f;
    float step = 0.05f;
    private bool isActivated;

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
        if (!globalRewinder.fireStartRewind && maxHeight <= transform.position.y && isActivated)
        {   
            transform.position -= new Vector3(0f, step, 0f);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Bullet")
        {
            isActivated = true;
        }
    }
}
