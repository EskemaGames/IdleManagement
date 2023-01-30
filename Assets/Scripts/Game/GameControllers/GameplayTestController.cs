using System;
using System.Collections.Generic;
using EG.Core.ComponentsSystem;
using EG.Core.Data;
using EG.Core.Entity;
using EG.Core.Interfaces;
using EG.Core.Works;
using EG.Game;
using UnityEngine;



public class GameplayTestController : MonoBehaviour
{

    
    private List<BuildingLogicData> buildings = new List<BuildingLogicData>();
    private List<EntityLogicData> entities = new List<EntityLogicData>();
    
    private List<IUpdateTimedSystems> updateTimedSystems = new List<IUpdateTimedSystems>(5);
    private GameTimeController timeController = new GameTimeController();
    private List<IGameTime> updateGameTimeList = new List<IGameTime>(5);
    
    

    private void Update()
    {
        for (var i = 0; i < updateTimedSystems.Count; ++i)
        {
            updateTimedSystems[i].IOnUpdate(timeController.GetCurrentTimeSpeed());
        }

        for (var i = 0; i < buildings.Count; ++i)
        {
            buildings[i].OnUpdate(timeController.GetCurrentTimeSpeed());
        }
    }


    public void Configure()
    {
        Debug.Log("GAME Configure");
        
        //prepare the time controller to accelerate or slow down the game time
        timeController.Init(OnDayPassed, OnYearPassed);
        timeController.SetGameSpeed(Constants.Speed4);
        
        //add the controller to our list for the update
        updateTimedSystems.Add(timeController);
        
        
        //parse buildings
        for (var i = 0; i < DataDDBB.Self().GetBuildingsData.Count; ++i)
        {
            DataDDBB.EntitiesParsed building = DataDDBB.Self().GetBuildingsData[i];

            var componentsList = new List<BaseComponent>();
            
            for (var j = 0; j < building.Components.Count; ++j)
            {
                string componentName = building.Components[j];
                Type myTypeLogic = Type.GetType(Constants.NamespaceBaseComponentsNameWithAppend + componentName);
                BaseComponent componentLogic = (BaseComponent) Activator.CreateInstance(myTypeLogic);
                    
                if (componentLogic != null)
                {
                    componentsList.Add(componentLogic);
                }
            }
            
            EntityData data = new EntityData();
            
            data.Init(
                GameEnums.GroupTypes.Buildings,
                building.Type,
                building.Type.ToString(),
                IdGenerator.GetNewUId(),
                componentsList,
                building.Attributes
            );

            var newBuilding = new BuildingLogicData(data);

            buildings.Add(newBuilding);
            
            updateGameTimeList.Add(newBuilding);
        }
        
        //parse all entities
        for (var i = 0; i < DataDDBB.Self().GetEntitiesData.Count; ++i)
        {
            DataDDBB.EntitiesParsed entity = DataDDBB.Self().GetEntitiesData[i];

            var componentsList = new List<BaseComponent>();
            
            for (var j = 0; j < entity.Components.Count; ++j)
            {
                string componentName = entity.Components[j];
                Type myTypeLogic = Type.GetType(Constants.NamespaceBaseComponentsNameWithAppend + componentName);
                BaseComponent componentLogic = (BaseComponent) Activator.CreateInstance(myTypeLogic);
                    
                if (componentLogic != null)
                {
                    componentsList.Add(componentLogic);
                }
            }
            
            EntityData data = new EntityData();
            
            data.Init(
                GameEnums.GroupTypes.Npc,
                entity.Type,
                entity.Type.ToString(),
                IdGenerator.GetNewUId(),
                componentsList,
                entity.Attributes
            );

            var newEntity = new EntityLogicData(data);

            entities.Add(newEntity);
        }

        
    }


    public void InitGame()
    {
        Debug.Log("GAME STARTED");
        
        EntityLogicData entityLogicData = entities[0];
            
        ItemWorkData itemWorkData = new ItemWorkData();
        itemWorkData.Init(5, (uint)GameEnums.WorkItem.Cebollas);
            
        WorkData workData = new WorkData();
        workData.Init(2, 0, itemWorkData.Amount, itemWorkData );
            
        buildings[0].AddToBuilding(entities[0]);
        buildings[0].Start( workData,2, 0, null, null, OnWorkDone);
        
        
    }



    private void OnWorkDone(uint anEntityId)
    {
        Debug.Log("On work done in test " + anEntityId);
    }
    
        
    private void OnDayPassed(uint day)
    {
        for (var i = 0; i < updateGameTimeList.Count; ++i)
        {
            updateGameTimeList[i].IOnDayPassed(day);
        }
    }

    private void OnYearPassed(uint year)
    {
        for (var i = 0; i < updateGameTimeList.Count; ++i)
        {
            updateGameTimeList[i].IOnYearPassed(year);
        }

    }
    


}
