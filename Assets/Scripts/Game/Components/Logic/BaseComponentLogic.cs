using System.Collections.Generic;
using EG.Core.Entity;
using EG.Core.Interfaces;


namespace EG
{
    namespace Core.Components
    {

        public abstract class BaseComponentLogic
        {
            protected bool canExecuteComponent = false;
            protected uint entityId = 0;


            public bool CanExecuteComponent => canExecuteComponent;
            public virtual uint GetId => entityId;

            public virtual bool IsBusy => false;
            public virtual bool CanExecute => false;

            public abstract void InitComponent(EntityLogicData aLogicData, List<BaseComponentLogic> aLogicComponents);
            
            public virtual void Update(float aDeltaTime = 0f){}
            
            public virtual void GamePaused(bool aIsPaused){}
            
            public virtual void ResetDaily(){}
            
            public virtual void Reset(){}
            
            public virtual void ResetForPool(){}
            
            public virtual void SetData(params object[] args){}
            
            public virtual void SetData(IWorkData aWorkData){}
            
            public virtual void RefreshData(params object[] args){}
            
            public virtual void UpdateLogicData(EntityLogicData aLogicData){}
            
            public virtual void Execute(){}
            
            public virtual void ExecuteWithResponse(System.Action onExecuted){}

            public virtual void Start()
            {
                canExecuteComponent = true;
            }
            
            public virtual void Start(float aDays,
                float aDelayDays,
                System.Action<float, float> anUpdateProgress,
                System.Action<float, float> anUpdateDelayProgress){}
            
            public virtual void Start(IWorkData aWorkData,
                float aDays,
                float aDelayDays,
                System.Action<float, float> anUpdateProgress,
                System.Action<float, float> anUpdateDelayProgress){}
            
            public virtual void Stop()
            {
                canExecuteComponent = false;
            }
            
            public virtual void Destroy(){}
            
            public virtual void DestroyUnity(){}
            
        }

    }
}
