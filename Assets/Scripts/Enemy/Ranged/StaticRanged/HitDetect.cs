using UnityEngine;

public class HitDetect : MonoBehaviour
{

    private bool _playerDetected = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool PlayerDetected()
    {
        return _playerDetected;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Entered");
        if (collider.tag == "Player")
        {
            _playerDetected = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        Debug.Log("Exited");
        if (collider.tag == "Player")
        {
            _playerDetected = false;
        }
    }
}
