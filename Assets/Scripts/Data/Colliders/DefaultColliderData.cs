using System;
using UnityEngine;

[Serializable]
public class DefaultColliderData
{
    [field: SerializeField] public float Height { get; set; } = 1.8f;
    [field: SerializeField] public float CenterY { get; set; } = 0.9f;
    [field: SerializeField] public float Radius { get; set; } = 0.2f;
}