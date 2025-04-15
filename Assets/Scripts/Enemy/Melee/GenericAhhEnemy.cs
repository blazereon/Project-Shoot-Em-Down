using UnityEngine;

public class GenericAhhEnemy : Enemy
{
    public RaycastHit2D PlayerDetectionRay;

    void Start()
    {
        
        
    }

    void Update()
    {
        PlayerDetectionRay = Physics2D.Raycast(transform.position, Vector2.left, 10);
        Debug.DrawRay(transform.position, Vector2.left * 10, Color.red);
        if (PlayerDetectionRay.collider.tag == "Player")
        {
            Debug.Log("Player detected");
        }
    }
}