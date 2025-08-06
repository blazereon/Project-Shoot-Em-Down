using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    Transform _cam;
    Vector3 _camStartPos;
    float _distance;

    GameObject[] _backgrounds;
    Material[] _mat;
    float[] _backspeed;

    float _farthestBack;

    [Range(0.01f, 0.05f)]
    public float parallaxSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cam = Camera.main.transform;
        _camStartPos = _cam.position;

        int backCount = transform.childCount;
        _mat = new Material[backCount];
        _backspeed = new float[backCount];
        _backgrounds = new GameObject[backCount];

        for (int i = 0; i < backCount; i++)
        {
            _backgrounds[i] = transform.GetChild(i).gameObject;
            _mat[i] = _backgrounds[i].GetComponent<Renderer>().material;
        }

        BackSpeedCalculate(backCount);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        _distance = _cam.position.x - _camStartPos.x;
        float _distanceY = _cam.position.y - _camStartPos.y;

        transform.position = new Vector3(_cam.position.x, _cam.position.y, 0);
        Debug.Log("Cam pos: " + transform.position + " " + _cam.position); 

        for (int i = 0; i < _backspeed.Length; i++)
        {
            float speed = _backspeed[i] * parallaxSpeed;
            _mat[i].SetTextureOffset("_MainTex", new Vector2(_distance, 0) * speed);
        }
    }

    void BackSpeedCalculate(int backCount)
    {
        for (int i = 0; i < backCount; i++)
        {
            if ((_backgrounds[i].transform.position.z - _cam.position.z) > _farthestBack)
            {
                _farthestBack = _backgrounds[i].transform.position.z - _cam.position.z;
            }
        }

        for (int i = 0; i < backCount; i++)
        {
            _backspeed[i] = 1 - (_backgrounds[i].transform.position.z - _cam.position.z);
        }
    }
}
