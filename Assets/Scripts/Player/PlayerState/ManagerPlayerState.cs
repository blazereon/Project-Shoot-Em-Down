using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ManagerPlayerState : Player
{
    private BasePlayerState _currentState;
    private Stack<BasePlayerState> _stateStack = new Stack<BasePlayerState>();

    private BasePlayerCombatState _currentCombatState; //should I use pushdown automata?
    private Stack<BasePlayerCombatState> _combatStateStack = new Stack<BasePlayerCombatState>();



    public Collider2D PlayerCollider;

    //Set the Sprite Animator
    public Animator BodyAnimator;
    public Animator ArmAnimator;
    //Set the Arm Offset
    public Transform ArmTransform;
    public Vector2 ArmOffsetLeft = new Vector2(0.2f, -0.6f);
    public Vector2 ArmOffsetRight = new Vector2(-0.2f, -0.6f);
    //Set the Gun Arm Offset
    public Transform GunArmTransform;
    public Vector2 GunArmOffsetLeft = new Vector2(0.2f, -0.6f);
    public Vector2 GunArmOffsetRight = new Vector2(-0.2f, -0.6f);
    //Set the Projectile Spawn point
    public Transform projectileSpawnPoint;

    public Facing facing = Facing.left;

    public bool isDashCooldown = false;

    void Awake()
    {

        //Set Component abilities to player
        KeenAbility = new Keen(this);
        DestructiveBoltAbility = new DestructiveBolt(this);

        //Some functions to be subscribed on events system
        EventSystem.Current.OnAttackPlayer += TakePlayerDamage;
        EventSystem.Current.OnSendPlayerOrb += ReceiveOrb;
        EventSystem.Current.OnEnemyKill += OnKillResponse;
        EventSystem.Current.OnApplyEffect += ReceiveEffect;
        EventSystem.Current.OnPlayerUpgradeAbility += ApplyUpgradeAbility;
        CheatEventSystem.Current.OnPlayerModifyStat += ApplyCheat;
        CheatEventSystem.Current.OnPlayerUpgradeAbility += ApplyUpgradeAbility;
        EventSystem.Current.OnRefreshPlayerUI += UpdateUIData;
        EventSystem.Current.OnSavePlayerStat += SavePlayerStats;


        //Component Abilities
        OnTriggerEmpowerment += DashAbility.SetEmpowered;
        OnTriggerEmpowerment += KeenAbility.SetEmpowered;

        //Initialize PlayerCurrentStats based on base stats
        PlayerCurrentStats = new PlayerStats
        {
            Health = PlayerBaseStats.Health,
            MaxHealth = PlayerBaseStats.MaxHealth,
            Violence = PlayerBaseStats.Violence,
            MaxMomentum = PlayerBaseStats.MaxMomentum,
            MaxPneumatic = PlayerBaseStats.MaxPneumatic,
            ProjectileSpeed = PlayerBaseStats.ProjectileSpeed,
            MaxAggression = PlayerBaseStats.MaxAggression,
            MaxViolence = PlayerBaseStats.MaxViolence,
            MaxChain = PlayerBaseStats.MaxChain,
            Xp = PlayerBaseStats.Xp,
            MaxXp = PlayerBaseStats.MaxXp,
            SkillPoint = PlayerBaseStats.SkillPoint,
            ChainDuration = PlayerBaseStats.ChainDuration,
            AttackRate = PlayerBaseStats.AttackRate

        };

    }
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        dashAction = InputSystem.actions.FindAction("Dash");
        attackAction = InputSystem.actions.FindAction("Attack");
        switchWeaponAction = InputSystem.actions.FindAction("SwitchWeapon");
        empowerAbilityAction = InputSystem.actions.FindAction("Empower");
        keenAbilityAction = InputSystem.actions.FindAction("KeenAbility");
        destructiveAbilityAction = InputSystem.actions.FindAction("DestructiveBolt");

        PlayerCollider = GetComponent<Collider2D>();

        // Main Body Animator
        BodyAnimator = GetComponent<Animator>(); 
        
        // Child Arm Animator
        ArmAnimator = transform.Find("Arm_Swing").GetComponent<Animator>(); 
        ArmTransform = ArmAnimator.transform;

        EventSystem.Current.UpdatePlayerStats(PlayerCurrentStats);
        StartCoroutine(MomentumDecay());
        _currentState = IdleState;
        _currentState.EnterState(this);

        _currentCombatState = IdleCombatState;
        _currentCombatState.EnterState(this);

    }


    void Update()
    {
        //for controlling player face
        MainUpdate();

        //Actual state update
        _currentState.UpdateState(this);
        _currentCombatState.UpdateState(this);
    }

    void FixedUpdate()
    {
        _currentState.FixedUpdateState(this);
        _currentCombatState.FixedUpdateState(this);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        _currentState.OnCollisionEnter2DState(collision, this);
        _currentCombatState.OnCollisionEnter2DState(collision, this);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        _currentState.OnCollisionExit2DState(collision, this);
        _currentCombatState.OnCollisionExit2DState(collision, this);
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

    public void SwitchCombatState(BasePlayerCombatState state)
    {
        _currentCombatState = state;
        _currentCombatState.EnterState(this);
    }

    public void PushCurrentCombatState()
    {
        _combatStateStack.Push(_currentCombatState);
    }

    public void PopCombatState()
    {
        _currentCombatState = _combatStateStack.Pop();
        _currentCombatState.EnterState(this);
    }

    void MainUpdate()
    {
        Vector2 _moveValue = moveAction.ReadValue<Vector2>();
        if (_moveValue.x < 0)
        {
            facing = Facing.left;
        }
        else if (_moveValue.x > 0)
        {
            facing = Facing.right;
        }

        //Invoking empowerment upon full aggression
        if (empowerAbilityAction.triggered && !IsEmpowerementInvoke)
        {
            IsEmpowerementInvoke = true;
        }

        //Invoking keen ability (test)
        if (keenAbilityAction.triggered)
        {
            Debug.Log("Keen Ability Triggered");
            KeenAbility.ConsumeKeen();
            if (KeenAbility.IsKeenCooldown) TriggerEmpowerment(KeenAbility);
        }

        //Invoking Destructive Bolt
        if (destructiveAbilityAction.triggered)
        {
            Debug.Log("Destructive Bolt Triggered");
            if (!DestructiveBoltAbility.IsCooldown) TriggerEmpowerment(DestructiveBoltAbility);
        }

        //facing sprite logic
        switch (facing)
        {
            case Facing.right:
                PlayerSprite.flipX = false;
                if (ArmTransform != null)
                {
                    ArmTransform.localPosition = ArmOffsetRight;
                    ArmTransform.localScale = new Vector3(1, 1, 1); // unflip
                }

                if (GunArmTransform != null)
                {
                    GunArmTransform.localPosition = GunArmOffsetRight;
                }

                break;

            case Facing.left:
                PlayerSprite.flipX = true;
                if (ArmTransform != null)
                {
                    ArmTransform.localPosition = ArmOffsetLeft;
                    ArmTransform.localScale = new Vector3(-1, 1, 1); // flip
                }

                if (GunArmTransform != null)
                {
                    GunArmTransform.localPosition = GunArmOffsetLeft;
                }

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
            playerStats = PlayerCurrentStats,
            EffectsList = CurrentEffect,
        };
        EventSystem.Current.UpdatePlayerDebug(_debugData);
    }


    IEnumerator MomentumDecay()
    {
        while (true)
        {
            yield return new WaitForSeconds(MomentumDecayRate / 8);
            if (_currentState == IdleState)
            {
                PlayerCurrentStats.Momentum = Mathf.Clamp(PlayerCurrentStats.Momentum - (20 / 8), 0, PlayerCurrentStats.MaxMomentum);
                EventSystem.Current.UpdatePlayerStats(PlayerCurrentStats);
                continue;
            }

            if (_currentState == RunState)
            {
                if (PlayerCurrentStats.Momentum > PlayerCurrentStats.MaxMomentum * 0.75)
                {
                    PlayerCurrentStats.Momentum = (int)Mathf.Clamp(PlayerCurrentStats.Momentum - (10 / 8), PlayerCurrentStats.MaxMomentum * 0.75f, PlayerCurrentStats.MaxMomentum);
                    EventSystem.Current.UpdatePlayerStats(PlayerCurrentStats);
                    continue;
                }
                PlayerCurrentStats.Momentum = (int)Mathf.Clamp(PlayerCurrentStats.Momentum + (10 / 8), 0, PlayerCurrentStats.MaxMomentum * 0.75f);
                EventSystem.Current.UpdatePlayerStats(PlayerCurrentStats);
                continue;
            }

        }
    }

    void OnDestroy()
    {
        EventSystem.Current.OnAttackPlayer -= TakePlayerDamage;
        EventSystem.Current.OnSendPlayerOrb -= ReceiveOrb;

        CheatEventSystem.Current.OnPlayerModifyStat -= ApplyCheat;
        CheatEventSystem.Current.OnPlayerUpgradeAbility -= ApplyUpgradeAbility;
    }

    private void OnDrawGizmos()
    {
        if (facing == Facing.right)
        {
            Gizmos.DrawWireSphere(new Vector2(transform.position.x + MeleePadding, transform.position.y), MeleeRadius);
        }
        else if (facing == Facing.left)
        {
            Gizmos.DrawWireSphere(new Vector2(transform.position.x - MeleePadding, transform.position.y), MeleeRadius);
        }
    }

    private string currentBodyAnimation = "";
    public string currentArmAnimation = "";
    private string currentGunAnimation = "";

    public void PlayBodyAnimation(string newAnimation)
    {
        if (newAnimation == currentBodyAnimation) return;
        BodyAnimator.Play(newAnimation);
        currentBodyAnimation = newAnimation;
    }

    public void PlayArmAnimation(string newAnimation)
    {
        if (newAnimation == currentArmAnimation) return;
        ArmAnimator.Play(newAnimation);
        currentArmAnimation = newAnimation;
    }

}