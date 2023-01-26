using System.Collections.Generic;
using EG.Core.Entity;
using EG.Core.Interfaces;

namespace EG
{
    namespace Core.Components
    {

        public class FarmComponentLogic : BaseComponentLogic
        {
            
            private BuildingLogicData buildingLogicData = null;
            private System.Action<uint> onComponentCompletedAction = null;
            private IWorkData workData = null;

            
            #region component

            public override void InitComponent(EntityLogicData aLogicData, List<BaseComponentLogic> aLogicComponents) { }

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
            
            public override void SetData(params object[] args)
            {
                for (var i = 0; i < args.Length; ++i)
                {
                    if (args[i] is BuildingLogicData)
                    {
                        buildingLogicData = (BuildingLogicData) args[i];
                    }
                }
            }
            
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
                
                PaymentsComponentLogic paymentsComponent = entity.GetLogicComponent<PaymentsComponentLogic>();

                if (paymentsComponent != null)
                {
                    uint cost = (uint)entity.GetAttributeValue(Attribute_Enums.AttributeType.SalaryAttr);
                    
                    WorkComponentLogic component = entity.GetLogicComponent<WorkComponentLogic>();
                    
                    cost *= component.GetCurrentTimeToWorkAmount;
                    
                    paymentsComponent.SetTotalPayments(cost);
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
                for (int i = 0, max = aEntity.GetComponentsLogic.Count; i < max; ++i)
                {
                    aEntity.GetComponentsLogic[i].ResetDaily();
                }
            }
            
            #endregion
            

        }

    }
}
