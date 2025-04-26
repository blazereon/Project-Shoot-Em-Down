using UnityEditor;
using UnityEngine;

public class AttackPlayerState : BasePlayerState
{
    private float _meleeRange = 1.5f;
    private LayerMask _layerMask;
    public override void EnterState(ManagerPlayerState player)
    {
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
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider == null) continue;
            if (hit.transform.tag == "Enemy")
            {
                EventSystem.Current.AttackEnemy(hit.collider.gameObject, 15);
            }
        }
    }
}