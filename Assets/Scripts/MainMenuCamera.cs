using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<GameObject> CameraPoints;

    private int destPoint = 1;
    public float Speed = 5f;

    void OnValidate()
    {
        Camera.main.transform.position = new Vector3(CameraPoints[0].transform.position.x, CameraPoints[0].transform.position.y, -1);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("destPoint distance: " + Vector3.Distance(Camera.main.transform.position, CameraPoints[destPoint].transform.position));
        if (Vector3.Distance(Camera.main.transform.position, CameraPoints[destPoint].transform.position) < 2f)
        {
            destPoint = ++destPoint % CameraPoints.Count;
        }

        Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, new Vector3(CameraPoints[destPoint].transform.position.x, CameraPoints[destPoint].transform.position.y, -1), Speed * Time.deltaTime);
    }
}
