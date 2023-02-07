using System.Collections.Generic;
using EG.Core.AttributesSystem;
using EG.Core.Entity;
using EG.Core.Messages;


namespace EG
{
    namespace Core.ComponentsSystem
    {

        //
        //store all goods produced by buildings, used to feed the people, or for trade, sell, whatever...
        //
        public class StorageBuildingComponentLogic : BaseComponent
        {

            private BuildingLogicData buildingLogicData = null;
            private PaymentsBuildingComponentLogic paymentsBuildingComponentLogic = null;
            private List<SuppliesItemsBuildingComponentLogic> suppliesBuildingComponents = null;
            private EG_MessageUpdateFood messageUpdateFood = null;
            
            
            #region init

            public override void InitComponent(uint anEntityId, params object[] args)
            {
                messageUpdateFood = new EG_MessageUpdateFood();
                suppliesBuildingComponents = new List<SuppliesItemsBuildingComponentLogic>();
                
                entityId = anEntityId;
                
                for (var i = 0; i < args.Length; ++i)
                {
                    if (args[i] is BuildingLogicData)
                    {
                        buildingLogicData = (BuildingLogicData) args[i];
                    }
                    else if (args[i] is List<BaseComponent>)
                    {
                        List<BaseComponent> listComponents = (List<BaseComponent>) args[i];

                        for (var j = 0; j < listComponents.Count; ++j)
                        {
                            BaseComponent component = listComponents[j];

                            if (component is SuppliesItemsBuildingComponentLogic logicSupplies)
                            {
                                suppliesBuildingComponents.Add(logicSupplies);
                            }
                            
                            if (component is PaymentsBuildingComponentLogic payment)
                            {
                                paymentsBuildingComponentLogic = payment;
                            }
                        }
                    }
                }
                
                EG_MessagesController<EG_MessageConsumeFood>.AddObserver(
                    (int)GameEnums.MessageTypes.ConsumeFood,
                    ConsumeFood);
                
                EG_MessagesController<EG_MessageStoreGoodsSupplies>.AddObserver(
                    (int)GameEnums.MessageTypes.StoreGoodsSupplies,
                    StoreGoodsSupplies);
                
                EG_MessagesController<EG_MessageStoreMoney>.AddObserver(
                    (int)GameEnums.MessageTypes.StoreMoney,
                    StoreMoney);
                
                EG_MessagesController<EG_MessageUpdatePayments>.AddObserver(
                    (int)GameEnums.MessageTypes.UpdatePayments,
                    UpdateMoney);
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
                EG_MessagesController<EG_MessageConsumeFood>.RemoveObserver(
                    (int)GameEnums.MessageTypes.ConsumeFood,
                    ConsumeFood);
                
                EG_MessagesController<EG_MessageStoreGoodsSupplies>.RemoveObserver(
                    (int)GameEnums.MessageTypes.StoreGoodsSupplies,
                    StoreGoodsSupplies);
                
                EG_MessagesController<EG_MessageStoreMoney>.RemoveObserver(
                    (int)GameEnums.MessageTypes.StoreMoney,
                    StoreMoney);
                
                EG_MessagesController<EG_MessageUpdatePayments>.RemoveObserver(
                    (int)GameEnums.MessageTypes.UpdatePayments,
                    UpdateMoney);

                messageUpdateFood = null;
                
                buildingLogicData?.Destroy();
                
                paymentsBuildingComponentLogic?.Destroy();
                
                for (var i = suppliesBuildingComponents.Count-1; i > -1; --i)
                {
                    suppliesBuildingComponents[i]?.Destroy();
                }
            }
            
            #endregion
            
            
            public override void Reset()
            {
                paymentsBuildingComponentLogic?.Reset();
                for (var i = suppliesBuildingComponents.Count-1; i > -1; --i)
                {
                    suppliesBuildingComponents[i]?.Reset();
                }
            }
            
            


            #region store money message
            
            private void StoreMoney(EG_MessageStoreMoney aMessage)
            {
                paymentsBuildingComponentLogic.SetMoneyEarned(aMessage.Amount);
            }

            private void UpdateMoney(EG_MessageUpdatePayments aMessage)
            {
                paymentsBuildingComponentLogic.UpdateTotalMoney(aMessage.Amount);
            }
            
            #endregion
            
            
            #region store goods supplies message
            
            private void StoreGoodsSupplies(EG_MessageStoreGoodsSupplies aMessage)
            {
                SuppliesItemsBuildingComponentLogic tmp = GetSuppliesDepot(aMessage.CategoryEnumId);

                tmp?.SetTotalGoodsItems(aMessage.Amount);
            }
            
            #endregion
            
            
            #region food message
            
            private void ConsumeFood(EG_MessageConsumeFood aMessage)
            {
                SuppliesItemsBuildingComponentLogic tmp = GetSuppliesDepot(aMessage.CategoryId);
                
                var entity = buildingLogicData.GetEntity(aMessage.CategoryId, aMessage.SenderId);

                //we were unable to eat, no food on the building!!
                if (tmp?.ConsumeGoodsItems(aMessage.Amount) == false)
                {
                    SetNoFood(entity);
                }
                else
                {
                    SendUpdateFood(aMessage.CategoryId, aMessage.Amount);
                    ResetNoFood(entity);
                }
            }
            
            private void SetNoFood(EntityLogicData anEntity)
            {
                Modifier modifier = AttributesAndModifiersController.CreateModifier(
                    IdGenerator.GetNewUId(),
                    anEntity.GetId,
                    -1,
                    false,
                    Attribute_Enums.AttributeFormulas.Addition,
                    1,
                    Attribute_Enums.AttributeType.DaysWithoutFoodAttr,
                    false);
                
                anEntity.AddModifier(modifier);
            }
            
            private void ResetNoFood(EntityLogicData anEntity)
            {
                anEntity.RemoveAllModifiersWithType(Attribute_Enums.AttributeType.DaysWithoutFoodAttr); 
            }

            private void SendUpdateFood(uint aCategoryId, uint anAmount)
            {
                messageUpdateFood.SetData(aCategoryId, anAmount, buildingLogicData.GetId);
                
                EG_MessagesController<EG_MessageUpdateFood>.Post(
                    (int)GameEnums.MessageTypes.UpdateFood,
                    messageUpdateFood);
            }
            
            #endregion


            
            private SuppliesItemsBuildingComponentLogic GetSuppliesDepot(uint anId)
            {
                SuppliesItemsBuildingComponentLogic tmp = null;
                for (var i = 0; i < suppliesBuildingComponents.Count; ++i)
                {
                    if (suppliesBuildingComponents[i].GetCategoryId != anId) continue;
                    
                    tmp = suppliesBuildingComponents[i];
                    break;
                }

                return tmp;
            }
        }

    }
}
