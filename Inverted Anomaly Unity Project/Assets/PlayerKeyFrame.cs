using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerKeyFrame
{
    public Vector3[] bonePositions;
    public Quaternion[] boneRotations;

    public PlayerKeyFrame(int boneCount)
    {
        bonePositions = new Vector3[boneCount];
        boneRotations = new Quaternion[boneCount];
    }
}

