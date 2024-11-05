using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{

    [SerializeField] public float startImpulse = 1000f;

    Rigidbody rb;
    Rewindable rewinder;

    public bool isRewinding;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rewinder = new Rewindable(gameObject, true);
        rb.AddForce(transform.forward * startImpulse, ForceMode.Impulse);
        Debug.Log($"Bullet Spawned at {transform.position}");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            rewinder.StartRewind();
            isRewinding = true;
        }
        if (Input.GetKeyUp(KeyCode.T))
        {
            rewinder.StopRewind();
            isRewinding = false;
        }
    }

    void FixedUpdate()
    {
        rewinder.physUpdate(isRewinding);
    }
}
