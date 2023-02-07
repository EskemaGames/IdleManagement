using EG.Core.AttributesSystem;
using EG.Core.ComponentsSystem;
using EG.Core.Interfaces;
using EG.Core.Messages;



namespace EG
{
    namespace Core.Entity
    {
        public class EntityLogicData : IGameTime
        {
            
            private EntityData entityData = new EntityData();
            private IEntityView entityView = null;
            private uint buildingTypeId = 0;
            private AttributesAndModifiersController attributesAndModifiersController = new AttributesAndModifiersController();
            private EG_MessageDeadByAge messageDeadByAge = new EG_MessageDeadByAge();


            #region constructor

            public EntityLogicData(){}

            public EntityLogicData(EntityLogicData aLogicData)
            {
                entityData = aLogicData.entityData;
                entityView = aLogicData.entityView;
                attributesAndModifiersController = aLogicData.attributesAndModifiersController;
                messageDeadByAge = aLogicData.messageDeadByAge;
            }

            public EntityLogicData(EntityData anEntityData)
            {
                entityData = anEntityData;

                attributesAndModifiersController.AddAttributes(entityData.GetAttributes);
                
                for (var i = 0; i < entityData.GetComponents.Count; ++i)
                {
                    BaseComponent component = entityData.GetComponents[i];
                    component.InitComponent(anEntityData.GetUniqueId, this);
                    component.Start();
                }
            }

            public EntityLogicData Clone()
            {
                return new EntityLogicData(this);
            }

            #endregion

            
            #region init and destroy

            public void Destroy()
            {
                entityData?.Destroy();
                entityView = null;
                
                attributesAndModifiersController?.Destroy();
            }
            
            #endregion


            #region public API

            public void SetBuildingTypeId(uint aBuildingTypeId)
            {
                buildingTypeId = aBuildingTypeId;
            }
            
            public void SetEntityView(IEntityView aView)
            {
                entityView = aView;
            }

            public uint GetBuildingTypeId => buildingTypeId;
            
            public EntityState GetState => entityData.GetCurrentState;

            public bool IsBusy => entityData.GetCurrentState.IsBusy;

            public GameEnums.EntityType GetNameId => entityData.GetNameId;

            public GameEnums.GroupTypes GetGroupId => entityData.GetGroupType;

            public T GetLogicComponent<T>() where T : BaseComponent
            {
                return entityData.GetComponent<T>();
            }

            public uint GetId => entityData.GetUniqueId;

            public float GetAttributeValue(Attribute_Enums.AttributeType anAttributeType) => attributesAndModifiersController.GetAttributeValue(anAttributeType);
            public float GetAttributeMaxValue(Attribute_Enums.AttributeType anAttributeType) => attributesAndModifiersController.GetAttributeBaseMaxValue(anAttributeType);

            public void ResetDaily()
            {
                for (int i = 0, max = entityData.GetComponents.Count; i < max; ++i)
                {
                    entityData.GetComponents[i].ResetDaily();
                }
            }

            public void AddModifier(Modifier aModifier)
            {
                attributesAndModifiersController.AddModifier(aModifier);
            }

            public void RemoveAllModifiersWithType(Attribute_Enums.AttributeType aType)
            {
                attributesAndModifiersController.RemoveAllModifiersWithType(aType);
            }
            
            public void RemoveModifierWithType(Attribute_Enums.AttributeType aType)
            {
                attributesAndModifiersController.RemoveModifier(aType);
            }
            
            public void RemoveModifierWithId(uint anId)
            {
                attributesAndModifiersController.RemoveModifier(anId);
            }
            
            #endregion


            #region update

            public void OnUpdate(float aDeltaTime)
            {
                for (var i = 0; i < entityData.GetComponents.Count; ++i)
                {
                    BaseComponent component = entityData.GetComponents[i];
                    component.DoUpdate(aDeltaTime);
                }
            }
            
            #endregion
          
            
            
            #region interface game time

            public void IOnYearPassed(uint aYear)
            {
                if (CheckForDeath()) return;
                
                for (var i = 0; i < entityData.GetComponents.Count; ++i)
                {
                    BaseComponent component = entityData.GetComponents[i];
                    component.UpdateYearData();
                }
                
            }

            public void IOnDayPassed(uint aDay)
            {
                for (var i = 0; i < entityData.GetComponents.Count; ++i)
                {
                    BaseComponent component = entityData.GetComponents[i];
                    component.UpdateDayData();
                }
            }
            
            #endregion


            
            private bool CheckForDeath()
            {
                Modifier modifier = AttributesAndModifiersController.CreateModifier(
                    IdGenerator.GetNewUId(),
                    entityData.GetUniqueId,
                    -1,
                    false,
                    Attribute_Enums.AttributeFormulas.Addition,
                    1,
                    Attribute_Enums.AttributeType.AgeAttr,
                    false);
                
                attributesAndModifiersController.AddModifier(modifier);

                var age = attributesAndModifiersController.GetAttributeValue(Attribute_Enums.AttributeType.AgeAttr);

                if (age > attributesAndModifiersController.GetAttributeBaseMaxValue(Attribute_Enums.AttributeType.AgeAttr))
                {
                    messageDeadByAge.SetData(entityData.GetUniqueId, (uint)entityData.GetNameId);
                    
                    EG_MessagesController<EG_MessageDeadByAge>.Post(
                        (int)GameEnums.MessageTypes.DeadByAge,
                        messageDeadByAge);
                    
                    Destroy();

                    return true;
                }

                return false;
            }
            
            
            
        }

    }
}


