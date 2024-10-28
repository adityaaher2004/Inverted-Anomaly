using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Rewindable
{
    public bool isRewinding = false;
    List<PointInTime> points = new List<PointInTime>();

    Rigidbody rb;

    Transform transform;
    GameObject gameObject;

    public Rewindable(GameObject gameObject)
    {
        this.gameObject = gameObject;
        rb = gameObject.GetComponent<Rigidbody>();
        this.transform = gameObject.transform;
    }


    public void Record()
    {
        points.Insert(0, new PointInTime(gameObject.transform.position, gameObject.transform.rotation, gameObject.activeSelf));
    }

    public void Rewind()
    {
        if (points.Count > 0)
        {
            PointInTime latest = points[0];
            gameObject.transform.position = latest.position;
            gameObject.transform.rotation = latest.rotation;
            gameObject.SetActive(latest.isActive);
            points.RemoveAt(0);

        }
        else
        {
            StopRewind();
        }
    }

    public void StartRewind()
    {
        isRewinding = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void StopRewind()
    {
        isRewinding = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
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


}