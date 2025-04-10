using UnityEngine;

public abstract class PlayerGroundState : PlayerState
{
    private readonly SlopeData _slopeData;
    
    protected PlayerGroundState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player)
    {
        _slopeData = player.ColliderUtility.Slope;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Float();
    }

    private void Float()
    {
        Vector3 capsuleColliderCenterInWorldSpace = Player.ColliderUtility.CapsuleCollider.Collider.bounds.center;
        Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);
        if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, _slopeData.FloatRayDistance, Player.LayerData.GroundLayer))
        {
            float distanceToFloatingPoint = Player.ColliderUtility.CapsuleCollider.ColliderCenterInLocalSpace.y * Player.transform.localScale.y - hit.distance;
            Debug.Log(distanceToFloatingPoint);
            if (distanceToFloatingPoint == 0f) return;
            float amountToLift = distanceToFloatingPoint * _slopeData.StepReachForce - GetPlayerVerticalVelocity().y;
            Vector3 liftForce = new Vector3(0f, amountToLift, 0f);
            Player.Rigidbody.AddForce(liftForce, ForceMode.VelocityChange);
        }
    }
}