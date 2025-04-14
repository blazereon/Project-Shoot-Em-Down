using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Health = 100;
    public int AttackDamage;
    
    public enum DamageType {
        Melee,
        Ranged
    }

    public void TakeDamage(GameObject pObject, int damage)
    {
        if (pObject == gameObject){
            Health -= damage;
            if (Health <= 0) Destroy(this.gameObject);
            Debug.Log("HP: " + Health);
        }
    }

}