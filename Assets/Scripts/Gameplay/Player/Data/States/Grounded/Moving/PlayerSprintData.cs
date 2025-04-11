using System;
using UnityEngine;

[Serializable]
public class PlayerSprintData
{
    [field: SerializeField, Range(1f, 3f)] public float SpeedModifier { get; set; } = 1.7f;
    [field: SerializeField, Range(0f, 5f)] public float SprintToRunTime { get; set; } = 1f;
    [field: SerializeField, Range(0f, 2f)] public float RunToWalkTime { get; set; } = 0.5f;
}