using System;

public class GameEnums
{
    
    
    [System.Serializable]
    public enum EntityType
    {
        Farmer,
        Merchant,
        Warrior,
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
    
    [Serializable]
    public enum EntityStates
    {
        Idle,
        Travel,
        Update,
        Work,
        Max
    };

    [System.Serializable]
    public enum WorkAction
    {
        Plant,
        Paint,
        Repairing,
        Travel,
        Max
    }
    
    
    [System.Serializable]
    public enum WorkItem
    {
        Patatas = 1,
        Cebollas = 2,
        Zanahorias = 3,
        Cucharas = 4,
        Sartenes = 5,
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
    
    
    [Serializable]
    public enum WaypointsPathType
    {
        Loop,
        PingPong,
        Straight,
        Max
    };
    
    
    public enum MessageTypes
    {
        Empty = 0,
        DeadByAge = 1,
        SetPayments = 2,

        ControllerStateChanged = 4995,
        AchievementUnlock = 5000,

        UserReset = 5500,
        UserLoggedIn = 5501,
        UserLoggedOut = 5502

    }

}



