using System;
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
            private Dictionary<uint, List<EntityLogicData>> entitiesLogicData = new Dictionary<uint, List<EntityLogicData>>();
            private AttributesAndModifiersController attributesAndModifiersController = new AttributesAndModifiersController();
            private uint needsUpdate = 0;


            #region constructor

            public BuildingLogicData(){}

            public BuildingLogicData(BuildingLogicData aLogicData)
            {
                entityData = aLogicData.entityData;
                entitiesLogicData = aLogicData.entitiesLogicData;
                attributesAndModifiersController = aLogicData.attributesAndModifiersController; 
            }

            public BuildingLogicData(EntityData anEntityData)
            {
                entityData = anEntityData;
                
                entityData.GetCurrentState.OnCancelUpdate();

                attributesAndModifiersController.AddAttributes(entityData.GetAttributes);
                
                for (var i = 0; i < entityData.GetComponents.Count; ++i)
                {
                    BaseComponent component = entityData.GetComponents[i];
                    component.InitComponent(anEntityData.GetUniqueId, this, entityData.GetComponents);
                    component.Start();
                }
                
                needsUpdate = (uint)attributesAndModifiersController.GetAttributeValue(Attribute_Enums.AttributeType.NeedsUpdateAttr);
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
                
                foreach (var tmpEntityList in entitiesLogicData)
                {
                    var list = tmpEntityList.Value;
                    
                    for (var i = list.Count-1; i > -1; --i)
                    {
                        var entity = list[i];
                        entity?.Destroy();
                    }
                }
                
            }
            
            #endregion

            
            #region public entity API

            public bool IsBusy => entityData.GetCurrentState.IsBusy;

            public EntityState GetState => entityData.GetCurrentState;
            
            public uint GetId => entityData.GetUniqueId;
            
            public GameEnums.EntityType GetNameId => entityData.GetNameId;

            public EntityLogicData GetEntity(uint aCategoryId, uint anId, out int anArrayPosition)
            {
                var list = entitiesLogicData[aCategoryId];
                
                for (var i = 0; i < list.Count; ++i)
                {
                    EntityLogicData entity = list[i];
                    
                    if (entity.GetId != anId) continue;

                    anArrayPosition = i;
                    return entity;
                }
                
                anArrayPosition = -1;
                return null;
            }

            public EntityLogicData GetEntity(uint aCategoryId, uint anId)
            {
                var list = entitiesLogicData[aCategoryId];
                
                for (var i = 0; i < list.Count; ++i)
                {
                    EntityLogicData entity = list[i];
                    
                    if (entity.GetId != anId) continue;

                    return entity;
                }
                
                return null;
            }
            
            public EntityLogicData GetEntityByPosition(uint aCategoryId, int anArrayPosition)
            {
                var list = entitiesLogicData[aCategoryId];

                return list[anArrayPosition];
            }
            
            public void RemoveEntity(uint aCategoryId, int anArrayPosition)
            {
                if (anArrayPosition == -1) return;

                var list = entitiesLogicData[aCategoryId];
                
                list.RemoveAt(anArrayPosition);
            }
            
            public int GetTotalEntities => entitiesLogicData[(uint)entityData.GetNameId].Count;
            
            #endregion


            #region building API
            
            public T GetLogicComponent<T>() where T : BaseComponent
            {
                return entityData.GetComponent<T>();
            }
            
            public List<BaseAttribute> GetUpdateableAttributes() => attributesAndModifiersController.GetAllUpdateableAttributes();

            public float GetAttributeValue(Attribute_Enums.AttributeType anAttributeType) => attributesAndModifiersController.GetAttributeValue(anAttributeType);
            public float GetAttributeMaxValue(Attribute_Enums.AttributeType anAttributeType) => attributesAndModifiersController.GetAttributeBaseMaxValue(anAttributeType);

            public int GetBuildingCapacity() => (int)attributesAndModifiersController.GetAttributeValue(Attribute_Enums.AttributeType.CapacityBuildingAttr);
            
            public int GetBuildingMaxCapacity() => (int)attributesAndModifiersController.GetAttributeBaseMaxValue(Attribute_Enums.AttributeType.CapacityBuildingAttr);

            public int GetBuildingLevel() => (int)attributesAndModifiersController.GetAttributeValue(Attribute_Enums.AttributeType.UpdateBuildingLevelAttr);
            
            public int GetBuildingMaxLevel() => (int)attributesAndModifiersController.GetAttributeBaseMaxValue(Attribute_Enums.AttributeType.UpdateBuildingLevelAttr);
            
            public void AddModifier(Modifier aModifier)
            {
                attributesAndModifiersController.AddModifier(aModifier);
            }

            public bool AddToBuilding(uint aCategoryId, EntityLogicData anEntityLogicData)
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

                try
                {
                    var list = entitiesLogicData[aCategoryId];
                    
                    list.Add(anEntityLogicData);
                    entitiesLogicData[aCategoryId] = list;
                }
                catch (Exception e)
                {
                    var tmpList = new List<EntityLogicData>();
                    tmpList.Add(anEntityLogicData);
                    entitiesLogicData.Add(aCategoryId, new List<EntityLogicData>(tmpList));
                }
                
                anEntityLogicData.SetBuildingTypeId((uint)GetNameId);

                return true;
            }
            
            public void RemoveFromBuilding(uint aCategoryId, int anArrayPosition)
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
                
                try
                {
                    var list = entitiesLogicData[aCategoryId];
                    
                    list.RemoveAt(anArrayPosition);
                    
                    entitiesLogicData[aCategoryId] = list;
                }
                catch (Exception e)
                {
                    entitiesLogicData.Remove(aCategoryId);
                }
            }

            #endregion
            
            
            #region using building API

            public void Start(IWorkData aWorkData,
                uint aDelayDays,
                System.Action<float, float> anUpdateProgress,
                System.Action<float, float> anUpdateDelayProgress,
                System.Action<uint> onCompleteEntity)
            {
                for (var i = 0; i < entityData.GetComponents.Count; ++i)
                {
                    BaseComponent component = entityData.GetComponents[i];
                    
                    component.SetData(aWorkData);
                    
                    component.Start(
                        0, 
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
                if (needsUpdate < 1) return;
                
                //now update the components if any implements that update
                for (var i = 0; i < entityData.GetComponents.Count; ++i)
                {
                    BaseComponent component = entityData.GetComponents[i];
                    component.DoUpdate(aDeltaTime);
                }
                
                foreach (var tmpList in entitiesLogicData)
                {
                    var list = tmpList.Value;
                    
                    //first update all our people within the building
                    for (var i = 0; i < list.Count; ++i)
                    {
                        EntityLogicData entity = list[i];
                        entity.OnUpdate(aDeltaTime);
                    }
                }
            }
            
            #endregion
            
            
            #region messages

            private void OnDeadByAge(EG_MessageDeadByAge message)
            {
                var list = entitiesLogicData[message.CategoryId];
                
                for (var i = 0; i < list.Count; ++i)
                {
                    if (list[i].GetId != message.EntityId) continue;
            
                    RemoveFromBuilding(message.CategoryId, i);
                    break;
                }
            }
    
            #endregion
            
            
            #region interface game time

            public void IOnYearPassed(uint aYear)
            {
                if (needsUpdate < 1) return;
                
                Debug.Log("\n");
                Debug.Log("On year passed on building= " + entityData.GetNameId);
                
                for (var i = 0; i < entityData.GetComponents.Count; ++i)
                {
                    BaseComponent component = entityData.GetComponents[i];
                    component.UpdateYearData();
                }
                
                Debug.Log("\n");
            }

            public void IOnDayPassed(uint aDay)
            {
                if (needsUpdate < 1) return;
                
                Debug.Log("\n");
                Debug.Log("On day passed on building= " + entityData.GetNameId + " day= " + aDay);
        
                for (var i = 0; i < entityData.GetComponents.Count; ++i)
                {
                    BaseComponent component = entityData.GetComponents[i];
                    component.UpdateDayData();
                }
                
                Debug.Log("\n");
            }

            #endregion





            
        }

    }
}
