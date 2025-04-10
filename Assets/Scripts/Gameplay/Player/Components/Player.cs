using UnityEngine;

[DefaultExecutionOrder(-18999), RequireComponent(typeof(PlayerInput)), RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [field: SerializeField] public PlayerSO PlayerData {get; private set;}
    public PlayerInput PlayerInput { get; set; }
    public Rigidbody Rigidbody { get; set; }
    public Transform MainCameraTransform { get; set; }
    
    private PlayerStateMachine _playerStateMachine;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
        Rigidbody = GetComponent<Rigidbody>();
        if (Camera.main != null) MainCameraTransform = Camera.main.transform;
    }

    private void Start()
    {
        _playerStateMachine = new PlayerStateMachine(this);
        _playerStateMachine.ChangeState(EPlayerStateType.Idle);
    }

    private void Update()
    {
        _playerStateMachine.HandleInput();
        _playerStateMachine.Update();
    }

    private void FixedUpdate()
    {
        _playerStateMachine.PhysicsUpdate();
    }
}