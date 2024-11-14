using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerRewinder : MonoBehaviour
{
    bool playerIsRewinding;
    public Transform[] bones;
    Stack<PlayerKeyFrame> keyframes;
    Stack<PointInTime> points;

    GlobalIsRewindingScript globalRewinder;

    private void Awake()
    {
        playerIsRewinding = false;
        points = new Stack<PointInTime>();
        keyframes = new Stack<PlayerKeyFrame>();

        GameObject[] bonesTemp = GameObject.FindGameObjectsWithTag("ArmatureBone");

        foreach (GameObject b in bonesTemp)
        {
            bones.Append(b.transform);
        }

        globalRewinder = GameObject.FindFirstObjectByType<GlobalIsRewindingScript>();
    }


    // Update is called once per frame
    void Update()
    {
        if (globalRewinder.fireStartRewind)
        {
            StartRewind();
        }
        if (globalRewinder.fireStopRewind)
        {
            StopRewind();
        }
    }

    void Record()
    {
        points.Push(new PointInTime(gameObject.transform.position, gameObject.transform.rotation));

        PlayerKeyFrame frameData = new PlayerKeyFrame(bones.Length);

        for (int i = 0; i < bones.Length; i++)
        {
            frameData.bonePositions[i] = bones[i].localPosition;
            frameData.boneRotations[i] = bones[i].localRotation;
        }

        keyframes.Push(frameData);
    }

    void Rewind()
    {
        if (points.Count > 0)
        {
            PointInTime latest = points.Pop();
            gameObject.transform.position = latest.position;
            gameObject.transform.rotation = latest.rotation;

            PlayerKeyFrame frameData = keyframes.Pop();

            for (int i = 0; i < bones.Length; i++)
            {
                bones[i].localPosition = frameData.bonePositions[i];
                bones[i].localRotation = frameData.boneRotations[i];
            }

        }
        else
        {
            StopRewind();
        }
    }

    void StartRewind()
    {
        gameObject.GetComponent<Animator>().enabled = false;
        playerIsRewinding = true;
    }

    void StopRewind()
    {
        gameObject.GetComponent<Animator>().enabled = true;
        playerIsRewinding = false;
    }

    void FixedUpdate()
    {
        physUpdate();
    }

    void physUpdate()
    {
        if (playerIsRewinding)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }
}
