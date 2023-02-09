using System.Collections.Generic;
using EG.Core.Entity;
using EG.Core.Interfaces;
using EG.Core.Messages;


namespace EG
{
    namespace Core.ComponentsSystem
    {

        public class MarketplaceBuildingComponentLogic : BaseComponent
        {
            
            private BuildingLogicData buildingLogicData = null;
            private System.Action<uint> onComponentCompletedAction = null;
            private IWorkData workData = null;
            private int counterEntitiesWorking = 0;
            private EntityState tmpState = null;
            private PaymentsBuildingComponentLogic paymentsBuildingComponent = null;
            private EG_MessageStoreGoodsSupplies messageStoreGoodsSupplies = null;
            private EG_MessageStoreMoney messageStoreMoney = null;
            private EntityLogicData tmpEntity = null;


            #region component

            public override void InitComponent(uint anEntityId, params object[] args)
            {
                messageStoreGoodsSupplies = new EG_MessageStoreGoodsSupplies();
                messageStoreMoney = new EG_MessageStoreMoney();
                
                tmpState = new EntityState();
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
                            
                            if (component is PaymentsBuildingComponentLogic logic) paymentsBuildingComponent = logic;
                        }
                    }
                }
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
                onComponentCompletedAction = null;
                buildingLogicData?.Destroy();
                workData?.Reset();
                tmpState?.Destroy();
                tmpEntity = null;
                messageStoreMoney = null;
                messageStoreGoodsSupplies = null;
            }
            
            #endregion


            #region data

            public override void SetData(IWorkData aWorkData)
            {
                if (aWorkData.Id != (int)GameEnums.WorkAction.Market) return;
                
                CheckCurrentWorkToResetIfDataChanged(aWorkData);

                workData = aWorkData.Clone();
            }

            #endregion
            
            
            public override void DoUpdate(float value = 1)
            {
                tmpState?.OnUpdate(value);
            }

            public override void UpdateDayData()
            {
                base.UpdateDayData();

                //every day our shopkeeper gets paid...
                if (tmpEntity == null)
                {
                    tmpEntity = buildingLogicData.GetEntityByPosition((uint) buildingLogicData.GetNameId, 0);
                }

                //get how much amount we owe this guy
                uint cost = (uint) tmpEntity.GetAttributeValue(Attribute_Enums.AttributeType.SalaryAttr);
                
                //let's set first the payment
                SetPayments(cost);
            }


            public override void Start(
                uint aDays,
                uint aDelayDays,
                System.Action<float, float> anUpdateProgress,
                System.Action<float, float> anUpdateDelayProgress,
                System.Action<uint> onComplete)
            {
                if (tmpState.IsBusy) return;
                
                counterEntitiesWorking = 0;
                
                onComponentCompletedAction = onComplete;

                uint totalWork = 0;
                var total = buildingLogicData.GetTotalEntities;
                
                for (var i = 0; i < total; ++i)
                {
                    EntityLogicData entity = buildingLogicData.GetEntityByPosition((uint)buildingLogicData.GetNameId, i);
                    
                    WorkComponentLogic component = entity.GetLogicComponent<WorkComponentLogic>();
                    
                    //get how fast the work will be done
                    uint valueWork = (uint)entity.GetAttributeValue(Attribute_Enums.AttributeType.WorkTimeAttr);      
                    workData.SetWorkTime(valueWork);

                    totalWork = valueWork;
                    
                    component?.SetData(workData);
                    
                    component?.Start(
                        valueWork,
                        aDelayDays,
                        anUpdateProgress,
                        anUpdateDelayProgress,
                        OnCompleteComponentAction);
                }
                
                //we have a fake update here, cause I want to know when all workers ended the assigned work
                //to unlock the component to do more work
                tmpState.Init(totalWork, aDelayDays, anUpdateProgress, anUpdateDelayProgress, OnCompleteFakeMarketJob);
            }

            
            #region finished work
            
            private void OnCompleteFakeMarketJob()
            {
                tmpState.OnCancelUpdate();
            }


            private void OnCompleteComponentAction(uint anEntityId)
            {
                var entity = buildingLogicData.GetEntity((uint)buildingLogicData.GetNameId, anEntityId);

                //the entity died or something, so return an empty one (if needed)
                if (entity == null)
                {
                    SetCounterEntityWorking();
                    
                    if (CheckAllEntitiesFinishedWorking())
                    {
                        onComponentCompletedAction?.Invoke(0);
                    }

                    return;
                }
                
                //get our work component present in the worker who finished the job
                WorkComponentLogic component = entity.GetLogicComponent<WorkComponentLogic>();
                
                //now set how much work this guy did
                SetGoodsSold(component);

                //finish our function with all the relevant properties
                SetCounterEntityWorking();
                
                if (CheckAllEntitiesFinishedWorking())
                {
                    onComponentCompletedAction?.Invoke(entityId);
                }
                
                ResetWorks(entity);
            }
            
            private void SetPayments(uint aCost)
            {
                //remove this to save some performance
                if (paymentsBuildingComponent == null) return;
                
                paymentsBuildingComponent?.SetTotalPaymentSalaries(aCost);
            }
            
            private void SetGoodsSold(WorkComponentLogic component)
            {
                //decrease our goods by the amount sold
                messageStoreGoodsSupplies.SetData((uint)buildingLogicData.GetNameId, component.GetCurrentResultAmount, buildingLogicData.GetId);
                
                EG_MessagesController<EG_MessageStoreGoodsSupplies>.Post(
                    (int)GameEnums.MessageTypes.DecreaseGoodsSupplies,
                    messageStoreGoodsSupplies);
                
                //update the money as we get paid for selling something
                var earnings = component.GetCurrentResultItemAmount * component.GetCurrentCost;
                
                messageStoreMoney.SetData((uint)buildingLogicData.GetNameId, earnings, buildingLogicData.GetId);
                EG_MessagesController<EG_MessageStoreMoney>.Post(
                    (int)GameEnums.MessageTypes.StoreMoney,
                    messageStoreMoney);
                
            }
            
            #endregion
            


            #region misc private

            private void SetCounterEntityWorking()
            {
                counterEntitiesWorking++;
            }
            private bool CheckAllEntitiesFinishedWorking()
            {
                return counterEntitiesWorking == buildingLogicData.GetTotalEntities;
            }
            
            private void CheckCurrentWorkToResetIfDataChanged(IWorkData aData)
            {
                if (workData == null) return;

                if (workData != aData)
                {
                    workData.Reset();
                }
            }
            
            private void ResetWorks(EntityLogicData aEntity)
            {
                aEntity.ResetDaily();
            }

            #endregion



           
        }

    }
}
