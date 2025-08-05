using UnityEngine;

public class Endflag : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Endflag test");
        if (collision.tag == "Player")
        {
            UIManager.Current.CurrentState = UIState.Complete;
            Debug.Log("level complete");
        }
    }
}
