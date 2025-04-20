using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManagerPlayerState :  Player
{
    private BasePlayerState _currentState;
    private Stack<BasePlayerState> _stateStack = new Stack<BasePlayerState>();

    public IdlePlayerState IdleState = new IdlePlayerState();
    public WalkPlayerState WalkState = new WalkPlayerState();
    public RunPlayerState RunState = new RunPlayerState();
    public JumpPlayerState JumpState = new JumpPlayerState();
    public AttackPlayerState AttackState = new AttackPlayerState();
    public DashPlayerState DashState = new DashPlayerState();

    public Facing facing = Facing.left;
    public bool isDashCooldown = false;


    void Awake()
    {
        EventSystem.Current.OnAttackPlayer += TakePlayerDamage;
    }
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        dashAction = InputSystem.actions.FindAction("Dash");
        attackAction = InputSystem.actions.FindAction("Attack");
        EventSystem.Current.UpdatePlayerStats(PlayerCurrentStats);
        StartCoroutine(MomentumDecay());
        _currentState = IdleState;
        _currentState.EnterState(this); 
    }

    void Update()
    {
        Vector2 _moveValue = moveAction.ReadValue<Vector2>();

        if (_moveValue.x < 0) {
            facing = Facing.left;
        } else if (_moveValue.x > 0) {
            facing = Facing.right;
        }

        Debug.Log(_currentState);
        

        _currentState.UpdateState(this);
    }

    void FixedUpdate()
    {
        _currentState.FixedUpdateState(this);
        PlayerRb.linearVelocityY -= LandAcceleration * Time.fixedDeltaTime;
    }

    //pop current state from the stackfontaine battle theme ostfontaine battle theme ost
    public void SwitchState()
    {
        _currentState = _stateStack.Pop();
        _currentState.EnterState(this);
    }

    //push current state to the stack and switch to the new state
    public void SwitchState(BasePlayerState state)
    {
        _stateStack.Push(_currentState);
        _currentState = state;
        _currentState.EnterState(this);
    }


    IEnumerator MomentumDecay()
    {
        while(true)
        {
            yield return new WaitForSeconds(MomentumDecayRate/8);
            if (_currentState == IdleState)
            {
                PlayerCurrentStats.Momentum = Mathf.Clamp(PlayerCurrentStats.Momentum - (20/8), 0, PlayerCurrentStats.MaxMomentum);
                EventSystem.Current.UpdatePlayerStats(PlayerCurrentStats);
                continue;
            }

            if (_currentState == RunState)
            {
                if (PlayerCurrentStats.Momentum > PlayerCurrentStats.MaxMomentum * 0.75)
                {
                    PlayerCurrentStats.Momentum = (int)Mathf.Clamp(PlayerCurrentStats.Momentum - (10/8), PlayerCurrentStats.MaxMomentum * 0.75f, PlayerCurrentStats.MaxMomentum);
                    EventSystem.Current.UpdatePlayerStats(PlayerCurrentStats);
                    continue;
                }
                PlayerCurrentStats.Momentum = (int)Mathf.Clamp(PlayerCurrentStats.Momentum + (10/8), 0,  PlayerCurrentStats.MaxMomentum * 0.75f);
                EventSystem.Current.UpdatePlayerStats(PlayerCurrentStats);
                continue;
            }

        }
        

    }

    void OnDestroy()
    {
        EventSystem.Current.OnAttackPlayer -= TakePlayerDamage;
    }

    
}