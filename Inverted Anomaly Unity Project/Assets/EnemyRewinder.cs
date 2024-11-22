using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyRewinder : MonoBehaviour
{
    bool enemyIsRewinding;
    public List<Transform> bones;
    public List<Transform> boneParents;
    Stack<PlayerKeyFrame> keyframes;
    Stack<PointInTime> points;
    Stack<bool> parentsAttached;

    GlobalIsRewindingScript globalRewinder;

    private void Awake()
    {
        enemyIsRewinding = false;
        points = new Stack<PointInTime>();
        keyframes = new Stack<PlayerKeyFrame>();

        Transform[] bonesTemp = gameObject.GetComponentsInChildren<Transform>();

        Debug.Log($"Total transforms = {bonesTemp.Length}");

        foreach (Transform item in bonesTemp)
        {
            bones.Add(item.transform);
            boneParents.Add(item.parent);
        }

        Debug.Log($"Total Bones = {bones.Count}");

        Debug.Log($"{bones[0].position}, {bones[0].localPosition}");


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

        PlayerKeyFrame frameData = new PlayerKeyFrame(bones.Count);

        for (int i = 0; i < bones.Count; i++)
        {
            frameData.bonePositions[i] = bones[i].position;
            frameData.boneRotations[i] = bones[i].rotation;
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

            for (int i = 0; i < bones.Count; i++)
            {
                bones[i].position = frameData.bonePositions[i];
                bones[i].rotation = frameData.boneRotations[i];
            }

        }
        else
        {
            StopRewind();
        }
    }

    void StartRewind()
    {
        //for (int i = 0; i < bones.Count; i++)
        //{
        //    bones[i].parent = null;
        //}
        gameObject.GetComponent<Animator>().enabled = false;
        enemyIsRewinding = true;
    }

    void StopRewind()
    {
        enemyIsRewinding = false;
        gameObject.GetComponent<Animator>().enabled = true;
        //for (int i = 0; i < bones.Count; i++)
        //{
        //    bones[i].parent = boneParents[i];
        //}
    }

    void FixedUpdate()
    {
        physUpdate();
    }

    void physUpdate()
    {
        if (enemyIsRewinding)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }
}
