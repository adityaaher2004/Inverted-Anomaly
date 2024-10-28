using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Rewindable
{

    /*
     * Class to implement basic position based rewind mechanics
     * 
     * This class contains basic rewind mechanics by:
     * - Storing the position, rotation, visibility of objects in a list
     * - The Record() method accumalates these variables in a list
     * - The Rewind variable unwinds this list of variables
     * - The physUpdate() implements the rewind control mechanism 
     * 
     * This class does not implement specific rewinding such as re-calculating timer for grenades
     * You need to implement variable specific rewinding in the required class
     * 
     * Tip: To implement rewinding for specific variables:
     * - Create bool isRewinding and 5 new methods : StartRewind, StopRewind, Rewind, Record, physUpdate
     * - Each one must mimic the implementations as below
     * Refer to Grendade class for an implementation
     */

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
        points.Insert(0, new PointInTime(gameObject.transform.position, gameObject.transform.rotation));
    }

    public void Rewind()
    {
        if (points.Count > 0)
        {
            PointInTime latest = points[0];
            gameObject.transform.position = latest.position;
            gameObject.transform.rotation = latest.rotation;
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