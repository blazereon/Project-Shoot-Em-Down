using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    public PlungePlayerState PlungeState = new PlungePlayerState();

    public Collider2D PlayerCollider;

    public Facing facing = Facing.left;

    public bool isDashCooldown = false;

    void Awake()
    {
        EventSystem.Current.OnAttackPlayer += TakePlayerDamage;
        EventSystem.Current.OnSendPlayerPneuma += ReceivePneuma;
        EventSystem.Current.OnEnemyKill += OnKillResponse;

        //Component Abilities
        OnTriggerEmpowerment += DashAbility.SetEmpowered;
        OnTriggerEmpowerment += KeenAbility.SetEmpowered;
    }
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        dashAction = InputSystem.actions.FindAction("Dash");
        attackAction = InputSystem.actions.FindAction("Attack");
        switchWeaponAction = InputSystem.actions.FindAction("SwitchWeapon");
        empowerAbility = InputSystem.actions.FindAction("Empower");
        keenAbility = InputSystem.actions.FindAction("KeenAbility");

        PlayerCollider = GetComponent<Collider2D>();

        EventSystem.Current.UpdatePlayerStats(PlayerCurrentStats);
        StartCoroutine(MomentumDecay());
        _currentState = IdleState;
        _currentState.EnterState(this);

        //for debugging purposes: dash upgrade
        DashAbility.UpgradeComponent();
        DashAbility.UpgradeComponent();
        DashAbility.UpgradeComponent();

        //for debugging purposes: keen upgrade
        KeenAbility.UpgradeComponent();
        KeenAbility.UpgradeComponent();
        KeenAbility.UpgradeComponent();
    }


    void Update()
    {
        //for controlling player face
        Vector2 _moveValue = moveAction.ReadValue<Vector2>();
        if (_moveValue.x < 0) {
            facing = Facing.left;
        } else if (_moveValue.x > 0) {
            facing = Facing.right;
        }

        //Invoking empowerment upon full aggression
        if (empowerAbility.triggered && !IsEmpowerementInvoke)
        {
            IsEmpowerementInvoke = true;
        }

        //Invoking keen ability (test)
        if (keenAbility.triggered)
        {
            Debug.Log("Keen Ability Triggered");
            TriggerEmpowerment(KeenAbility);
        }

        //facing sprite logic
            switch (facing)
            {
                case Facing.right:
                    PlayerSprite.flipX = false;
                    break;
                case Facing.left:
                    PlayerSprite.flipX = true;
                    break;
                default:
                    Debug.LogError("Invalid facing value");
                    break;
            }

        //for toggling weapon (ranged, melee)
        if (switchWeaponAction.triggered)
        {
            if (PlayerCurrentStats.CurrentAttackType == AttackType.Melee)
            {
                PlayerCurrentStats.CurrentAttackType = AttackType.Ranged;
            }
            else
            {
                PlayerCurrentStats.CurrentAttackType = AttackType.Melee;
            }
        }

        //send player position and collider information to event script to be used by other game objects
        EventSystem.Current.PlayerLocation = transform.position;
        EventSystem.Current.PlayerCollider = PlayerCollider;

        //Helps devs debug states. 
        PlayerDebug _debugData = new PlayerDebug
        {
            playerState = _currentState,
            playerStats = PlayerCurrentStats
        };
        EventSystem.Current.UpdatePlayerDebug(_debugData);

        //Actual state update
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
        EventSystem.Current.OnSendPlayerPneuma -= ReceivePneuma;
    }

    private void OnDrawGizmos()
    {
        if (facing == Facing.right)
        {
            Gizmos.DrawWireSphere(new Vector2(transform.position.x + MeleePadding, transform.position.y), MeleeRadius);
        } else if (facing == Facing.left)
        {
            Gizmos.DrawWireSphere(new Vector2(transform.position.x - MeleePadding, transform.position.y), MeleeRadius);
        }
    }

}