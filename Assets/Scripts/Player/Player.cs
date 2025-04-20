using System.Collections;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public InputAction moveAction;
    public InputAction jumpAction;
    public InputAction dashAction;
    public InputAction attackAction;

    
    public Rigidbody2D PlayerRb;
    public PlayerStats PlayerCurrentStats;
    public float Speed = 500f;
    public float JumpForce = 5f;
    public float LandAcceleration = 2f;
    public float LandStart = 3f;
    public float DashForce;
    public float DashDuration;
    public float DashRecovery;
    public float MomentumDecayRate;

    public AttackBox attackBox;
    public GroundBox groundBox;

    public void TakePlayerDamage(int damage)
    {
        PlayerCurrentStats.Health -= damage;
        if (PlayerCurrentStats.Health <= 0) Destroy(this.gameObject);
        Debug.Log("HP: " + PlayerCurrentStats.Health);
    }

}