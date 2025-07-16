using System;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class AbilityTree : MonoBehaviour
{
    public String AbilityName;
    public Sprite Tier1Icon, Tier2Icon, Tier3Icon;
    public Sprite FrameSprite, LineSprite;
    public TreeGroup SkillTree;
    public TextMeshProUGUI AbilityNameTMP;

    [SerializeField]
    private CompAbilityType _abilityType;
    public CompAbilityType AbilityType
    {
        get
        {
            return _abilityType;
        }
        set
        {
            _abilityType = value;
            SkillTree.Tier1.AbilityType = _abilityType;
            SkillTree.Tier2.AbilityType = _abilityType;
            SkillTree.Tier3.AbilityType = _abilityType;
        }
    }

    void OnValidate()
    {
        InitializeTree();
    }

    void Start()
    {
        InitializeTree();
    }

    void InitializeTree()
    {
        //Ability Name
        if (AbilityName != null || AbilityName != "")
        {
            AbilityNameTMP.text = AbilityName;
        }
        //Icon
        if (Tier1Icon != null)
        {
            SkillTree.Tier1.IconSprite = Tier1Icon;
        }
        if (Tier2Icon != null)
        {
            SkillTree.Tier2.IconSprite = Tier2Icon;
        }
        if (Tier3Icon != null)
        {
            SkillTree.Tier3.IconSprite = Tier3Icon;
        }

        //Frame Sprite

        if (FrameSprite != null)
        {
            // foreach (Tier currentTier in SkillTree.AbilityTier)
            // {
            //     currentTier.FrameSprite = FrameSprite;
            // }
            SkillTree.Tier1.FrameSprite = FrameSprite;
            SkillTree.Tier2.FrameSprite = FrameSprite;
            SkillTree.Tier3.FrameSprite = FrameSprite;

            var MainFrame = this.GetComponent<Image>();

            MainFrame.sprite = FrameSprite;
        }

        //Line Sprite
        if (LineSprite != null)
        {
            SkillTree.Tier1.LineSprite = LineSprite;
            SkillTree.Tier2.LineSprite = LineSprite;
            SkillTree.Tier3.LineSprite = LineSprite;
        }

        //AbilityType
        AbilityType = _abilityType;
    }

    void Update()
    {
        
    }
}
