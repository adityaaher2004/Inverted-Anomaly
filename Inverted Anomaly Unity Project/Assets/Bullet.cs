using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{

    [SerializeField] public float startImpulse = 1000f;

    Rigidbody rb;
    Rewindable rewinder;
    GlobalIsRewindingScript globalRewinder;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rewinder = new Rewindable(gameObject, true);
        globalRewinder = GameObject.FindFirstObjectByType<GlobalIsRewindingScript>();

        rb.AddForce(transform.up * startImpulse, ForceMode.Impulse);
        Debug.Log($"Bullet Spawned at {transform.position}");
    }

    void Update()
    {
        if (globalRewinder.fireStartRewind)
        {
            rewinder.StartRewind();
        }
        if (globalRewinder.fireStopRewind)
        {
            rewinder.StopRewind();
        }
    }

    void FixedUpdate()
    {
        rewinder.physUpdate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = rb.velocity * 0.05f;
    }

}
