using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GlobalIsRewindingScript : MonoBehaviour
{

    public bool fireStartRewind;
    public bool fireStopRewind;

    public bool rewindAlreadyFired = false;

    float totalGameTime = 60f;

    [SerializeField] Transform EndB;
    [SerializeField] Transform EndD;
    [SerializeField] Transform Laval;
    [SerializeField] TextMeshProUGUI gameOver;

    bool reachedB;
    bool reachedD;
    bool touchedLava;

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
        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}
