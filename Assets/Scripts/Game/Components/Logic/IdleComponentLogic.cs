using System.Collections.Generic;
using EG.Core.Entity;

namespace EG
{
    namespace Core.Components
    {

        public class IdleComponentLogic : BaseComponentLogic
        {
            
            private EntityLogicData entityLogicData = null;
            private System.Action<uint> onComponentCompletedAction = null;


            #region component

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
            }
            
            #endregion


        }

    }
}
