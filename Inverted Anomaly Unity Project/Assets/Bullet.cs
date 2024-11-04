using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{

    [SerializeField] public float startImpulse = 1000f;

    public bool globalIsRewinding;

    Rigidbody rb;
    Rewindable rewinder;

    public Vector3 SpawnLocation
    {
        get;
        private set;
    }

    [SerializeField] private float DelayedDisableTime = 5f;

    public delegate void CollisionEvent(Bullet bullet, Collision collision);
    public event CollisionEvent OnCollision;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        globalIsRewinding = false;
    }

    public void Spawn(Vector3 SpawnForce)
    {
        SpawnLocation = transform.position;
        transform.forward = SpawnLocation.normalized;
        rb.AddForce(SpawnForce, ForceMode.Impulse);
        StartCoroutine(DelayedDisabled(DelayedDisableTime));

    }

    private IEnumerator DelayedDisabled(float delay) {
        yield return new WaitForSeconds(delay);
        OnCollisionEnter(null);
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnCollision?.Invoke(this, collision);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        rb.velocity = Vector3.zero; 
        rb.angularVelocity = Vector3.zero;
        OnCollision = null;
    }

    void Start()
    {
        rewinder = new Rewindable(gameObject, true);
        Debug.Log($"Bullet Spawned at {transform.position}");
    }

    void Update()
    {
        if (globalIsRewinding)
        {
            rewinder.StartRewind();
        }
        if (globalIsRewinding)
        {
            rewinder.StopRewind();
        }
    }

    void FixedUpdate()
    {
        rewinder.physUpdate(globalIsRewinding);
    }
}
