using UnityEngine;

public class PlungePlayerState : BasePlayerState
{
    float midAirStopDuration = 0.25f;
    float midAirStopTimer;

    float plungeRadius = 2.5f;

    bool isDropping;

    public override void EnterState(ManagerPlayerState player)
    {
        midAirStopTimer = 0;
        isDropping = false;
    }
    public override void UpdateState(ManagerPlayerState player)
    {
        if (midAirStopTimer <= midAirStopDuration && !isDropping)
        {
            player.PlayerRb.linearVelocity = Vector2.zero;
            midAirStopTimer += Time.deltaTime;
            isDropping = false;
            return;
        }

        isDropping = true;

        //hard drop sub state
        player.PlayerRb.linearVelocityY = -30;

        if (player.groundBox.isGrounded)
        {
            DropAoE(player);
            player.PopState();
        }

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

    public void DropAoE(ManagerPlayerState player)
    {
        var _layerMask = LayerMask.GetMask("Enemy", "NonCollideEnemy", "Projectile");
        Collider2D[] hits = Physics2D.OverlapCircleAll(player.transform.position, plungeRadius, _layerMask);

        if (hits == null)
        {
            return;
        }

        foreach (var hit in hits)
        {
            if (hit.gameObject.tag == "Enemy")
            {
                EventSystem.Current.AttackEnemy(hit.gameObject, DamageType.Melee, 40, 0, false);
            }
        }
    }
}