using System;
using UnityEngine;

[Serializable]
public class PlayerGroundedData
{
    [Range(0f, 25f)] public float baseSpeed = 5f;
    public PlayerRotationData playerRotationData;
    public PlayerWalkData playerWalkData;
    public PlayerRunData playerRunData;
}