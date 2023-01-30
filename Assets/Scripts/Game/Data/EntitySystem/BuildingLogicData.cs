using System.Collections.Generic;
using EG.Core.AttributesSystem;
using EG.Core.ComponentsSystem;
using EG.Core.Interfaces;
using EG.Core.Messages;
using UnityEngine;


namespace EG
{
    namespace Core.Entity
    {

        public class BuildingLogicData : IGameTime
        {

            private EntityData entityData = new EntityData();
            private List<BaseComponent> logicComponents = new List<BaseComponent>(5);
            private List<EntityLogicData> entitiesLogicData = new List<EntityLogicData>(100);
            private AttributesAndModifiersController attributesAndModifiersController = new AttributesAndModifiersController();

            private int totalEntities = 0;
            private int totalDaysPassed = 0;
            private int totalDaysPassedPayments = 0;
            private readonly int paymentDay = 0;
            

            #region constructor

            public BuildingLogicData(){}

            public BuildingLogicData(BuildingLogicData aLogicData)
            {
                entityData = aLogicData.entityData;
                logicComponents = aLogicData.logicComponents;
                entitiesLogicData = aLogicData.entitiesLogicData;
                attributesAndModifiersController = aLogicData.attributesAndModifiersController; 
            }

            public BuildingLogicData(EntityData anEntityData)
            {
                entityData = anEntityData;

                attributesAndModifiersController.AddAttributes(entityData.GetAttributes);

                paymentDay = (int)attributesAndModifiersController.GetAttributeValue(Attribute_Enums.AttributeType.PaymentDayAttr);
                
                logicComponents =  new List<BaseComponent>(anEntityData.GetComponents);
                
                for (var i = 0; i < logicComponents.Count; ++i)
                {
                    BaseComponent component = logicComponents[i];
                    component.InitComponent(anEntityData.GetUniqueId, this, logicComponents);
                    component.Start();
                }
            }

            public BuildingLogicData Clone()
            {
                return new BuildingLogicData(this);
            }

            #endregion

            
            #region init and destroy

            public void Init()
            {
                EG_MessagesController<EG_MessageDeadByAge>.AddObserver(
                    (int)GameEnums.MessageTypes.DeadByAge,
                    OnDeadByAge);
            }

            public void Destroy()
            {
                EG_MessagesController<EG_MessageDeadByAge>.RemoveObserver(
                    (int)GameEnums.MessageTypes.DeadByAge,
                    OnDeadByAge);
                
                entityData?.Destroy();
         
                for (var i = logicComponents.Count-1; i > -1; --i)
                {
                    logicComponents[i].Destroy();
                }

                for (var i = entitiesLogicData.Count-1; i > -1; --i)
                {
                    entitiesLogicData[i].Destroy();
                }
                
            }
            
            #endregion

            
            #region public entity API

            public bool IsBusy()
            {
                return entityData.GetCurrentState?.IsBusy ?? false;
            }

            public uint GetId => entityData.GetUniqueId;
            
            public GameEnums.EntityType GetNameId => entityData.GetNameId;

            public EntityLogicData GetEntity(uint anId, out int anArrayPosition)
            {
                for (var i = 0; i < entitiesLogicData.Count; ++i)
                {
                    EntityLogicData entity = entitiesLogicData[i];
                    
                    if (entity.GetId != anId) continue;

                    anArrayPosition = i;
                    return entity;
                }
                
                anArrayPosition = -1;
                return null;
            }
            
            public EntityLogicData GetEntity(uint anId)
            {
                for (var i = 0; i < entitiesLogicData.Count; ++i)
                {
                    EntityLogicData entity = entitiesLogicData[i];
                    
                    if (entity.GetId != anId) continue;

                    return entity;
                }

                return null;
            }

            public EntityLogicData GetEntityByPosition(int anArrayPosition)
            {
                return entitiesLogicData[anArrayPosition];
            }

            public void RemoveEntity(int anArrayPosition)
            {
                if (anArrayPosition == -1) return;

                entitiesLogicData.RemoveAt(anArrayPosition);
            }

            public int GetTotalEntities => totalEntities;
            
            #endregion


            #region building API
            
            public T GetLogicComponent<T>() where T : BaseComponent
            {
                for (int i = 0, max = logicComponents.Count; i < max; ++i)
                {
                    BaseComponent component = logicComponents[i];
                    if (component is T)
                    {
                        return component as T;
                    }
                }

                return null;
            }
            
            public List<BaseAttribute> GetUpdateableAttributes() => attributesAndModifiersController.GetAllUpdateableAttributes();

            public float GetAttributeValue(Attribute_Enums.AttributeType anAttributeType) => attributesAndModifiersController.GetAttributeValue(anAttributeType);

            public int GetBuildingCapacity() => (int)attributesAndModifiersController.GetAttributeValue(Attribute_Enums.AttributeType.CapacityBuildingAttr);
            
            public int GetBuildingMaxCapacity() => (int)attributesAndModifiersController.GetAttributeBaseMaxValue(Attribute_Enums.AttributeType.CapacityBuildingAttr);

            public int GetBuildingLevel() => (int)attributesAndModifiersController.GetAttributeValue(Attribute_Enums.AttributeType.UpdateBuildingAttr);
            
            public int GetBuildingMaxLevel() => (int)attributesAndModifiersController.GetAttributeBaseMaxValue(Attribute_Enums.AttributeType.UpdateBuildingAttr);

            public void AddModifier(Modifier aModifier)
            {
                attributesAndModifiersController.AddModifier(aModifier);
            }
            
            public bool AddToBuilding(EntityLogicData anEntityLogicData)
            {
                if (GetBuildingCapacity() >= GetBuildingMaxCapacity()) return false;
                
                Modifier modifier = AttributesAndModifiersController.CreateModifier(
                    IdGenerator.GetNewUId(),
                    entityData.GetUniqueId,
                    -1,
                    false,
                    Attribute_Enums.AttributeFormulas.Addition,
                    1,
                    Attribute_Enums.AttributeType.CapacityBuildingAttr,
                    false);
                
                attributesAndModifiersController.AddModifier(modifier);
                
                entitiesLogicData.Add(anEntityLogicData);

                totalEntities = entitiesLogicData.Count;
                
                Debug.Log("Entity added= " + anEntityLogicData.GetEntityNameId + " to building= " + entityData.GetNameId);
                return true;
            }

            public void RemoveFromBuilding(int anArrayPosition)
            {
                 Modifier modifier = AttributesAndModifiersController.CreateModifier(
                     IdGenerator.GetNewUId(),
                     entityData.GetUniqueId,
                     -1,
                     false,
                     Attribute_Enums.AttributeFormulas.Addition,
                     -1,
                     Attribute_Enums.AttributeType.CapacityBuildingAttr,
                     false);
                 
                attributesAndModifiersController.AddModifier(modifier);
                
                entitiesLogicData.RemoveAt(anArrayPosition);
                
                totalEntities = entitiesLogicData.Count;
            }
            
            #endregion
            
            
            #region using building API

            public void Start(IWorkData aWorkData,
                float aDays,
                float aDelayDays,
                System.Action<float, float> anUpdateProgress,
                System.Action<float, float> anUpdateDelayProgress,
                System.Action<uint> onCompleteEntity)
            {
                for (var i = 0; i < logicComponents.Count; ++i)
                {
                    BaseComponent component = logicComponents[i];
                    
                    component.SetData(aWorkData);
                    
                    component.Start(
                        aDays,
                        aDelayDays,
                        anUpdateProgress,
                        anUpdateDelayProgress,
                        onCompleteEntity);
                }
            }

            #endregion


            #region update

            public void OnUpdate(float aDeltaTime)
            {
                for (var i = 0; i < entitiesLogicData.Count; ++i)
                {
                    EntityLogicData entity = entitiesLogicData[i];
                    entity.OnUpdate(aDeltaTime);
                }
            }
            
            #endregion
            
            
            #region messages

            private void OnDeadByAge(EG_MessageDeadByAge message)
            {
                for (var i = 0; i < entitiesLogicData.Count; ++i)
                {
                    if (entitiesLogicData[i].GetId != message.EntityId) continue;
            
                    RemoveFromBuilding(i);
                    break;
                }
            }
    
            #endregion
            
            
            #region interface game time

            public virtual void IOnYearPassed(uint aYear)
            {
                Debug.Log("On year passed on building= " + entityData.GetNameId);
            }

            public virtual void IOnDayPassed(uint aDay)
            {

                totalDaysPassed++;
                
                totalDaysPassedPayments++;
                
                Debug.Log("On day passed on building= " + entityData.GetNameId + " day= " + aDay + " total days passed= " + totalDaysPassed);

                CheckMonthlyPayments();
            }

            #endregion


            private void CheckMonthlyPayments()
            {
                if (GetAttributeValue(Attribute_Enums.AttributeType.PaymentDayAttr) != 0f) return;

                if (totalDaysPassedPayments >= paymentDay)
                {
                    totalDaysPassedPayments = 0;

                    PaymentsBuildingComponentLogic paymentsBuildingComponent = GetLogicComponent<PaymentsBuildingComponentLogic>();
                    
                    paymentsBuildingComponent?.SetTotalPayments(0);

                }
            }



            
        }

    }
}
