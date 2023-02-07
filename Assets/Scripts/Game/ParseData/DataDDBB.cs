using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EG.Core.AttributesSystem;
using EG.Core.Parsedata;
using EG.Core.Util;
using UnityEngine;



namespace EG
{
    namespace Core.Data
    {
        public class DataDDBB : EG_Singleton<DataDDBB>
        {
            public class EntitiesParsed
            {
                public GameEnums.EntityType Type = GameEnums.EntityType.Max;
                public List<BaseAttribute> Attributes = new List<BaseAttribute>();
                public List<ComponentJsonData> Components = new List<ComponentJsonData>();
            }

            [SerializeField] private TextAsset entitiesList = null;
            [SerializeField] private TextAsset buildingsList = null;
            

            private List<EntitiesParsed> entities = new List<EntitiesParsed>();
            private List<EntitiesParsed> buildings = new List<EntitiesParsed>();

            
            public ReadOnlyCollection<EntitiesParsed> GetEntitiesData => entities.AsReadOnly();
            public ReadOnlyCollection<EntitiesParsed> GetBuildingsData => buildings.AsReadOnly(); 
            

            
            #region init and destroy
            
            protected override void Initialize(bool dontdestroy = false)
            {
                base.Initialize(true);
            }

            public override void OnDestroy()
            {
                base.OnDestroy();
                entities.Clear();
                buildings.Clear();
            }
            
            #endregion


            public void Init()
            {
                ParseEntities(entitiesList.text, entities);
                ParseEntities(buildingsList.text, buildings);
            }




            #region parse data

            private void ParseEntities(string aJsonFile, List<EntitiesParsed> anEntitiesList)
            {
                PARSERootAttributesData allData = null;

                try
                {
                    allData = JsonUtility.FromJson<PARSERootAttributesData>(aJsonFile);
                }
                catch (Exception)
                {
                    Debug.LogError("error parsing entities json");
                }


                for (var i = 0; i < allData.Entities.Count; ++i)
                {
                    var group = new EntitiesParsed();

                    var type = (GameEnums.EntityType) System.Enum.Parse(typeof(GameEnums.EntityType), allData.Entities[i].Type);

                    group.Type = type;

                    var attrsParsed = new List<BaseAttribute>(10);
 
                    for (var j = 0; j < allData.Entities[i].Attributes.Count; ++j)
                    {
                        var formula = (Attribute_Enums.AttributeFormulas) System.Enum.Parse(typeof(Attribute_Enums.AttributeFormulas), allData.Entities[i].Attributes[j].FormulaType);
                        var attrType = (Attribute_Enums.AttributeType) System.Enum.Parse(typeof(Attribute_Enums.AttributeType), allData.Entities[i].Attributes[j].AttributeName);

                        var levels = new List<LevelsAttribute>(5);

                        for (var k = 0; k < allData.Entities[i].Attributes[j].UpdateLevels.Count; ++k)
                        {
                            levels.Add(new LevelsAttribute(allData.Entities[i].Attributes[j].UpdateLevels[k].IncreaseStep,
                                allData.Entities[i].Attributes[j].UpdateLevels[k].Value));
                        }
                        
                        var attr = new BaseAttribute(
                            IdGenerator.GetNewUId(),
                            allData.Entities[i].Attributes[j].Value,
                            allData.Entities[i].Attributes[j].MinValue,
                            allData.Entities[i].Attributes[j].MaxValue,
                            allData.Entities[i].Attributes[j].Cost,
                            allData.Entities[i].Attributes[j].IsUpdateable,
                            levels,
                            formula,
                            attrType
                        );

                        attrsParsed.Add(attr);
                    }


                    List<ComponentJsonData> tmpComponentsList = new List<ComponentJsonData>();
                    for (var j = 0; j < allData.Entities[i].Components.Count; ++j)
                    {
                        allData.Entities[i].Components[j].ConvertParameters();
                        tmpComponentsList.Add(allData.Entities[i].Components[j]);
                    }
                    
                    group.Attributes = new List<BaseAttribute>(attrsParsed);
                    group.Components = new List<ComponentJsonData>(tmpComponentsList);
                    
                    anEntitiesList.Add(group);
                }
                
            }
            

            #endregion
            
        }

    }
}
