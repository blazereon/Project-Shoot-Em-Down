using UnityEditor;
using UnityEngine;

public class AttackPlayerState : BasePlayerState
{
    private float _meleeRange = 1.5f;
    private LayerMask _layerMask;
    private ManagerPlayerState _player;
    public override void EnterState(ManagerPlayerState player)
    {
        switch (player.attackType)
        {
            case Player.AttackType.Melee:
                MeleeAttack(player);
                break;
            case Player.AttackType.Ranged:
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

    private void DamageEnemy(RaycastHit2D[] hits)
    {
        /***
        This should be replaced with a much more flexible approach to which the
        damage of the player and the way they attack depends on the weapon type
        (melee, ranged) and the weapon itself they are using. For dev purposes,
        the following line of code should be used to test momentum, pneumatic,
        and chain mechanic.
        ***/
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider == null) continue;
            if (hit.transform.tag == "Enemy")
            {
                EventSystem.Current.AttackEnemy(hit.collider.gameObject, 15 + (15/2 * _player.PlayerCurrentStats.Chain));
                return;
            }
            if (hit.transform.tag == "Projectile")
            {
                //Modify projectile trajectory and shit
            }
        }
    }

    private void MeleeAttack(ManagerPlayerState player)
    {
        _player = player;
        Debug.Log("Attack Invoked");
        _layerMask = LayerMask.GetMask("Wall", "Enemy");
        RaycastHit2D[] hits;

        if (player.facing == Facing.right)
        {
            hits = Physics2D.RaycastAll(player.transform.position, Vector2.right, _meleeRange, _layerMask);
            Debug.DrawRay(player.transform.position, Vector2.right * _meleeRange, Color.cyan, 0.2f);
            DamageEnemy(hits);
            player.PopState();
            return;
        }
        else if (player.facing == Facing.left)
        {
            hits = Physics2D.RaycastAll(player.transform.position, Vector2.left, _meleeRange, _layerMask);
            Debug.DrawRay(player.transform.position, Vector2.right * _meleeRange, Color.cyan, 0.2f);
            DamageEnemy(hits);
            player.PopState();
            return;
        }
    }

    private void RangeAttack(ManagerPlayerState player)
    {

    }
}