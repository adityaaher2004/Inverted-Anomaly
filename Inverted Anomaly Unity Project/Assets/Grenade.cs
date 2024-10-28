using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    // Grenade Specific Variables
    // -------------------------
    float cookTime = 2f;
    float timer;
    float blastRadius = 25f;
    float blastForce = 5000f;
    bool hasExploded = false;
    // -------------------------


    // Rewinder Variables
    // ------------------
    Rigidbody rb;
    Rewindable rewinder;
    List<bool> isActiveRewinderFrames;
    List<float> grenadeTimerFrames;
    bool grenadeIsRewinding = false;
    // ------------------

    void Start()
    {
        timer = cookTime;
        isActiveRewinderFrames = new List<bool>();
        grenadeTimerFrames = new List<float>();
        rewinder = new Rewindable(gameObject);
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

        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    void Record()
    {
        isActiveRewinderFrames.Insert(0, gameObject.GetComponent<MeshRenderer>().enabled == true);
        grenadeTimerFrames.Insert(0, timer);
        
    }

    void Rewind()
    {
        if (isActiveRewinderFrames.Count > 0)
        {
            bool latestActiveStatus = isActiveRewinderFrames[0];
            gameObject.GetComponent<MeshRenderer>().enabled = latestActiveStatus;
            hasExploded = !latestActiveStatus;
            timer = grenadeTimerFrames[0];

            grenadeTimerFrames.RemoveAt(0);
            isActiveRewinderFrames.RemoveAt(0);
        }
        else
        {
            StopRewind();
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
