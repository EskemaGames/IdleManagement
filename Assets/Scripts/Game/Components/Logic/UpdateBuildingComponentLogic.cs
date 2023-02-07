using EG.Core.AttributesSystem;
using EG.Core.Entity;
using EG.Core.Interfaces;

namespace EG
{
    namespace Core.ComponentsSystem
    {

        public class UpdateBuildingComponentLogic : BaseComponent
        {
            
            private BuildingLogicData buildingLogicData = null;
            private System.Action<uint> onComponentCompletedAction = null;
            private IWorkData workData = null;
            private EntityState tmpState = null;

            
            #region component
            
            public override void InitComponent(uint anEntityId, params object[] args)
            {
                tmpState = new EntityState();
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
                tmpState?.Destroy();
            }
            
            #endregion


            public override void DoUpdate(float value = 1)
            {
                tmpState?.OnUpdate(value);
            }

            
            public override void SetData(IWorkData aWorkData)
            {
                if (aWorkData.Id != (int) GameEnums.WorkAction.Update) return;
                
                CheckCurrentWorkToResetIfDataChanged(aWorkData);

                workData = aWorkData;
            }
            
            public override void Start(
                uint aDays,
                uint aDelayDays,
                System.Action<float, float> anUpdateProgress,
                System.Action<float, float> anUpdateDelayProgress,
                System.Action<uint> onComplete)
            {
                if (tmpState.IsBusy) return;
                
                if (!GotAWork()) return;

                onComponentCompletedAction = onComplete;
                
                uint timeNeeded = (uint)buildingLogicData.GetAttributeValue(Attribute_Enums.AttributeType.UpdateTimeAttr);      
                workData.SetWorkTime(timeNeeded);
                
                //let's check if this update cost money or if it's free
                uint cost = (uint)buildingLogicData.GetAttributeValue(Attribute_Enums.AttributeType.UpdateCostAttr);
                if (cost > 0f)
                {
                    //the cost of the update will be adjusted immediately, assuming this entity have a payments component
                    PaymentsBuildingComponentLogic paymentsBuildingComponent = buildingLogicData.GetLogicComponent<PaymentsBuildingComponentLogic>();

                    if (paymentsBuildingComponent != null)
                    {
                        paymentsBuildingComponent.SetTotalCosts(cost);
                    }
                }

                tmpState.Init(timeNeeded, aDelayDays, anUpdateProgress, anUpdateDelayProgress, OnCompleteUpdateComponent);
            }
            


            private void OnCompleteUpdateComponent()
            {
                tmpState.OnCancelUpdate();
   
                for (int i = 0, max = buildingLogicData.GetUpdateableAttributes().Count; i < max; ++i)
                {
                    BaseAttribute attribute = buildingLogicData.GetUpdateableAttributes()[i];

                    //get the update of the building
                    //some attributes will be only 1 level for the whole time
                    var levelValue = 0;
                    if (attribute.UpdateLevels.Count > 1)
                    {
                        levelValue = (int) buildingLogicData.GetAttributeValue(Attribute_Enums.AttributeType.UpdateBuildingLevelAttr) - 1;
                    }

                    Modifier modifier = AttributesAndModifiersController.CreateModifier(
                        IdGenerator.GetNewUId(),
                        buildingLogicData.GetId,
                        -1,
                        false,
                        Attribute_Enums.AttributeFormulas.Addition,
                        attribute.UpdateLevels[levelValue].IncreaseStep,
                        attribute.AttributeType,
                        false);
                    buildingLogicData.AddModifier(modifier);
                }
                
                onComponentCompletedAction?.Invoke(entityId);

                workData = null;
            }
            
            

            
            
            #region misc private
            
            private void CheckCurrentWorkToResetIfDataChanged(IWorkData aData)
            {
                if (workData == null) return;

                if (workData != aData)
                {
                    workData.Reset();
                }

                workData = null;
            }
            
            private bool GotAWork()
            {
                return workData != null;
            }
            
            #endregion


        }

    }
}
