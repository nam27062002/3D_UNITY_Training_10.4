using System;
using UnityEngine;

[Serializable]
public class PlayerRunData
{
    [Range(1f, 2f)] public float speedModifier = 1f;
}