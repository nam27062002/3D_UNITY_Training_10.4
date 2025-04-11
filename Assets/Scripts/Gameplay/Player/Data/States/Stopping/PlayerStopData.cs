using System;
using UnityEngine;

[Serializable]
public class PlayerStopData
{
    [field: SerializeField, Range(0f, 15f)]
    public float LightDecelerationForce { get; set; } = 5f;

    [field: SerializeField, Range(0f, 15f)]
    public float MediumDecelerationForce { get; set; } = 6.5f;

    [field: SerializeField, Range(0f, 15f)]
    public float HardDecelerationForce { get; set; } = 5f;
}