using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class SlopeData
{
    [field: SerializeField, Range(0f, 1f)] public float StepHeightPercentage { get; private set; } = 0.25f;
    [field: SerializeField, Range(0f, 10f)] public float FloatRayDistance { get; private set; } = 2f;
    [field: SerializeField, Range(0f, 50f)] public float StepReachForce { get; private set; } = 25f;
    [Button("Reset Step Height")]
    public void ResetStepHeight()
    {
        StepHeightPercentage = 0.25f;
        Object.FindAnyObjectByType<Player>().OnValidate();
    }
}