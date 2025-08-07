using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Credits : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    TextMeshProUGUI tmpCredits;
    RectTransform rectTransform;
    void Start()
    {

    }

    void OnValidate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += Vector3.up * 40 * Time.deltaTime;

    }
}
