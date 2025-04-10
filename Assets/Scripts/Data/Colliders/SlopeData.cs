using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class SlopeData
{
    [field: SerializeField, Range(0f, 1f)] public float StepHeightPercentage { get; private set; } = 0.25f;

    [Button("Reset Step Height")]
    public void ResetStepHeight()
    {
        StepHeightPercentage = 0.25f;
        Object.FindAnyObjectByType<Player>().OnValidate();
    }
}