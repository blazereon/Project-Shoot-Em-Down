using UnityEngine;

public class Void : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            EventSystem.Current.AttackPlayer(1);
            collision.transform.position = CheckpointSystem.Current.CheckpointTransform.position;
        }
    }
}