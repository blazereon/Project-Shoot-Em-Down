using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialHandler : MonoBehaviour
{
    bool isSkillUpgradeInit = false;
    bool isSkillTreeExecute = false;
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

    void Update()
    {
        if (isSkillUpgradeInit && !isSkillTreeExecute)
        {
            if (UIManager.Current.CurrentState == UIState.SkillTree)
            {
                // Show tutorial for skill tree
                var msg = new Toast.ToastMessage
                {
                    Title = "Skill Tree",
                    Message = "Hover over skill to see its functionalities. Each skill consumes 1 skill point",
                    DisplayTime = 3f,
                };
                UIManager.Current.PushToast(msg);

                var msg2 = new Toast.ToastMessage
                {
                    Title = "Skill Tree",
                    Message = "Left click and hold the selected skill until the whole skill turns into a color",
                    DisplayTime = 3f,
                };
                UIManager.Current.PushToast(msg2);
                isSkillTreeExecute = true;
            }
        }
    }
}
