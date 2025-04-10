using System;
using UnityEngine;

[Serializable]
public class PlayerGroundedData
{
    [Range(0f, 25f)] public float baseSpeed = 5f;
    [field: SerializeField] public AnimationCurve SlopeSpeedAngles { get; private set; }
    public PlayerRotationData playerRotationData;
    public PlayerWalkData playerWalkData;
    public PlayerRunData playerRunData;
    public PlayerDashData playerDashData;
}