using UnityEngine;
using UnityEngine.UI;

public class WeaponsUI : MonoBehaviour
{

    public Image Range;
    
    public Image Melee;

    public void UpdateValue(PlayerStats playstats)
    {
        if (playstats.CurrentAttackType == Player.AttackType.Melee){
            SetOpacity(Melee, 1f); // 100% opacity for Melee
            SetOpacity(Range, 0f); // 0% opacity for Range
        }else if (playstats.CurrentAttackType == Player.AttackType.Ranged)
        {
            SetOpacity(Melee, 0f); // 0% opacity for Melee
            SetOpacity(Range, 1f); // 100% opacity for Range
        }
    }

    private void SetOpacity(Image img, float alpha)
    {
        Color color = img.color;   // Get the current color of the image
        color.a = alpha;           // Set the alpha (opacity)
        img.color = color;         // Apply the new color with modified alpha
    }
}
