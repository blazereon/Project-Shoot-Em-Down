using UnityEngine;

public class TutorialHandler : MonoBehaviour
{
    bool isSkillUpgradeInit = false;
    void Awake()
    {
        EventSystem.Current.OnUpdatePlayerStats += OnPlayerStatUpdate;
    }

    private void OnPlayerStatUpdate(PlayerStats stats)
    {
        if (stats.SkillPoint >= 1 && !isSkillUpgradeInit)
        {
            // Show tutorial for skill point
            Toast.ToastMessage msg = new Toast.ToastMessage
            {
                Title = "Skill Upgrade",
                Message = "Upon Receiving a Skill Point, You can upgrade your skills by pressing [Tab].",
                DisplayTime = 4f,
            };
            UIManager.Current.PushToast(msg);
            isSkillUpgradeInit = true;
        }
    }
}
