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
                if (!player.KeenAbility.IsTandemCooldown && player.KeenAbility.UpgradeTier >= 3) player.KeenAbility.TriggerTandem();
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

        _projectileProps.ProjectileCurrentProperties = new ProjectileProps
        {
            Trajectory = _playerToMouseDistance.normalized,
            ProjectileSpeed = player.PlayerCurrentStats.ProjectileSpeed,    // note: you can add public var for projectile speed
            isPiercing = false,
            AttackDamage = (int)(8 + (8 / 2 * player.PlayerCurrentStats.Chain)),
            DestroyOnly = LayerMask.GetMask("Ground", "Wall", "Shield"),
            FiredBy = ProjectileOwner.Player,   // both of these can be set in the inspector... or runtime
            Destination = LayerDestinations.Enemy
        };

        if (player.DestructiveBoltAbility.UpgradeTier >= 1)
        {
            _projectileProps.ProjectileCurrentProperties.isPiercing = true;

            if (player.DestructiveBoltAbility.IsNextBulletEmpowered)
            {
                _projectileProps.ProjectileCurrentProperties.DestroyOnly = LayerMask.GetMask("Ground", "Wall");
            }
        }

        if (player.DestructiveBoltAbility.UpgradeTier >= 2)
        {
            if (Random.value < 0.25f)
            {
                var _stun = new Stun(null, 1f);
                _projectileProps.EffectsList.Add(_stun);
            }

        }

        if (player.DestructiveBoltAbility.UpgradeTier >= 3)
        {
            _projectileProps.ProjectileCurrentProperties.ProjectileSpeed = player.PlayerCurrentStats.ProjectileSpeed * 1.5f;

            //If bullet is empowered
            if (player.DestructiveBoltAbility.IsNextBulletEmpowered)
            {
                _projectileProps.EffectsList.Add(new Instakill(null, 4));
            }

        }


        Debug.Log("Tandem status: " + player.KeenAbility.IsTandemTriggered);

        //Add keen to effects list in projectile
        if (player.KeenAbility.IsTandemTriggered && player.KeenAbility.UpgradeTier >= 3)
        {
            player.KeenAbility.ConsumeTandem();
            var _aetherMark = new AetherMark(null, 4);
            _aetherMark.KeenInstance = player.KeenAbility;
            _projectileProps.EffectsList.Add(_aetherMark);

        }

        Debug.Log("Ranged attack invoked");
        player.DestructiveBoltAbility.IsNextBulletEmpowered = false;
        EventSystem.Current.UpdateDestructiveBoltUI(player.DestructiveBoltAbility.GetCurrentStatus());
    }
}