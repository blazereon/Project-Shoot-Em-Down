using System;
using System.Collections;
using Mono.Cecil.Cil;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    public enum AttackType
    {
        Melee,
        Ranged
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject projectileObject;

    public InputAction moveAction;
    public InputAction jumpAction;
    public InputAction dashAction;
    public InputAction attackAction;
    public InputAction switchWeaponAction;

    
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

    public AttackBox attackBox;
    public GroundBox groundBox;

    public void TakePlayerDamage(int damage)
    {
        AudioManager.instance.RandomSFX(AudioManager.instance.playerTakeDmg);
        PlayerCurrentStats.Health -= damage;
        if (PlayerCurrentStats.Health <= 0) 
        {
            Destroy(this.gameObject);
            AudioManager.instance.PlayFX(AudioManager.instance.playerDeath, false);
        }
        
        Debug.Log("HP: " + PlayerCurrentStats.Health);
    }

    public void ReceivePneuma(int Pneuma)
    {
        int _PnGain = Pneuma * (1 + (PlayerCurrentStats.Momentum / PlayerCurrentStats.MaxMomentum)) + (Pneuma * PlayerCurrentStats.Chain);
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