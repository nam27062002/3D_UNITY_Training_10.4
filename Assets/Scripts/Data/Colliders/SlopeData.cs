using System;
using UnityEngine;

[Serializable]
public class SlopeData
{
    [field: SerializeField, Range(0f, 1f)] public float StepHeightPercentage { get; private set; } = 0.25f;
}