using System.Collections.Generic;
using EG.Core.Entity;
using EG.Core.Messages;
using UnityEngine;

namespace EG
{
    namespace Core.ComponentsSystem
    {

        //
        //store all goods produced by the building currently attached
        // or just to store other things like the goods produced by the blacksmith building
        //
        public class SuppliesItemsBuildingComponentLogic : BaseComponent
        {

            private BuildingLogicData buildingLogicData = null;

            
            private uint categoryId = 0; //GameEnums.EntityType type of entity that stores goods here, farm, blacksmith, etc, etc
            private uint initialGoodsItemsAmount = 0;
            private uint currentGoodsItems = 0;
            private uint maxTotalGoodsItems = 0;


            public uint GetCategoryId => categoryId;
            public uint GetCurrentGoodsItems => currentGoodsItems;
            public uint GetMaxTotalGoodsItems => maxTotalGoodsItems;
            public uint GetInitialGoodsItems => initialGoodsItemsAmount;

            
            
            #region constructor
            
            public SuppliesItemsBuildingComponentLogic(){}
            
            public SuppliesItemsBuildingComponentLogic(List<string> parameters)
            {
                // WE EXPECT 2 PARAMETERS PER "KEY" in the Json file ("Level, 2", "Money, 100", things like that...)
                for (var counter = 0; counter < parameters.Count; ++counter)
                {
                    var split = parameters[counter].Split(',');
                    var trimEmpty = split[1].Trim(' ');
                    this.parameters.Add(split[0], trimEmpty);
                }
            }
            
            #endregion
            

            #region init
            
            public override void InitComponent(uint anEntityId, params object[] args)
            {
                entityId = anEntityId;
                
                for (var i = 0; i < args.Length; ++i)
                {
                    if (args[i] is BuildingLogicData)
                    {
                        buildingLogicData = (BuildingLogicData) args[i];
                        break;
                    }
                }

                if (parameters.Count > 0)
                {
                    var categoryType = (GameEnums.EntityType) System.Enum.Parse(typeof(GameEnums.EntityType), parameters[Constants.NakedStrings.EntityType].ToString());
                    categoryId = (uint)categoryType;
                }
                else
                {
                    categoryId = (uint)buildingLogicData.GetNameId;
                }

                initialGoodsItemsAmount = (uint)buildingLogicData.GetAttributeValue(Attribute_Enums.AttributeType.GoodsItemsAttr);
                currentGoodsItems = initialGoodsItemsAmount;
                maxTotalGoodsItems = (uint)buildingLogicData.GetAttributeMaxValue(Attribute_Enums.AttributeType.GoodsItemsAttr);
                
                EG_MessagesController<EG_MessageUpdateFood>.AddObserver(
                    (int)GameEnums.MessageTypes.UpdateFood,
                    UpdateFood);
            }

            #endregion

            
            #region destroy
            
            public override void Destroy()
            {
                base.Destroy();
                DestroyStuff();
            }

            public override void DestroyUnity()
            {
                base.DestroyUnity();
                DestroyStuff();
            }

            private void DestroyStuff()
            {
                EG_MessagesController<EG_MessageUpdateFood>.RemoveObserver(
                    (int)GameEnums.MessageTypes.UpdateFood,
                    UpdateFood);
                
                buildingLogicData?.Destroy();
            }
            
            #endregion
            
            public override void Reset()
            {
                maxTotalGoodsItems = 0;
                currentGoodsItems = initialGoodsItemsAmount;
            }
            
            
            
            #region public API

            public void SetTotalGoodsItems(uint anAmount)
            {
                //only produce up to the max capacity of the building
                currentGoodsItems += anAmount;
                currentGoodsItems = (uint)Mathf.Clamp(currentGoodsItems, 0, maxTotalGoodsItems);
            }
            
            
            public bool ConsumeGoodsItems(uint anAmount)
            {
                if (currentGoodsItems <= 0) return false;

                currentGoodsItems -= anAmount;
                currentGoodsItems = (uint)Mathf.Clamp(currentGoodsItems, 0, maxTotalGoodsItems);

                return true;
            }
            
            
            
            #endregion


            private void UpdateFood(EG_MessageUpdateFood aMessage)
            {
                if (aMessage.CategoryId != (uint)buildingLogicData.GetNameId) return;
                
                ConsumeGoodsItems(aMessage.Amount);
            }
        }

    }
}
