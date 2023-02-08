using EG.Core.Entity;
using EG.Core.Messages;



namespace EG
{
    namespace Core.ComponentsSystem
    {

        public class FoodComponentLogic : BaseComponent
        {
            
            private EntityLogicData entityLogicData = null;
            private EG_MessageConsumeFood messageConsumeFood = null;
     

            
            #region component

            public override void InitComponent(uint anEntityId, params object[] args)
            {
                messageConsumeFood = new EG_MessageConsumeFood();
                
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
                messageConsumeFood = null;
                entityLogicData?.Destroy();
            }
            
            #endregion


            #region data
            
            public override void UpdateDayData()
            {
                ConsumeFood();
            }

            #endregion

            
            #region food private
            
            private void ConsumeFood()
            {
                if (entityLogicData.GetBuildingTypeId == 0) return;
                
                var foodToConsume = (uint)entityLogicData.GetAttributeValue(Attribute_Enums.AttributeType.FoodAttr);

                messageConsumeFood.SetData(entityLogicData.GetBuildingTypeId, foodToConsume, entityId);

                EG_MessagesController<EG_MessageConsumeFood>.Post(
                    (int)GameEnums.MessageTypes.ConsumeFood,
                    messageConsumeFood);
            }

            #endregion

            
     
           
        }

    }
}
