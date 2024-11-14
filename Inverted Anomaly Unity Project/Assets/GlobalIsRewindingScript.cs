using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalIsRewindingScript : MonoBehaviour
{

    public bool fireStartRewind;
    public bool fireStopRewind;

    public bool rewindAlreadyFired = false;

    float totalGameTime = 15f;

    private void Update()
    {
        totalGameTime -= Time.deltaTime;

        fireStartRewind = false;
        fireStopRewind = false;

        if (totalGameTime <= 0 && !rewindAlreadyFired)
        {
            Debug.Log("Firing Global Start Rewind");
            fireStartRewind = true;
            rewindAlreadyFired = true;
        }
        if (Input.GetKeyUp(KeyCode.T))
        {
            Debug.Log("Firing Global Stop Rewind");
            fireStopRewind = true;
        }
    }
}
