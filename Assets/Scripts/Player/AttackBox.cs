using UnityEngine;

public class AttackBox : MonoBehaviour
{

    [SerializeField]
    public Vector2 AttackLeftPos, AttackRightPos;

    public Vector2 AttackLeftRelPos, AttackRightRelPos;
    public float AttackRadius;
    Player.Facing facing;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void SwitchDirection(Player.Facing setFacing)
    {
        facing = setFacing;
    }

    public void Attack(Player.Facing face)
    {
        AttackLeftRelPos = AttackLeftPos + (Vector2)transform.position;
        AttackRightRelPos = AttackRightPos + (Vector2)transform.position;
        
        Collider2D[] collidedObject;
        if (face == Player.Facing.left){
            collidedObject = Physics2D.OverlapCircleAll(AttackLeftRelPos, AttackRadius);
        } else if (face == Player.Facing.right) {
            collidedObject = Physics2D.OverlapCircleAll(AttackRightRelPos, AttackRadius);
        } else {
            Debug.LogWarning("No facing direction selected");
            return;
        }
        
        foreach (var obj in collidedObject)
        {
            if (obj.tag == "Enemy")
            {
                EventSystem.Current.AttackEnemy(obj.gameObject, 20);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackLeftPos, AttackRadius);
        Gizmos.DrawWireSphere(AttackRightPos, AttackRadius);

        Gizmos.DrawWireSphere(AttackLeftRelPos, AttackRadius);
        Gizmos.DrawWireSphere(AttackRightRelPos, AttackRadius);


    }

}
