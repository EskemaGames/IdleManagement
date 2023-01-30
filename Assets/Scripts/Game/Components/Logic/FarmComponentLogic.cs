using EG.Core.Entity;
using EG.Core.Interfaces;


namespace EG
{
    namespace Core.ComponentsSystem
    {

        public class FarmComponentLogic : BaseComponent
        {
            
            private BuildingLogicData buildingLogicData = null;
            private System.Action<uint> onComponentCompletedAction = null;
            private IWorkData workData = null;

            
            #region component

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
            }
            
            #endregion


            #region data

            public override void SetData(IWorkData aWorkData)
            {
                CheckCurrentWorkToResetIfDataChanged(aWorkData);

                workData = aWorkData;
            }

            #endregion
            

            public override void Start(
                float aDays,
                float aDelayDays,
                System.Action<float, float> anUpdateProgress,
                System.Action<float, float> anUpdateDelayProgress,
                System.Action<uint> onComplete)
            {
                onComponentCompletedAction = onComplete;
                
                for (var i = 0; i < buildingLogicData.GetTotalEntities; ++i)
                {
                    EntityLogicData entity = buildingLogicData.GetEntityByPosition(i);
                    
                    WorkComponentLogic component = entity.GetLogicComponent<WorkComponentLogic>();
                    
                    component?.SetData(workData);
                    
                    component?.Start(
                        aDays,
                        aDelayDays,
                        anUpdateProgress,
                        anUpdateDelayProgress,
                        OnCompleteComponentAction);
                }
            }


            private void OnCompleteComponentAction(uint anEntityId)
            {
                var entity = buildingLogicData.GetEntity(anEntityId);

                if (entity == null)
                {
                    onComponentCompletedAction?.Invoke(anEntityId);
                    return;
                }
                
                PaymentsBuildingComponentLogic paymentsBuildingComponent = buildingLogicData.GetLogicComponent<PaymentsBuildingComponentLogic>();

                if (paymentsBuildingComponent != null)
                {
                    uint cost = (uint)entity.GetAttributeValue(Attribute_Enums.AttributeType.SalaryAttr);
                    
                    WorkComponentLogic component = entity.GetLogicComponent<WorkComponentLogic>();
                    
                    cost *= component.GetCurrentTimeToWorkAmount;
                    
                    paymentsBuildingComponent.SetTotalPayments(cost);
                }
                
                onComponentCompletedAction?.Invoke(anEntityId);
                
                ResetWorks(entity);
            }

            
            
            #region misc private
            
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
