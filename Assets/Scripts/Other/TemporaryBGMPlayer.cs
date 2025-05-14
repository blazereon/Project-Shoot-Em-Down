using UnityEngine;

public class TemporaryBGMPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Start Playing");
        AudioManager.instance.PlayMusic(AudioManager.instance.levelBGM);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
