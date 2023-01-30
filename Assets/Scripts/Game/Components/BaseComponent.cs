using EG.Core.Interfaces;

namespace EG
{
    namespace Core.ComponentsSystem
    {
        //
        //any component will inherit from the base
        //
        public abstract class BaseComponent
        {
            protected bool canExecuteComponent = false;
            protected uint entityId = 0;

            public bool CanExecuteComponent => canExecuteComponent;

            //what entity is tied to this component?
            public uint GetId => entityId;

            //some components can handle data, so they could be "busy" if they implement an internal flag
            public virtual bool IsBusy() => false;
            

            public virtual bool CanExecute() => false;
            

            public abstract void InitComponent(
                uint anEntityId,
                params object[] args);

            //can receive delta time or other value you want to pass as a float
            public virtual void DoUpdate(float value = 1f)
            {
            }

            public virtual void DoFixedUpdate(float value = 1f)
            {
            }

            public virtual void DoLateUpdate(float value = 1f)
            {
            }

            public virtual void GamePaused(bool ispaused)
            {
            }

            public virtual void Reset()
            {
                canExecuteComponent = false;
            }

            public virtual void ResetForPool()
            {
                canExecuteComponent = false;
            }
            
            public virtual void ResetDaily(){}

            //set data to be used by this component
            public virtual void SetData(params object[] args)
            {
            }
            
            public virtual void SetData(IWorkData aWorkData){}

            //you can use it to update vars or assign something to those vars
            public virtual void RefreshData(params object[] args)
            {
            }

            //you can use it to update vars or assign something to those vars
            //mostly to update parameters from attributes
            public virtual void UpdateData()
            {
            }

            public virtual void Execute()
            {
            }

            public virtual void ExecuteWithResponse(System.Action onExecuted)
            {
            }

            public virtual void Start()
            {
                canExecuteComponent = true;
            }
            
            public virtual void Start(float aDays,
                float aDelayDays,
                System.Action<float, float> anUpdateProgress,
                System.Action<float, float> anUpdateDelayProgress,
                System.Action<uint> onComplete){}

            public virtual void Stop()
            {
                canExecuteComponent = false;
            }

            public virtual void Destroy()
            {
            }

            public virtual void DestroyUnity()
            {
            }
        }

    }
}