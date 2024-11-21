using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BreakEnemy : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject grenadePrefab;

    public float health = 100f;
    public float damageAmt = 10f;

    List<Transform> bones;
    List<Transform> boneParents;

    bool isRewinding = false;

    Stack<float> health_stack;

    GlobalIsRewindingScript globalRewinder;

    private void Awake()
    {
        Transform[] bonesTemp = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform item in bonesTemp)
        {
            if (item.CompareTag("ArmatureBone"))
            {
                bones.Add(item.transform);
                boneParents.Add(item.parent);
            }
        }
        health_stack = new Stack<float>();
        globalRewinder = GameObject.FindFirstObjectByType<GlobalIsRewindingScript>();
    }

    private void Update()
    {
        if (globalRewinder.fireStartRewind)
        {
            StartRewind();
        }
        if (globalRewinder.fireStopRewind)
        {
            StopRewind();
        }
    }

    void FixedUpdate()
    {
        // Need this line to apply rewinding
        // --------------------------------
        physUpdate();
        // --------------------------------
    }

    void StartRewind()
    {
        isRewinding = true;
    }

    void StopRewind()
    {
        isRewinding = false;
    }

    void Record()
    {
        health_stack.Push(health);
    }

    void Rewind()
    {
        if (health_stack.Count > 0)
        {
            health = health_stack.Pop();
        }
        else
        {
            StopRewind();
        }
    }

    public void physUpdate()
    {
        if (isRewinding)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Bullet")
        {
            TakeDamage(damageAmt);

            if (health <= 0)
            {
                DetachParents();

                Instantiate(grenadePrefab);
            }
        }

    }

    void TakeDamage(float amt)
    {
        gameObject.GetComponentInChildren<HealthBar>().TakeDamage(amt);
        health -= amt;
    }

    void DetachParents()
    {
        foreach (Transform b in bones)
        {
            if (b.CompareTag("ArmatureBone"))
            {
                b.parent = null;
                b.AddComponent<Rigidbody>();
            }
        }
    }

    void AttachParents()
    {

        foreach (Transform b in bones)
        {
            if (b.CompareTag("ArmatureBone"))
            {
                b.parent = boneParents[bones.IndexOf(b)];
                b.AddComponent<Rigidbody>();
            }
        }
    }
}
