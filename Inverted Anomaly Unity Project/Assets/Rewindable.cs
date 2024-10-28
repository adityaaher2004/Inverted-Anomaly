using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class IRewindable : MonoBehaviour
{
    public abstract void Rewind();

    public abstract void Record();
}