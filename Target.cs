using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public bool isTargetable { get; private set; }

    public Transform GetTargetTransform()
    {
        return this.transform;
    }
}
