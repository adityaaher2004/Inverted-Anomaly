using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;

public class Grenade : MonoBehaviour
{

    // Grenade Specific Variables
    // -------------------------
    [SerializeField] float cookTime = 0f;
    [SerializeField] bool isDestructable = false;
    float timer;
    [SerializeField] float blastRadius = 25f;
    [SerializeField] float blastForce = 5000f;
    bool hasExploded = false;
    bool grenadeIsRewinding;
    // ------------------------


    // Rewinder Variables
    // ------------------
    Rigidbody rb;
    Rewindable rewinder;
    Stack<bool> isActiveRewinderFrames;
    Stack<float> grenadeTimerFrames;

    GlobalIsRewindingScript globalRewinder;
    bool isMeshRendering;
    // ------------------

    void Start()
    {
        timer = cookTime;
        isMeshRendering = true;
        isActiveRewinderFrames = new Stack<bool>();
        grenadeIsRewinding = false;
        grenadeTimerFrames = new Stack<float>();
        globalRewinder = GameObject.FindFirstObjectByType<GlobalIsRewindingScript>();
        rewinder = new Rewindable(gameObject, isDestructable);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }

        // Need this snippet to check if rewinding
        // --------------------------------
        if (globalRewinder.fireStartRewind)
        {
            StartRewind();
        }
        if (globalRewinder.fireStopRewind)
        {
            StopRewind();
        }
        // --------------------------------
    }

    void FixedUpdate()
    {
        // Need this line to apply rewinding
        // --------------------------------
            physUpdate();
        // --------------------------------
    }

    void Explode()
    {

        Collider[] collisions = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (Collider collider in collisions)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(blastForce, transform.position, blastRadius);
            }
        }

        isMeshRendering = false;
        foreach (MeshRenderer mesh in gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            mesh.enabled = false;
        }
    }

    void Record()
    {
        isActiveRewinderFrames.Push(isMeshRendering);
        grenadeTimerFrames.Push(timer);
    }

    void Rewind()
    {
        if (isActiveRewinderFrames.Count > 0)
        {
            bool latestActiveStatus = isActiveRewinderFrames.Pop();
            foreach(MeshRenderer mesh in gameObject.GetComponentsInChildren<MeshRenderer>())
            {
                mesh.enabled = latestActiveStatus;
            }
            hasExploded = !latestActiveStatus;
            isMeshRendering = latestActiveStatus;
            timer = grenadeTimerFrames.Pop();

        }
        else
        {
            StopRewind();
            isMeshRendering = true;
        }
    }

    void StartRewind()
    {
        grenadeIsRewinding = true;
        rewinder.StartRewind();
    }

    void StopRewind()
    {
        grenadeIsRewinding = false;
        rewinder.StopRewind();
    }

    void physUpdate()
    {
        rewinder.physUpdate();
        if (grenadeIsRewinding)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }

}
