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


    EventSystem() { }
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
    public Collider2D PlayerCollider;

    //Event handlers
    public event Action<GameObject, int> OnAttackEnemy;
    public event System.Action<GameObject, DamageType, int, int, bool> OnDamageEnemy;
    public event Action<int> OnAttackPlayer;
    public event Action<int> OnSendPlayerPneuma;
    public event Action<GameObject> OnMeleeDeflect;
    public event Action<ComponentAbility> OnPlayerEmpowermentTrigger;
    public event Action OnEnemyKill;

    //Player Related Events
    public event Action<GameObject, Effect> OnApplyEffect;
    public event Func<GameObject> OnPlayerGameObject;
    public event Action<PlayerStats> OnUpdatePlayerStats;
    public event Action<PlayerDebug> OnUpdatePlayerDebug;

    //Player Comonent Ability related Events
    public event Action<DashAbilityStatus> OnUpdateDashAbilityUI;
    public event Action<KeenAbilityStatus> OnUpdateKeenAbilityStatus;

    public event Action<GameObject, ProjectileProps> OnModifyProjectile;
    public event Action<GameObject, float> OnSimpleDeflectProjectile;


    //This is where you add the event trigger function
    public void AttackEnemy(GameObject enemyObject, DamageType type, int damage, int violence, bool weakSpotHit)
    {
        OnDamageEnemy?.Invoke(enemyObject, type, damage, violence, weakSpotHit);
    }

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

    public void ModifyProjectile(GameObject projectile, ProjectileProps props)
    {
        OnModifyProjectile?.Invoke(projectile, props);
    }

    public void SimpleDeflectProjectile(GameObject projectile, float speed)
    {
        OnSimpleDeflectProjectile?.Invoke(projectile, speed);
    }
    public void EnemyKill()
    {
        OnEnemyKill?.Invoke();
    }

    public void ApplyEffect(GameObject pObject, Effect effect)
    {
        OnApplyEffect?.Invoke(pObject, effect);
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

    //Events related to Player Component Abilities
    public void UpdateDashAbilityUI(DashAbilityStatus dashStatus)
    {
        OnUpdateDashAbilityUI?.Invoke(dashStatus);
    }

    public void UpdateKeenAbilityStatus(KeenAbilityStatus keenStatus)
    {
        OnUpdateKeenAbilityStatus?.Invoke(keenStatus);
    }
}
