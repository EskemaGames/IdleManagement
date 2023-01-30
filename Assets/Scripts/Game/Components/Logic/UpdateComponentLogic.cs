using System.Collections.Generic;
using EG.Core.Entity;

namespace EG
{
    namespace Core.ComponentsSystem
    {

        public class UpdateComponentLogic : BaseComponent
        {
            
            private EntityLogicData entityLogicData = null;
            private System.Action<uint> onComponentCompletedAction = null;


            #region component
            
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

            // public override void InitComponent(EntityLogicData aLogicData, List<BaseComponentLogic> aLogicComponents)
            // {
            //     entityId = aLogicData.GetId;
            //     entityLogicData = aLogicData;
            //     
            // }

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
            }
            
            #endregion


        }

    }
}
