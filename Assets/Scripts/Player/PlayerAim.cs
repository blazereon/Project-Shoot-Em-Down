using UnityEngine;

public class PlayerAim : MonoBehaviour
{
  private Transform aimTransform;

    private void Awake()
    {
        aimTransform = transform.Find("Aim");
    }

    private void Update()
    {
        Vector3 mousePosition = GetMousePosition();

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float Zangle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, Zangle);

        Debug.Log(Zangle);
        Debug.DrawLine(transform.position, mousePosition, Color.red);
    }

    public static Vector3 GetMousePosition()
    {
        Vector3 vector = GetMousePositionWithZ(Input.mousePosition, Camera.main);
        vector.z = 0f;
        return vector;
    }
    
    public static Vector3 GetMousePositionWithZ()
    {
        return GetMousePositionWithZ(Input.mousePosition, Camera.main);
    }

     public static Vector3 GetMousePositionWithZ(Camera worldCamera)
    {
        return GetMousePositionWithZ(Input.mousePosition, worldCamera);
    }

     public static Vector3 GetMousePositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
