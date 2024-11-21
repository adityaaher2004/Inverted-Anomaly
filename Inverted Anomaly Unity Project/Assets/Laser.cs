using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Transform LaserEnd;

    LineRenderer lr;

    void Start()
    {
        lr = gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, gameObject.transform.position);
        lr.SetPosition(1, LaserEnd.position);
    }
}
