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
    public Tree SkillTree;
    public TextMeshProUGUI AbilityNameTMP;

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
            SkillTree.AbilityTier[0].IconSprite = Tier1Icon;
        }
        if (Tier2Icon != null)
        {
            SkillTree.AbilityTier[1].IconSprite = Tier2Icon;
        }
        if (Tier3Icon != null)
        {
            SkillTree.AbilityTier[2].IconSprite = Tier3Icon;
        }

        //Frame Sprite

        if (FrameSprite != null)
        {
            foreach (Tier currentTier in SkillTree.AbilityTier)
            {
                currentTier.FrameSprite = FrameSprite;
            }

            var MainFrame = this.GetComponent<Image>();

            MainFrame.sprite = FrameSprite;
        }

        //Line Sprite
        if (LineSprite != null)
        {
            foreach (Tier currentTier in SkillTree.AbilityTier)
            {
                currentTier.LineSprite = LineSprite;
            }
        }
    }

    void Update()
    {
        
    }
}
