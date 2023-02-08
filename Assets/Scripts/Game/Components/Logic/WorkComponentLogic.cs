using EG.Core.Entity;
using EG.Core.Interfaces;


namespace EG
{
    namespace Core.ComponentsSystem
    {

        public class WorkComponentLogic : BaseComponent
        {
            

            private EntityLogicData entityLogicData = null;
            private uint currentTimeToWorkAmount = 0;
            private uint currentResultAmount = 0;
            private uint currentResultItemAmount = 0;
            private uint currentCost = 0;
            private IWorkData workData = null;
            
            private System.Action<uint> onComponentCompletedAction = null;

            public uint GetCurrentTimeToWorkAmount => currentTimeToWorkAmount;
            public uint GetCurrentResultAmount => currentResultAmount;
            public uint GetCurrentResultItemAmount => currentResultItemAmount;
            public uint GetCurrentCost => currentCost;

            
            #region init component
            
            public override void InitComponent(uint anEntityId, params object[] args)
            {
                entityId = anEntityId;

                for (var i = 0; i < args.Length; ++i)
                {
                    if (args[i] is EntityLogicData)
                    {
                        entityLogicData = (EntityLogicData) args[i];
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
                entityLogicData?.Destroy();
                workData?.Reset();
            }
            
            #endregion
            
            
            #region update

            public override void DoUpdate(float aDeltaTime = 1f)
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

            
            #region data
            
            public override void SetData(IWorkData aWorkData)
            {
                CheckCurrentWorkToResetIfDataChanged(aWorkData);

                workData = aWorkData.Clone();
            }

            #endregion
            
    
            public override void Start(uint aDays,
                uint aDelayDays,
                System.Action<float, float> anUpdateProgress,
                System.Action<float, float> anUpdateDelayProgress,
                System.Action<uint> onComplete)
            {
                if (entityLogicData.IsBusy) return;
                
                if (!GotAWork()) return;
                
                onComponentCompletedAction = onComplete;
                
                entityLogicData.GetState.Init(aDays, aDelayDays, anUpdateProgress, anUpdateDelayProgress, OnCompleteComponentAction);
            }

            private void OnCompleteComponentAction()
            {
                entityLogicData.GetState.OnCancelUpdate();
   
                currentTimeToWorkAmount += workData.TimeToWorkAmount;
                currentResultAmount += workData.ResultFromWork;
                currentResultItemAmount += workData.Item.Amount;
                currentCost += workData.Item.Cost;
                
                onComponentCompletedAction?.Invoke(entityId);

                workData = null;
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
