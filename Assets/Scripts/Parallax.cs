using UnityEngine;

public class Parallax : MonoBehaviour
{
    Material _mat;
    float _distance;

    [Range(0f, 0.5f)]
    public float speed = 0.2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     _mat = GetComponent<Renderer>().material;   
    }

    // Update is called once per frame
    void Update()
    {
        _distance += Time.deltaTime * speed;
        _mat.SetTextureOffset("_MainTex", Vector2.right * _distance);
    }
}
