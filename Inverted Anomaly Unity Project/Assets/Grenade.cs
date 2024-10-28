using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    
    // Start is called before the first frame update

    float cookTime = 2f;
    float timer;
    float blastRadius = 25f;
    float blastForce = 5000f;

    bool hasExploded = false;

    void Start()
    {
        timer = cookTime;
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

        Destroy(gameObject);
    }
}
