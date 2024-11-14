using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakEnemy : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject grenadePrefab;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Bullet")
        {
            gameObject.transform.DetachChildren();

            Instantiate(grenadePrefab);
        }

    }
}
