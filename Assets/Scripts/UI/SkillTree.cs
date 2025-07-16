using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public Abilities AbilitiesGroup;

    void Awake()
    {
        EventSystem.Current.OnUpdateDashAbilityUI += UpdateDashAbility;
        EventSystem.Current.OnUpdateDestructiveBoltUI += UpdateDestructiveBoltAbility;
        EventSystem.Current.OnUpdateKeenAbilityUI += UpdateKeenAbility;
    }

    public void UpdateValues(CompAbilityType type, ComponentAbilityData data)
    {

        switch (type)
        {
            case CompAbilityType.Dash:
                AbilitiesGroup.Dash.SkillTree.FillAmount = (float)data.UpgradeTier;
                break;
            case CompAbilityType.DestructiveBolt:
                AbilitiesGroup.DestructiveBolt.SkillTree.FillAmount = (float)data.UpgradeTier;
                break;
            case CompAbilityType.Keen:
                AbilitiesGroup.Keen.SkillTree.FillAmount = (float)data.UpgradeTier;
                break;
            default:
                Debug.LogError("Invalid Component Ability Type");
                break;
        }
    }

    private void UpdateDashAbility(DashAbilityStatus status)
    {
        // if (AbilitiesGroup.Dash.SkillTree.FillAmount >= (float)status.AbilityData.UpgradeTier) return;
        AbilitiesGroup.Dash.SkillTree.FillAmount = (float)status.AbilityData.UpgradeTier;
    }

    private void UpdateDestructiveBoltAbility(DestructiveBoltStatus status)
    {
        // if (AbilitiesGroup.DestructiveBolt.SkillTree.FillAmount >= (float)status.AbilityData.UpgradeTier) return;
        AbilitiesGroup.DestructiveBolt.SkillTree.FillAmount = (float)status.AbilityData.UpgradeTier;
    }

    private void UpdateKeenAbility(KeenAbilityStatus status)
    {
        // if (AbilitiesGroup.Keen.SkillTree.FillAmount >= (float)status.AbilityData.UpgradeTier) return;
        AbilitiesGroup.Keen.SkillTree.FillAmount = (float)status.AbilityData.UpgradeTier;
    }
}
