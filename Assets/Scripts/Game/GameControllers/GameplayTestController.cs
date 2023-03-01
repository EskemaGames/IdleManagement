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

    [SerializeField] private Camera gameCamera = null;
    
    private InputRaycastsController inputRaycastsController = new InputRaycastsController();
    private List<BuildingLogicData> buildings = new List<BuildingLogicData>();
    private List<EntityLogicData> entities = new List<EntityLogicData>();
    
    private List<IUpdateTimedSystems> updateTimedSystems = new List<IUpdateTimedSystems>(5);
    private GameTimeController timeController = new GameTimeController();
    private List<IGameTime> updateGameTimeList = new List<IGameTime>(5);
    private ISelectableObject lastSelectedObject = null;
    

    public BuildingLogicData GetBuilding(string aBuildingName)
    {
        BuildingLogicData tmpBuilding = null;
        for (int i = 0; i < buildings.Count; ++i)
        {
            BuildingLogicData building = buildings[i];
            
            if (building.GetNameId.ToString().Equals(aBuildingName))
            {
                tmpBuilding = building;
                break;
            }
        }

        return tmpBuilding;
    }
   
    
    
    
    #region monobehaviour

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

    #endregion


    

    public void Configure()
    {
        Debug.Log("GAME Configure");

        lastSelectedObject = null;
        
        //prepare the time controller to accelerate or slow down the game time
        timeController.Init(OnDayPassed, OnYearPassed);
        timeController.SetGameSpeed(Constants.Speed2);

        int layer = InspectorStorage.Self().GetLayerInt(Constants.NakedStrings.CollidablesLayer);
        layer |= InspectorStorage.Self().GetLayerInt(Constants.NakedStrings.MapObjectsLayer);
        layer |= InspectorStorage.Self().GetLayerInt(Constants.NakedStrings.EntitiesLayer);
        
        inputRaycastsController.Init(gameCamera, layer, OnInputTouchedCallback);
        
        //add the controller to our list for the update
        updateTimedSystems.Add(timeController);
        updateTimedSystems.Add(inputRaycastsController);

        
        
        //parse buildings
        for (var i = 0; i < DataDDBB.Self().GetBuildingsData.Count; ++i)
        {
            DataDDBB.EntitiesParsed building = DataDDBB.Self().GetBuildingsData[i];

            var componentsList = new List<BaseComponent>();
            
            for (var j = 0; j < building.Components.Count; ++j)
            {
                string componentName = building.Components[j].ClassName;
                
                Type myTypeLogic = Type.GetType(Constants.NamespaceBaseComponentsNameWithAppend + componentName);
                
                if (building.Components[j].ParametersToParse.Count > 0)
                {
                    var argTypeInstruction = new object[]
                        {building.Components[j].ParametersToParse};
                    BaseComponent componentLogic = (BaseComponent) Activator.CreateInstance(myTypeLogic, argTypeInstruction);
                    
                    if (componentLogic != null)
                    {
                        componentsList.Add(componentLogic);
                    }
                }
                else
                {
                    BaseComponent componentLogic = (BaseComponent) Activator.CreateInstance(myTypeLogic);
                    
                    if (componentLogic != null)
                    {
                        componentsList.Add(componentLogic);
                    }
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
                string componentName = entity.Components[j].ClassName;
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
            
            updateGameTimeList.Add(newEntity);
        }
        
    }


    public void InitGame()
    {
        Debug.Log("GAME STARTED");
        
        //get an entity to work with
        ////the farmer is the ID 1 according to the json file
        var entityToWorkWith = entities[1];
        
        //prepare the item to do the work
        ItemWorkData itemWorkData = new ItemWorkData();
        itemWorkData.Init(5, (uint)GameEnums.WorkItem.Vegetables, 0);
        
        //set the work data order
        //the delay will come from the UI or whatever, for the test no delay is added
        WorkData workData = new WorkData();
        workData.Init( (uint)GameEnums.WorkAction.Plant, 0, itemWorkData );



        //first "sync" all money and goods to the storage building
        for (var i = 1; i < buildings.Count; ++i)
        {
            var building = buildings[i];
            var money = building.GetAttributeValue(Attribute_Enums.AttributeType.InitialMoneyAttr);
            var goods = building.GetAttributeValue(Attribute_Enums.AttributeType.GoodsItemsAttr);

            var storage = buildings[0].GetLogicComponent<StorageBuildingComponentLogic>();
            storage.InitMoney((uint)money);
            storage.InitStorage((uint)buildings[i].GetNameId, (uint)goods);
        }

        Debug.Log("-- SET FIRST BUILDING WORK TO START--");
        buildings[0].AddToBuilding((uint)GameEnums.EntityType.FarmBuilding, entityToWorkWith);
        buildings[0].AddToBuilding((uint)GameEnums.EntityType.FarmBuilding, entities[0]); //add the slave as well
        buildings[0].AddToBuilding((uint)GameEnums.EntityType.SmithyBuilding, entities[2]); //add the slave as well
        buildings[0].AddToBuilding((uint)GameEnums.EntityType.MarketplaceBuilding, entities[4]); //add the slave as well
        
        //add the farmer to the building and start the work
        buildings[1].AddToBuilding((uint)GameEnums.EntityType.FarmBuilding, entityToWorkWith);
        buildings[1].AddToBuilding((uint)GameEnums.EntityType.FarmBuilding, entities[0]); //add the slave as well
        buildings[1].Start( workData,
            workData.DelayTimeToWorkAmount,
            null,
            null,
            OnWorkDone);
        
        
        Debug.Log("--SET SECOND BUILDING UPDATE WORK TO START--");
        
        //prepare the item to do the work
        ItemWorkData tmpItemWorkData = new ItemWorkData();
        tmpItemWorkData.Init(0, (uint)GameEnums.WorkItem.Update, 0);
        
        //set the work data order
        //the delay is set to 3 days
        WorkData workData2 = new WorkData();
        workData2.Init( (uint)GameEnums.WorkAction.Update, 3, tmpItemWorkData );
        
        buildings[1].Start( workData2,
            workData2.DelayTimeToWorkAmount,
            null,
            null,
            OnBuildingUpdateDone);
        
        
        
        Debug.Log("--SET THIRD BUILDING SMITHY WORK TO START--");
        
        ItemWorkData itemWorkData3 = new ItemWorkData();
        itemWorkData3.Init(20, (uint)GameEnums.WorkItem.Iron, 0);
        
        //set the work data order
        //the delay will come from the UI or whatever, for the test no delay is added
        WorkData workData3 = new WorkData();
        workData3.Init( (uint)GameEnums.WorkAction.Smithy, 0, itemWorkData3 );
        
        //add the farmer to the building and start the work
        buildings[2].AddToBuilding((uint)GameEnums.EntityType.SmithyBuilding, entities[2]); 
        buildings[2].Start( workData3,
            workData3.DelayTimeToWorkAmount,
            null,
            null,
            OnWorkSmithyDone);
        
        
        Debug.Log("--SET FOURTH BUILDING MARKET WORK TO START--");
        
        ItemWorkData itemWorkData4 = new ItemWorkData();
        itemWorkData4.Init(10, (uint)GameEnums.WorkItem.SellMarket, 5);
        
        //set the work data order
        //the delay will come from the UI or whatever, for the test no delay is added
        WorkData workData4 = new WorkData();
        workData4.Init( (uint)GameEnums.WorkAction.Market, 0, itemWorkData4 );
        
        //add the farmer to the building and start the work
        buildings[3].AddToBuilding((uint)GameEnums.EntityType.MarketplaceBuilding, entities[4]); 
        buildings[3].Start( workData4,
            workData4.DelayTimeToWorkAmount,
            null,
            null,
            OnWorkSmithyDone);
        

    }

    

    
    #region input raycasts
    
    private void OnInputTouchedCallback(GameObject aGameobject, GameEnums.EventTouchCode anEventCode)
    {
        DeselectPreviousObject();
        
        switch (anEventCode)
        {
            case GameEnums.EventTouchCode.MapObjects:
            case GameEnums.EventTouchCode.Collidables:
            case GameEnums.EventTouchCode.EntitySelected:
                ExecuteObjectSelected(aGameobject);
                break;
            
            default:
                break;
        }
    }

    private void DeselectPreviousObject()
    {
        if (lastSelectedObject != null)
        {
            lastSelectedObject.IDeselect();
            lastSelectedObject = null;
        }
    }

    private void ExecuteObjectSelected(GameObject aGameObject)
    {
        var mapObjTouched = aGameObject.GetComponentInParent<IMapObjectTouched>();
        mapObjTouched.IExecute();

        lastSelectedObject = aGameObject.GetComponentInParent<ISelectableObject>();
    }
    
    #endregion
    


    private void OnWorkDone(uint anEntityId)
    {
        Debug.Log("TEST GAMEPLAYCONTROLLER- OnWorkDone in test for entityId= " + anEntityId);
    }
    
    private void OnBuildingUpdateDone(uint anEntityId)
    {
        Debug.Log("TEST GAMEPLAYCONTROLLER- OnBuildingUpdateDone in test for entityId= " + anEntityId);
    }
    
    private void OnWorkSmithyDone(uint anEntityId)
    {
        Debug.Log("TEST GAMEPLAYCONTROLLER- On work done in OnWorkSmithyDone for entityId= " + anEntityId);
    }
    
    private void OnWorkMarketDone(uint anEntityId)
    {
        Debug.Log("TEST GAMEPLAYCONTROLLER- On work done in OnWorkMarketDone for entityId= " + anEntityId);
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
