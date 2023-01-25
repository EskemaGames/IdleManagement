using System.Collections.Generic;
using EG.Core.Entity;
using EG.Core.Interfaces;




namespace EG
{
    namespace Core.Components
    {

        public class WorkComponentLogic : BaseComponentLogic
        {
            
            private EntityState state = new EntityState();
            private EntityLogicData entityLogicData = null;
            private uint currentWorkAmount = 0;
            private uint currentResultAmount = 0;
            private uint currentResultItemAmount = 0;
            private IWorkData workData = null;
            
            private System.Action<uint> onComponentCompletedAction = null;

            public uint GetCurrentWorkAmount => currentWorkAmount;
            public uint GetCurrentResultAmount => currentResultAmount;
            public uint GetCurrentResultItemAmount => currentResultItemAmount;

            public bool IsWorking()
            {
                return workData != null ? true : false;
            }

            
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
                state?.Destroy();
            }
            
            #endregion

            
            #region data

            public override void SetData(params object[] args)
            {
                for (var i = 0; i < args.Length; ++i)
                {
                    if (args[i] is System.Action<uint>)
                    {
                        onComponentCompletedAction = (System.Action<uint>) args[i];
                    }
                }
            }
            
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

                state.OnUpdate(aDeltaTime);
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
                System.Action<float, float> anUpdateDelayProgress)
            {
                if (entityLogicData.IsBusy) return;
                
                if (IsWorking()) return;
                
                entityLogicData.SetState(state);
                
                state.Init(aDays, aDelayDays, anUpdateProgress, anUpdateDelayProgress, OnCompleteComponentAction);
            }
            
            public override void Start(IWorkData aWorkData,
                float aDays,
                float aDelayDays,
                System.Action<float, float> anUpdateProgress,
                System.Action<float, float> anUpdateDelayProgress)
            {
                if (entityLogicData.IsBusy) return;
                
                CheckCurrentWorkToResetIfDataChanged(aWorkData);

                workData = aWorkData;
                
                entityLogicData.SetState(state);
                
                state.Init(aDays, aDelayDays, anUpdateProgress, anUpdateDelayProgress, OnCompleteComponentAction);
            }

            private void OnCompleteComponentAction()
            {
                state.OnCancelUpdate();
                entityLogicData.SetState(state);

                currentResultItemAmount += workData.Item.Amount;
                currentResultAmount += workData.ResultFromWork;
                currentWorkAmount += workData.Item.Amount;
                
                onComponentCompletedAction?.Invoke(entityId);
            }


            public override void ResetDaily()
            {
                if (entityLogicData.IsBusy) return;
                
                workData?.Reset();
            }


            private void CheckCurrentWorkToResetIfDataChanged(IWorkData aData)
            {
                if (workData == null) return;

                if (workData != aData)
                {
                    workData.Reset();
                }
                
            }
        }

    }
}
