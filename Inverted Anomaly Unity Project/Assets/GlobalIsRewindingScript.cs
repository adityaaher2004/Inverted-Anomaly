using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalIsRewindingScript : MonoBehaviour
{
    public bool globalIsRewinding;

    private void Awake()
    {
        globalIsRewinding = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            globalIsRewinding = true;
        }
        if (Input.GetKeyUp(KeyCode.T))
        {
            globalIsRewinding = false;
        }
    }
}
