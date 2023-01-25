using System.Collections.Generic;
using EG.Core.Entity;

namespace EG
{
    namespace Core.Components
    {

        public class IdleComponentLogic : BaseComponentLogic
        {

            private EntityState state = new EntityState();
            private EntityLogicData entityLogicData = null;
            private System.Action<uint> onComponentCompletedAction = null;


            #region component

            public override void InitComponent(EntityLogicData aLogicData, List<BaseComponentLogic> aLogicComponents)
            {
                entityId = aLogicData.GetId;
                entityLogicData = aLogicData;
                
            }

            #endregion


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
            
            
            public override void Start(float aDays,
                float aDelayDays,
                System.Action<float, float> anUpdateProgress,
                System.Action<float, float> anUpdateDelayProgress)
            {
                if (entityLogicData.IsBusy) return;
                
                entityLogicData.SetState(state);
                
                state.Init(aDays, aDelayDays, anUpdateProgress, anUpdateDelayProgress, OnCompleteComponentAction);
            }


            private void OnCompleteComponentAction()
            {
                onComponentCompletedAction?.Invoke(entityId);
            }


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
        }

    }
}
