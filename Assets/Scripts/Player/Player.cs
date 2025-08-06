using System;
using System.Collections;
using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class Player : Entity
{
    public enum AttackType
    {
        Melee,
        Ranged
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject projectileObject;
    public Action<ComponentAbility> OnTriggerEmpowerment;

    //will be migrated in the future
    public InputAction moveAction;
    public InputAction jumpAction;
    public InputAction dashAction;
    public InputAction attackAction;
    public InputAction switchWeaponAction;
    public InputAction empowerAbilityAction;
    public InputAction keenAbilityAction;
    public InputAction destructiveAbilityAction;

    public IdlePlayerState IdleState = new IdlePlayerState();
    public WalkPlayerState WalkState = new WalkPlayerState();
    public RunPlayerState RunState = new RunPlayerState();
    public JumpPlayerState JumpState = new JumpPlayerState();
    public LandPlayerState LandState = new LandPlayerState();
    public DashPlayerState DashState = new DashPlayerState();
    public WallGrabPlayerState WallGrabState = new WallGrabPlayerState();
    public WallJumpPlayerState WallJumpState = new WallJumpPlayerState();
    public PlungePlayerState PlungeState = new PlungePlayerState();

    //Combat related states

    public IdlePlayerCombatState IdleCombatState = new IdlePlayerCombatState();
    public AttackPlayerCombatState AttackState = new AttackPlayerCombatState();

    public Dash DashAbility = new Dash();
    public Keen KeenAbility;
    public DestructiveBolt DestructiveBoltAbility;

    public Rigidbody2D PlayerRb;
    public SpriteRenderer PlayerSprite;
    public PlayerStats PlayerBaseStats;
    public PlayerStats PlayerCurrentStats;
    public float Speed = 500f;
    public float JumpForce = 10f;
    public float ShortJumpTime = 0.2f;
    public float LongJumpTime = 0.4f;
    public float LandAcceleration = 2f;
    public float LandStart = 3f;
    public float DashForce;
    public float DashDuration;
    public float DashRecovery;
    public float ImmuneDuration = 3f;

    public float MeleePadding;
    public float MeleeRadius;

    public bool CanAtack = true;

    public GameObject DashAttackBox;
    public GroundBox groundBox;
    public Coroutine AttackDelayInstance;

    [NonSerialized] public bool IsEmpowerementInvoke;

    public void TriggerEmpowerment(ComponentAbility ability)
    {
        if (!IsEmpowerementInvoke || PlayerCurrentStats.Aggression < PlayerCurrentStats.MaxAggression) return;
        PlayerCurrentStats.Aggression = 0;
        IsEmpowerementInvoke = false;
        OnTriggerEmpowerment?.Invoke(ability);

    }

    public void TakePlayerDamage(int damage)
    {
        if (!CanTakeDamage)
        {
            //you may put sfx for this
            return;
        }
        AudioManager.instance.RandomSFX(AudioManager.instance.playerTakeDmg);
        PlayerCurrentStats.Health -= damage;
        if (PlayerCurrentStats.Health <= 0)
        {
            UIManager.Current.CurrentState = UIState.GameOver;
            AudioManager.instance.PlayFX(AudioManager.instance.playerDeath, false);
            Destroy(this.gameObject);
        }
        StartCoroutine(PlayerImmune());
        Debug.Log("HP: " + PlayerCurrentStats.Health);
    }

    public void ReceiveOrb(OrbType type, int value)
    {
        switch (type)
        {
            case OrbType.Pneuma:
                int _PnGain = value * (1 + (PlayerCurrentStats.Momentum / PlayerCurrentStats.MaxMomentum)) + (value * PlayerCurrentStats.Chain);
                int _PnDiff = (PlayerCurrentStats.Pneumatic + _PnGain) - PlayerCurrentStats.MaxPneumatic;
                PlayerCurrentStats.Pneumatic = Mathf.Clamp(PlayerCurrentStats.Pneumatic + _PnGain, 0, PlayerCurrentStats.MaxPneumatic);
                if (PlayerCurrentStats.Pneumatic == PlayerCurrentStats.MaxPneumatic)
                {
                    if (PlayerCurrentStats.Health == PlayerCurrentStats.MaxHealth)
                    {
                        return;
                    }
                    PlayerCurrentStats.Health += 1;
                    PlayerCurrentStats.Pneumatic = _PnDiff;
                }
                break;
            case OrbType.Aggression:
                int _prevAggroStat = PlayerCurrentStats.Aggression;
                int _AggrGain = value * (1 + (PlayerCurrentStats.Momentum / PlayerCurrentStats.MaxMomentum)) + (value * PlayerCurrentStats.Chain);
                PlayerCurrentStats.Aggression = Mathf.Min(100, PlayerCurrentStats.Aggression + _AggrGain);

                //If Aggression meter is full
                if (PlayerCurrentStats.Aggression == PlayerCurrentStats.MaxAggression)
                {
                    if (!(_prevAggroStat == PlayerCurrentStats.MaxAggression))
                    {
                        AudioManager.instance.PlayFX(AudioManager.instance.fullAggro, false);
                    }
                }
                EventSystem.Current.UpdatePlayerStats(PlayerCurrentStats);
                break;
            case OrbType.Xp:
                int _skillPointEarned = Math.DivRem(value + PlayerCurrentStats.Xp, PlayerBaseStats.MaxXp, out PlayerCurrentStats.Xp);
                PlayerCurrentStats.SkillPoint += _skillPointEarned;
                break;
        }
        EventSystem.Current.UpdatePlayerStats(PlayerCurrentStats);
    }

    public void OnKillResponse()
    {
        PlayerCurrentStats.Chain = Mathf.Min(PlayerCurrentStats.Chain + 1, PlayerCurrentStats.MaxChain);
        float _attackRateReduction = (PlayerBaseStats.AttackRate * (PlayerCurrentStats.Chain * 0.20f));
        float _attackRateReductionLimit = (PlayerBaseStats.AttackRate * (PlayerBaseStats.MaxChain * 0.20f));
        if (PlayerCurrentStats.Chain >= 2) PlayerCurrentStats.AttackRate = PlayerBaseStats.AttackRate - Mathf.Min(_attackRateReduction, _attackRateReductionLimit);
        if (PlayerCurrentStats.Chain >= 1)
        {
            if (PlayerCurrentStats.Chain <= 1)
            {
                StartCoroutine(StartChain());
                Debug.Log("Player Chain: x" + PlayerCurrentStats.Chain);
                return;
            }
            PlayerCurrentStats.ChainTimer = PlayerCurrentStats.ChainDuration;
            Debug.Log("Player Chain: x" + PlayerCurrentStats.Chain);
        }
    }

    public void SavePlayerStats()
    {
        PlayerCurrentStats.KeenAbility.AbilityData = KeenAbility.GetComponentAbilityData();
        PlayerCurrentStats.DestructiveBoltAbility.AbilityData = DestructiveBoltAbility.GetComponentAbilityData();
        PlayerCurrentStats.DashAbility.AbilityData = DashAbility.GetComponentAbilityData();
        GameManager.Current.PlayerSavedStats = PlayerCurrentStats;
    }

    public override void UpdateUIData()
    {
        //Component Abilities
        KeenAbility.RefreshStatus();
        DashAbility.RefreshStatus();
        DestructiveBoltAbility.RefreshStatus();

        EventSystem.Current.UpdatePlayerStats(PlayerCurrentStats);        
    }

    public IEnumerator AttackDelay()
    {
        CanAtack = false;
        yield return new WaitForSeconds(PlayerCurrentStats.AttackRate);
        CanAtack = true;
    }

    IEnumerator StartChain()
    {
        PlayerCurrentStats.ChainTimer = PlayerCurrentStats.ChainDuration;
        while (PlayerCurrentStats.ChainTimer >= 1)
        {
            yield return new WaitForSeconds(0.1f);
            PlayerCurrentStats.ChainTimer -= 0.1f;
            Debug.Log("Chain Timer: " + PlayerCurrentStats.ChainTimer);
            EventSystem.Current.UpdatePlayerStats(PlayerCurrentStats);
        }

        PlayerCurrentStats.Chain = 0;
        PlayerCurrentStats.AttackRate = PlayerBaseStats.AttackRate;
        Debug.Log("Player Chain: x" + PlayerCurrentStats.Chain);
    }

    IEnumerator PlayerImmune()
    {
        CanTakeDamage = false;
        yield return new WaitForSeconds(ImmuneDuration);
        CanTakeDamage = true;
    }

    public void ApplyCheat(PlayerStatDelta newStat)
    {
        if (newStat.Health.HasValue) PlayerCurrentStats.Health = newStat.Health.Value;
        if (newStat.MaxHealth.HasValue) PlayerCurrentStats.MaxHealth = newStat.MaxHealth.Value;
        if (newStat.Aggression.HasValue) PlayerCurrentStats.Aggression = newStat.Aggression.Value;
        if (newStat.MaxAggression.HasValue) PlayerCurrentStats.MaxAggression = newStat.MaxAggression.Value;

        if (newStat.deltaHealth.HasValue) PlayerCurrentStats.Health += newStat.deltaHealth.Value;
        if (newStat.deltaMaxHealth.HasValue) PlayerCurrentStats.MaxHealth += newStat.deltaMaxHealth.Value;
    }

    public void ApplyUpgradeAbility(CompAbilityType ability)
    {
        if (PlayerCurrentStats.SkillPoint <= 0)
        {
            Debug.LogWarning("Player does not have any skill points for upgrade");
            return;
        }
        PlayerCurrentStats.SkillPoint--;
        EventSystem.Current.UpdatePlayerStats(PlayerCurrentStats);
        switch (ability)
        {
            case CompAbilityType.Keen:
                KeenAbility.UpgradeComponent();
                break;
            case CompAbilityType.Dash:
                DashAbility.UpgradeComponent();
                break;
            case CompAbilityType.DestructiveBolt:
                DestructiveBoltAbility.UpgradeComponent();
                break;
            default:
                Debug.LogError("Invalid Component ability");
                break;
        }
    }
}
