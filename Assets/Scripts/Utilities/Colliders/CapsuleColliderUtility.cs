using System;
using UnityEngine;

[Serializable]
public class CapsuleColliderUtility
{
    public CapsuleColliderData CapsuleCollider { get; private set; }
    [field: SerializeField] public DefaultColliderData DefaultCollider { get; private set; }
    [field: SerializeField] public SlopeData Slope { get; private set; }

    public void Initialize(GameObject gameObject)
    {
        if (CapsuleCollider != null) return;
        CapsuleCollider = new CapsuleColliderData();
        CapsuleCollider.Initialize(gameObject);
    }
    
    public void CalculateCapsuleColliderDimensions()
    {
        SetCapsuleColliderRadius(DefaultCollider.Radius);
        SetCapsuleColliderHeight(DefaultCollider.Height * (1f - Slope.StepHeightPercentage));
        RecalculateCapsuleColliderCenter();
        var halfColliderHeight = DefaultCollider.Height / 2f;
        if (halfColliderHeight < CapsuleCollider.Collider.radius)
        {
            SetCapsuleColliderRadius(halfColliderHeight);
        }
        CapsuleCollider.UpdateColliderData();
    }

    private void SetCapsuleColliderRadius(float radius)
    {
        CapsuleCollider.Collider.radius = radius;
    }
    
    private void SetCapsuleColliderHeight(float height)
    {
        CapsuleCollider.Collider.height = height;
    }

    public void RecalculateCapsuleColliderCenter()
    {
        float colliderHeightDifference = DefaultCollider.Height - CapsuleCollider.Collider.height;
        var newColliderCenter = new Vector3(0f, DefaultCollider.CenterY + (colliderHeightDifference / 2f), 0f);
        CapsuleCollider.Collider.center = newColliderCenter;
    }
}