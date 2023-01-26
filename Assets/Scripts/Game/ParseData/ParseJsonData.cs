using UnityEngine;
using System.Collections.Generic;


namespace EG
{
    namespace Core.Parsedata
    {

        #region components

        [System.Serializable]
        public class PARSERootComponentsJsonData
        {
            public List<ComponentJsonData> Components = new List<ComponentJsonData>();
        }
        
        [System.Serializable]
        public class ComponentJsonData
        {
            public string ClassName = System.String.Empty;
        }

        #endregion



        #region attributes base
        
        [System.Serializable]
        public class PARSERootAttributesData
        {
            public List<ParseGrouppedAttributesData> Entities = new List<ParseGrouppedAttributesData>();
        }
        
        [System.Serializable]
        public class ParseGrouppedAttributesData
        {
            public string Type = System.String.Empty;
            [System.NonSerialized] public GameEnums.EntityType EntityType = GameEnums.EntityType.Max;
            public List<AttributesJsonData> Attributes = new List<AttributesJsonData>();
            public List<ComponentJsonData> Components = new List<ComponentJsonData>();
        }
        
        [System.Serializable]
        public class AttributesJsonData
        {
            public uint Id = 0;
            public float Value = 0f;
            public float MinValue = 0f;
            public float MaxValue = 0f;
            public int Cost = 0;
            public bool IsUpdateable = false;
            public List<LevelsAttributeJsonData> UpdateLevels = new List<LevelsAttributeJsonData>(15);
            public string AttributeName = System.String.Empty;
            public string FormulaType = System.String.Empty;
            [System.NonSerialized] public Attribute_Enums.AttributeType Attribute = Attribute_Enums.AttributeType.Max;
            [System.NonSerialized] public Attribute_Enums.AttributeFormulas Formula = Attribute_Enums.AttributeFormulas.Max;
        }

        [System.Serializable]
        public class LevelsAttributeJsonData
        {
            [SerializeField] public float IncreaseStep = 0f;
            [SerializeField] public float Value = 0f;
        }

        [System.Serializable]
        public class ModifierJsonData
        {
            public uint Id; //try to put a unique id, that way same modifier cant be added twice
            public uint EntityId;
            public bool Overwrite = false;
            public string FormulaParsed = System.String.Empty; // formulas "percent" "addition", maybe more to add later
            public int TimeActive = 0;
            public float Value = 0f;
            public string AttributeParsed = System.String.Empty;
            public string EffectNameParsed = System.String.Empty;
            [System.NonSerialized] public Attribute_Enums.AttributeFormulas Formula = Attribute_Enums.AttributeFormulas.Max;
            [System.NonSerialized] public Attribute_Enums.AttributeType Attribute = Attribute_Enums.AttributeType.Max;
        }

        #endregion


        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        // #region character
        //

        //
        // [System.Serializable]
        // public class CharacterParseJsonData
        // {
        //     public string ClassName = System.String.Empty;
        //     public string PortraitName = System.String.Empty;
        //     public string PrefabName = System.String.Empty;
        //     public string AnimationNameId = System.String.Empty;
        //     public string GroupType = System.String.Empty;
        //     [System.NonSerialized] public GameEnums.GroupTypes Group = GameEnums.GroupTypes.Max;
        //     public List<AttributesJsonData> VisualAttributes = new List<AttributesJsonData>();
        //     public List<AttributesJsonData> Attributes = new List<AttributesJsonData>();
        //     public List<SkillsParseJsonData> Skills = new List<SkillsParseJsonData>();
        //     public List<ComponentJsonData> Components = new List<ComponentJsonData>();
        // }
        //
        // [System.Serializable]
        // public class PARSERootCharactersJsonData
        // {
        //     public List<CharacterParseJsonData> Characters = new List<CharacterParseJsonData>();
        // }
        //
        // #endregion
        //
        //
        // #region animations for characters
        //
        // [System.Serializable]
        // public class AnimationsParseJsonData
        // {
        //     public string AnimationName = System.String.Empty;
        //     public string ClipName = System.String.Empty;
        //     public string VariableName = System.String.Empty;
        //     public float OffsetAnimation = 0f;
        //     public float DelayStartAnimation = 0f;
        //     public int Hash = 0;
        //     public float AnimationLength = 0f;
        //     public bool AnimationIsPlaying = false;
        //     public bool IsLoop = false;
        // }
        //
        // [System.Serializable]
        // public class ControllersAnimationParseJsonData
        // {
        //     public string Name = System.String.Empty;
        //     public List<AnimationsListJsonData> AnimationList = new List<AnimationsListJsonData>();
        // }
        //
        // [System.Serializable]
        // public class AnimationsListJsonData
        // {
        //     public string PivotAnimatorName = System.String.Empty;
        //     public List<AnimationsParseJsonData> Animations = new List<AnimationsParseJsonData>();
        // }
        //
        // [System.Serializable]
        // public class PARSERootAnimationControllersJsonData
        // {
        //     public List<ControllersAnimationParseJsonData> CharacterAnimations =
        //         new List<ControllersAnimationParseJsonData>();
        // }
        //
        // #endregion



        // #region skills
        //
        // [System.Serializable]
        // public class SkillsParseJsonData
        // {
        //     public uint Id = 0;
        //     public int Cost = 0;
        //     public float Value = 0f;
        //     public float MinValue = 0f;
        //     public float MaxValue = 0f;
        //     public int TimeActive = -1;
        //     public string GroupParsed = System.String.Empty;
        //     public string AttributeName = System.String.Empty;
        //     public List<LevelsAttributeJsonData> UpdateLevels = new List<LevelsAttributeJsonData>();
        //     public string FormulaType = System.String.Empty;
        //     public string Name = System.String.Empty;
        //     public List<ModifierJsonData> Modifiers = new List<ModifierJsonData>();
        //
        //     [System.NonSerialized] public Attribute_Enums.AttributeType Attribute = Attribute_Enums.AttributeType.Max;
        //     [System.NonSerialized] public GameEnums.GroupTypes Group = GameEnums.GroupTypes.Max;
        // }
        //
        // [System.Serializable]
        // public class SkillsListData
        // {
        //     public string ClassName = System.String.Empty;
        //     public List<SkillsParseJsonData> Skills = new List<SkillsParseJsonData>();
        // }
        //
        // [System.Serializable]
        // public class PARSEAllSkillsData
        // {
        //     public List<SkillsListData> AllSkills = new List<SkillsListData>();
        // }
        //
        // #endregion


    }
}