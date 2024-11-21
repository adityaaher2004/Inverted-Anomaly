using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{

    [SerializeField] public float startImpulse = 1000f;


    Rewindable rewinder;
    GlobalIsRewindingScript globalRewinder;


    Rigidbody rb;
    [field : SerializeField]

    public Vector3 SpwnLocation
    {
        get;
        private set;
    }

    public delegate void CollisionEvent(Bullet bullet, Collision collision);
    public event CollisionEvent OnCollision;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rewinder = new Rewindable(gameObject, true);
        globalRewinder = GameObject.FindFirstObjectByType<GlobalIsRewindingScript>();

        // rb.AddForce(transform.up * startImpulse, ForceMode.Impulse);
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
        OnCollision?.Invoke(this, collision);
    }

    public void Shoot(Vector3 direction)
    {
        rb.AddForce(direction * startImpulse, ForceMode.Impulse);
    }

}
