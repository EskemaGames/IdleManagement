using System.Collections.Generic;
using EG.Core.AttributesSystem;
using EG.Core.ComponentsSystem;
using UnityEngine;

namespace EG
{
    namespace Core.Entity
    {
        public class EntityData
        {
            [SerializeField] private GameEnums.GroupTypes groupType = GameEnums.GroupTypes.Max;
            
            //name of this object for grouping objects into different "types"
            [SerializeField] private GameEnums.EntityType nameId = GameEnums.EntityType.Max;
            
            //value to parse from different json data files
            //store the class for later instantiate dynamically from other places
            [SerializeField] private string className = System.String.Empty;
            [SerializeField] private uint uniqueId = 0;
            [SerializeField] private List<BaseComponent> components = new List<BaseComponent>();
            [SerializeField] private List<BaseAttribute> attributes = new List<BaseAttribute>();
            [SerializeField] private EntityState currentState = new EntityState();
            
            public string GetClassName => className;
            public GameEnums.EntityType GetNameId => nameId;
            public GameEnums.GroupTypes GetGroupType => groupType;
            public uint GetUniqueId => uniqueId;
            public EntityState GetCurrentState => currentState;
            public List<BaseComponent> GetComponents => components;
            public List<BaseAttribute> GetAttributes => attributes;
            

            #region constructor
            
            public EntityData(){}

            public EntityData(EntityData aData)
            {
                groupType = aData.groupType;
                nameId = aData.nameId;
                className = aData.className;
                uniqueId = aData.uniqueId;
                currentState = aData.currentState;
                attributes = new List<BaseAttribute>(aData.attributes);
                components = new List<BaseComponent>(aData.components);
            }

            public EntityData Clone()
            {
                return new EntityData(this);
            }
            
            #endregion


            #region init and destroy

            public void Init(
                GameEnums.GroupTypes aGroupType,
                GameEnums.EntityType aNameId,
                string aClassName,
                uint aUniqueId,
                List<BaseComponent> aComponentsList,
                List<BaseAttribute> anAttributesList)
            {
                groupType = aGroupType;
                nameId = aNameId;
                className = aClassName;
                uniqueId = aUniqueId;

                attributes = new List<BaseAttribute>(anAttributesList);
                components = new List<BaseComponent>(aComponentsList);
            }
            
            public void Destroy()
            {
                components.Clear();
                currentState?.Destroy();
            }

            #endregion



        }

    }
}
