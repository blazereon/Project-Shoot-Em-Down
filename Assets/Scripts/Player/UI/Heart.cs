using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    public enum Status
    {
        Enabled,
        Disabled
    }
    [SerializeField] Image HeartSprite;

    void Start()
    {
        HeartSprite = this.GetComponent<Image>();
    }

    public void SetHeart(Status status)
    {
        if (status == Status.Enabled)
        {
            HeartSprite.color = Color.red;
        }
        else
        {
            HeartSprite.color = Color.gray;
        }
    }
}
