using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
     * To implement rewinding:
     * 
     * void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                rewinder.StartRewind();
            }
            if (Input.GetKeyUp(KeyCode.R))
            {
                rewinder.StopRewind();
            }
        }

      void FixedUpdate()
        {
            rewinder.physUpdate();
        }

     * 
     * This class does not implement specific rewinding such as re-calculating timer for grenades
     * You need to implement variable specific rewinding in the required class
     * 
     * Tip: To implement rewinding for specific variables:
     * - Create bool isRewinding and 5 new methods for your object: StartRewind, StopRewind, Rewind, Record, physUpdate
     * - Each one must mimic the implementations as below
     * Refer to Grendade class for an implementation
     */

    List<PointInTime> points = new List<PointInTime>();

    Transform transform;
    GameObject gameObject;


    /*
     * For entities Instantiated during gameplay such as bullets 
     * Once the rewind stack is empty, destroy the gameObject if destructMode is true
     */
    bool destructMode = false;

    public Rewindable(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.transform = gameObject.transform;
    }

    public Rewindable(GameObject gameObject, bool destructable)
    {
        this.gameObject = gameObject;
        this.transform = gameObject.transform;
        this.destructMode = destructable;
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
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void StopRewind()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        if (destructMode && points.Count <= 0)
        {
            UnityEngine.Object.Destroy(gameObject);
        }
    }

    public void physUpdate(bool isRewinding)
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