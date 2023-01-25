using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EG.Core.AttributesSystem;
using EG.Core.Components;
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
            private List<BaseComponentLogic> logicComponents = new List<BaseComponentLogic>();
            private AttributesAndModifiersController attributesAndModifiersController = new AttributesAndModifiersController();
            private EG_MessageDeadByAge messageDeadByAge = new EG_MessageDeadByAge();
            public ReadOnlyCollection<BaseComponentLogic> GetComponentsLogic => logicComponents.AsReadOnly();
            

            #region constructor

            public EntityLogicData(){}

            public EntityLogicData(EntityLogicData aLogicData)
            {
                entityData = aLogicData.entityData;
                entityView = aLogicData.entityView;
                logicComponents = aLogicData.logicComponents;
                attributesAndModifiersController = aLogicData.attributesAndModifiersController;
                messageDeadByAge = aLogicData.messageDeadByAge;
            }

            public EntityLogicData(EntityData anEntityData)
            {
                entityData = anEntityData;

                attributesAndModifiersController.AddAttributes(entityData.GetAttributes);
                
                logicComponents =  new List<BaseComponentLogic>(anEntityData.GetComponents);
                
                for (var i = 0; i < logicComponents.Count; ++i)
                {
                    BaseComponentLogic component = logicComponents[i];
                    component.InitComponent(this, logicComponents);
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

                for (var i = logicComponents.Count-1; i > -1; --i)
                {
                    logicComponents[i].Destroy();
                }
                
                attributesAndModifiersController?.Destroy();
            }
            
            #endregion


            #region public API
            
            public void SetEntityView(IEntityView aView)
            {
                entityView = aView;
            }

            public void SetState(EntityState aState)
            {
                entityData.SetEntityState(aState);
            }

            public bool IsBusy => entityData.GetCurrentState?.IsBusy ?? false;

            public GameEnums.EntityType GetEntityNameId => entityData.GetNameId;

            public GameEnums.GroupTypes GetGroupId => entityData.GetGroupType;

            public BaseComponentLogic GetComponent(Type aComponentType)
            {
                for (var i = 0; i < logicComponents.Count; ++i)
                {
                    BaseComponentLogic component = logicComponents[i];
                    if (component.GetType() == aComponentType)
                    {
                        return component;
                    }
                }

                return null;
            }

            public uint GetId => entityData.GetUniqueId;

            public float GetAttributeValue(Attribute_Enums.AttributeType anAttributeType) => attributesAndModifiersController.GetAttributeValue(anAttributeType);
            
            #endregion


            #region update

            public void OnUpdate(float aDeltaTime)
            {
                for (var i = 0; i < logicComponents.Count; ++i)
                {
                    BaseComponentLogic component = logicComponents[i];
                    component.Update(aDeltaTime);
                }
            }
            
            #endregion
          
            
            
            #region interface game time

            public void IOnYearPassed(uint aYear)
            {
                if (CheckForDeath()) return;
                
                //
                //check para la paguita
                //check si eres presidente del gobierno
                //
                
            }

            public void IOnDayPassed(uint aDay)
            {
                
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
                    messageDeadByAge.SetData(entityData.GetUniqueId);
                    
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


