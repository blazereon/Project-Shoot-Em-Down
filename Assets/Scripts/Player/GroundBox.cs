using Unity.VisualScripting;
using UnityEngine;

public class GroundBox : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool isGrounded = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            isGrounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            isGrounded = false;
        }
    }
}
