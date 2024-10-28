using UnityEngine;

public class PointInTime 
{

    public Vector3 position;
    public Quaternion rotation;
    public bool isActive;

    public PointInTime(Vector3 position, Quaternion rotation, bool isActive)
    {
        this.position = position;
        this.rotation = rotation;
        this.isActive = isActive;
    }
}
