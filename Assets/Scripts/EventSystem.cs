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
    //This is where you subscribe functions to events


    //Global variables
    public Vector2 PlayerLocation; //player transform.position
    
    //Event handlers
    public event Action<GameObject, int> OnAttackEnemy;

    public event Action<int> OnAttackPlayer;
    public event Action<int> OnSendPlayerPneuma;
    public event Action OnEnemyKill;


    public event Func<GameObject> OnPlayerGameObject;
    public event Action<PlayerStats> OnUpdatePlayerStats;
    public event Action<PlayerDebug> OnUpdatePlayerDebug;


    
    //This is where you add the event trigger function
    public void AttackEnemy(GameObject enemyObject, int damage)
    {
        OnAttackEnemy?.Invoke(enemyObject, damage);
    }

    public void AttackPlayer(int damage)
    {
        OnAttackPlayer?.Invoke(damage);
    }

    public void SendPlayerPneuma(int Pneuma)
    {
        OnSendPlayerPneuma?.Invoke(Pneuma);
    }

    public void EnemyKill()
    {
        OnEnemyKill?.Invoke();
    }
    
    public void UpdatePlayerStats(PlayerStats stats)
    {
        OnUpdatePlayerStats?.Invoke(stats);
    }

    public void UpdatePlayerDebug(PlayerDebug playerDebugData)
    {
        OnUpdatePlayerDebug?.Invoke(playerDebugData);
    }
    public GameObject GetPlayerGameObject()
    {
        return OnPlayerGameObject?.Invoke();
    }
}
