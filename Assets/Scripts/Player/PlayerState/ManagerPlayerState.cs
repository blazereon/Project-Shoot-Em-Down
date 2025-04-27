using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public LandPlayerState LandState = new LandPlayerState();
    public AttackPlayerState AttackState = new AttackPlayerState();
    public DashPlayerState DashState = new DashPlayerState();
    public WallGrabPlayerState WallGrabState = new WallGrabPlayerState();
    public WallJumpPlayerState WallJumpState = new WallJumpPlayerState();

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
        DebugUI.Current.updatePlayerState(_currentState); //comment this if not using DebugUI
        if (_moveValue.x < 0) {
            facing = Facing.left;
        } else if (_moveValue.x > 0) {
            facing = Facing.right;
        }
        EventSystem.Current.PlayerLocation = transform.position;
        _currentState.UpdateState(this);
    }

    void FixedUpdate()
    {
        _currentState.FixedUpdateState(this);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        _currentState.OnCollisionEnter2DState(collision, this);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        _currentState.OnCollisionExit2DState(collision, this);
    }

    //pop state from the stack and use it as a current state
    public void PopState()
    {
        _currentState = _stateStack.Pop();
        _currentState.EnterState(this);
    }

    //push current state to the stack and switch to the new state
    public void PushCurrentState()
    {
        _stateStack.Push(_currentState);
    }

    //switch the current state (no prev state will be pushed)
    public void SwitchState(BasePlayerState state)
    {
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