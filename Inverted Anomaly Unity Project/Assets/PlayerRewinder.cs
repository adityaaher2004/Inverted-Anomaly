using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRewinder : MonoBehaviour
{
    public Transform[] bones;
    Stack<PlayerKeyFrame> keyframes;

    Stack<PointInTime> points;

    [SerializeField] GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        points = new Stack<PointInTime>();
        keyframes = new Stack<PlayerKeyFrame>();

        bones = gameObject.GetComponentsInChildren<Transform>();
    }


    // Update is called once per frame
    void Update()
    {
        if (gameManager.globalIsRewinding)
        {
            StartRewind();
        }
        if (gameManager.globalIsRewinding)
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
    }

    void StopRewind()
    {
        gameObject.GetComponent<Animator>().enabled = true;
    }

    void FixedUpdate()
    {
        physUpdate();
    }

    void physUpdate()
    {
        if (gameManager.globalIsRewinding)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }
}
