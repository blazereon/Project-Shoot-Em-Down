using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EventSystem
{

    /*
    It's just a singleton pattern. Ignore it
    vvvvvvv
    */
    private static EventSystem current = null;
    private static readonly object padlock = new object();

    public enum DamageType
    {
        Melee,
        Ranged
    }

    EventSystem() {}
    public static EventSystem Current
    {
        get
        {
            lock (padlock)
            {
                if (current == null)
                {
                    current = new EventSystem();
                }
                return current;
            }
        }
    }
     /*
     ^^^^^
    It's just a singleton pattern. Ignore it
    */

    private void Awake()
    {
        current = this;
    }

    //This is where you subscribe functions to events
    public event Action<GameObject, int> OnAttackEnemy;


    //This is where you add the event trigger function
    public void AttackEnemy(GameObject enemyObject, int damage, DamageType damageType)
    {
        OnAttackEnemy?.Invoke(enemyObject, damage);
    }

}
