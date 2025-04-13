using UnityEngine;

public class AttackBox : MonoBehaviour
{

    [SerializeField]
    public float AttackRadius;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Attack()
    {
        Collider2D[] collidedObject = Physics2D.OverlapCircleAll(transform.position, AttackRadius);
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
        Gizmos.DrawWireSphere(transform.position, AttackRadius);     
    }

}
