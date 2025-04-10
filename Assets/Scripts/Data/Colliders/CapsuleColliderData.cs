using UnityEngine;

public class CapsuleColliderData
{
    public CapsuleCollider Collider;
    public Vector3 ColliderCenterInLocalSpace;

    public void Initialize(GameObject gameObject)
    {
        if (Collider == null)
        {
            Collider = gameObject.GetComponent<CapsuleCollider>();
            UpdateColliderData();
        }
    }

    public void UpdateColliderData()
    {
        ColliderCenterInLocalSpace = Collider.center;
    }
}