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
            
            public List<string> ParametersToParse = new List<string>();
            public Dictionary<string, object> Parameters = new Dictionary<string, object>();

            public void ConvertParameters()
            {
                // WE EXPECT 2 PARAMETERS PER "KEY" in the Json file ("Level, 2", "Money, 100", things like that...)
                for (var counter = 0; counter < ParametersToParse.Count; ++counter)
                {
                    var split = ParametersToParse[counter].Split(',');
                    var trimEmpty = split[1].Trim(' ');
                    Parameters.Add(split[0], trimEmpty);
                }
            }
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


        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
    

    }
}
