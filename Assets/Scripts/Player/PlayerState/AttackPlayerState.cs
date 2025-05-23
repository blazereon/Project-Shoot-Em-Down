using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackPlayerState : BasePlayerState
{
    private float _meleeRange = 1.5f;
    private LayerMask _layerMask;
    public override void EnterState(ManagerPlayerState player)
    {
        switch (player.PlayerCurrentStats.CurrentAttackType)
        {
            case Player.AttackType.Melee:
                AudioManager.instance.RandomSFX(AudioManager.instance.playerAttackMelee);
                MeleeAttack(player);
                break;
            case Player.AttackType.Ranged:
                AudioManager.instance.RandomSFX(AudioManager.instance.playerAttackRanged);
                RangeAttack(player);
                break;
            default:
                Debug.LogError("Attack type not specified");
                break;
        }
        player.PopState();

    }

    public override void UpdateState(ManagerPlayerState player)
    {
        
    }

    public override void FixedUpdateState(ManagerPlayerState player)
    {
        
    }
    public override void OnCollisionEnter2DState(Collision2D collision, ManagerPlayerState player)
    {
        
    }

    public override void OnCollisionExit2DState(Collision2D collision, ManagerPlayerState player)
    {
        
    }

    private void MeleeAttack(ManagerPlayerState player)
    {
        // _player = player;
        // Debug.Log("Melee attack invoked");
        _layerMask = LayerMask.GetMask("Wall", "Enemy", "NonCollideEnemy", "Projectile");
        Collider2D[] hits;

        if (player.facing == Facing.right)
        {
            hits = Physics2D.OverlapCircleAll(new Vector2(player.transform.position.x + player.MeleePadding, 
                                                    player.transform.position.y),
                                                    player.MeleeRadius, _layerMask);
        }
        else if (player.facing == Facing.left)
        {
            hits = Physics2D.OverlapCircleAll(new Vector2(player.transform.position.x - player.MeleePadding, 
                                                    player.transform.position.y),
                                                    player.MeleeRadius, _layerMask);
        } else {
            Debug.Log("Invalid player facing");
            return;
        }

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.tag == "EnemyProjectile")
            {
                hit.gameObject.tag = "PlayerProjectile";
                EventSystem.Current.SimpleDeflectProjectile(hit.gameObject, 20);
                return;
            }
            //melee deflect 
            else if (hit.gameObject.tag == "Enemy")
            {
                Debug.Log("enemy melee hit!!");
                EventSystem.Current.AttackEnemy(hit.gameObject, DamageType.Melee, 15 /*+ (15/2 * player.PlayerCurrentStats.Chain)*/, player.PlayerCurrentStats.Violence, false);
            }
        }

        Debug.Log("Melee attack invoked");
    }

    private void RangeAttack(ManagerPlayerState player)
    {
        if (Camera.main == null)
        {
            Debug.LogError("Camera.main is null");
        }
        
        var _projectile = Object.Instantiate(player.projectileObject, player.transform.position, player.transform.rotation);
  
        Vector2 _playerToMouseDistance = (Vector2)(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - player.transform.position).normalized;
        var _projectileProps = _projectile.GetComponent<Projectile>();

        _projectileProps.ProjectileCurrentProperties = new ProjectileProps {
            Trajectory = _playerToMouseDistance.normalized,
            ProjectileSpeed = 15,    // note: you can add public var for projectile speed
            AttackDamage = (int)(8 + (8 / 2 * player.PlayerCurrentStats.Chain)),
            DestroyOnly = LayerMask.GetMask("Ground", "Wall", "Shield"),
            FiredBy = ProjectileOwner.Player,   // both of these can be set in the inspector... or runtime
            Destination = LayerDestinations.Enemy
        };

        Debug.Log("Ranged attack invoked");
    }
}