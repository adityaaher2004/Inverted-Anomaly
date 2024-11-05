using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [SerializeField] TextMeshProUGUI globalGameTimeText;
    float globalGameTime;

    public bool globalIsRewinding;

    Stack<float> globalGameTimePoints;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        globalIsRewinding = false;
        globalGameTime = 0;
        globalGameTimePoints = new Stack<float>();
    }
    void Start()
    {
        globalGameTimeText.text = "" + globalGameTime;
    }

    void Update()
    {
        float microSeconds = Mathf.Round((globalGameTime % 1) * 100);
        int seconds = (int) globalGameTime / 1;
        if (seconds < 10)
        {
            globalGameTimeText.text = "0" + seconds + ":" + microSeconds;
        }
        else
        {
            globalGameTimeText.text = "" + seconds + ":" + microSeconds;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartRewind();
        }
        if (Input.GetKeyUp(KeyCode.T))
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

    void Rewind()
    {
        if (globalGameTimePoints.Count > 0)
        {
            float latestTime = globalGameTimePoints.Pop();
            globalGameTime = latestTime;
        }
        else
        {
            StopRewind();
        }
    }

    void Record()
    {
        globalGameTimePoints.Push(globalGameTime);
        globalGameTime += Time.deltaTime;
    }

    void StartRewind()
    {
        globalIsRewinding = true;
    }

    void StopRewind()
    {
        globalIsRewinding = false;
    }
   

    void physUpdate()
    {
        if (globalIsRewinding)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }
}
