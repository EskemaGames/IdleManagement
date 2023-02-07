using System;

public class GameEnums
{
    
    
    [System.Serializable]
    public enum EntityType
    {
        //people
        Slave = 1,
        Farmer,
        Merchant,
        BlackSmith,
        Warrior,
        
        //buildings
        Farm,
        Smithy,
        Forge,
        Storage,
        
        Max
    }


    [Serializable]
    public enum GroupTypes
    {
        Enemies,
        Buildings,
        Npc,
        Max
    };

    [System.Serializable]
    public enum WorkAction
    {
        Plant = 1,
        Paint = 2,
        Repairing = 3,
        Travel = 4,
        Update = 5,
        Max
    }
    
    
    [System.Serializable]
    public enum WorkItem
    {
        Vegetables = 1,
        Meat = 2,
        Update = 3,

        Max
    }

    
    
    [Serializable]
    public enum Difficulty
    {
        EASY,
        NORMAL,
        HARD,
        MAX
    };


    public enum MessageTypes
    {
        Empty = 0,
        DeadByAge = 1,
        UpdatePayments = 2,
        ConsumeFood = 3,
        StoreGoodsSupplies = 4,
        StoreMoney = 5,
        UpdateFood = 6,


        ControllerStateChanged = 4995,
        AchievementUnlock = 5000,

        UserReset = 5500,
        UserLoggedIn = 5501,
        UserLoggedOut = 5502

    }

}



