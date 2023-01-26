using System.Collections.Generic;
using EG.Core.Entity;
using EG.Core.Interfaces;




namespace EG
{
    namespace Core.Components
    {

        public class WorkComponentLogic : BaseComponentLogic
        {
            

            private EntityLogicData entityLogicData = null;
            private uint currentTimeToWorkAmount = 0;
            private uint currentResultAmount = 0;
            private uint currentResultItemAmount = 0;
            private IWorkData workData = null;
            
            private System.Action<uint> onComponentCompletedAction = null;

            public uint GetCurrentTimeToWorkAmount => currentTimeToWorkAmount;
            public uint GetCurrentResultAmount => currentResultAmount;
            public uint GetCurrentResultItemAmount => currentResultItemAmount;

            
  

            
            #region init component

            public override void InitComponent(EntityLogicData aLogicData, List<BaseComponentLogic> aLogicComponents)
            {
                entityId = aLogicData.GetId;
                entityLogicData = aLogicData;
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
                entityLogicData?.Destroy();
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
            
            
            #region update

            public override void Update(float aDeltaTime = 0)
            {
                if (!canExecuteComponent) return;
                
                entityLogicData?.GetState.OnUpdate(aDeltaTime);
            }

            #endregion
            

    
            public override void GamePaused(bool aIsPaused)
            {
                base.GamePaused(aIsPaused);
                canExecuteComponent = !aIsPaused;
            }

    
            public override void Start(float aDays,
                float aDelayDays,
                System.Action<float, float> anUpdateProgress,
                System.Action<float, float> anUpdateDelayProgress,
                System.Action<uint> onComplete)
            {
                if (entityLogicData.IsBusy) return;
                
                if (IsWorking()) return;

                onComponentCompletedAction = onComplete;
                
                entityLogicData.GetState.Init(aDays, aDelayDays, anUpdateProgress, anUpdateDelayProgress, OnCompleteComponentAction);
            }

            private void OnCompleteComponentAction()
            {
                entityLogicData.GetState.OnCancelUpdate();
   
                currentTimeToWorkAmount += workData.TimeToWorkAmount;
                currentResultAmount += workData.ResultFromWork;
                currentResultItemAmount += workData.Item.Amount;
                
                onComponentCompletedAction?.Invoke(entityId);
            }


            public override void ResetDaily()
            {
                if (entityLogicData.IsBusy) return;
                
                workData?.Reset();
            }


            
            #region private misc 
            
            private void CheckCurrentWorkToResetIfDataChanged(IWorkData aData)
            {
                if (workData == null) return;

                if (workData != aData)
                {
                    workData.Reset();
                }
            }
            
            private bool IsWorking()
            {
                return workData != null;
            }
            
            #endregion
             
        }

    }
}
