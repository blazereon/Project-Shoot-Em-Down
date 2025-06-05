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

    public Dash DashAbility = new Dash();
    public Keen KeenAbility;
    public DestructiveBolt DestructiveBoltAbility;
    
    public Rigidbody2D PlayerRb;
    public SpriteRenderer PlayerSprite;
    public PlayerStats PlayerCurrentStats;
    public float Speed = 500f;
    public float JumpForce = 10f;
    public float LandAcceleration = 2f;
    public float LandStart = 3f;
    public float DashForce;
    public float DashDuration;
    public float DashRecovery;
    public float MomentumDecayRate;

    public float MeleePadding;
    public float MeleeRadius;

    public GameObject DashAttackBox;
    public GroundBox groundBox;

    [NonSerialized] public bool IsEmpowerementInvoke;

    public void TriggerEmpowerment(ComponentAbility ability)
    {
        if (!IsEmpowerementInvoke) return;
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
            Destroy(this.gameObject);
            AudioManager.instance.PlayFX(AudioManager.instance.playerDeath, false);
        }

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
                break;
        }
        EventSystem.Current.UpdatePlayerStats(PlayerCurrentStats);
    }

    public void OnKillResponse()
    {
        PlayerCurrentStats.Chain = Mathf.Min(PlayerCurrentStats.Chain + 1, PlayerCurrentStats.MaxChain);
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

    public override void UpdateUIData()
    {
        //does nothing yet
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
        Debug.Log("Player Chain: x" + PlayerCurrentStats.Chain);
    }
}