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
        Shopkeeper,

        //buildings
        FarmBuilding = 200,
        SmithyBuilding,
        ForgeBuilding,
        StorageBuilding,
        MarketplaceBuilding,
        
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
        Smithy = 6,
        Market = 7,
        Max
    }
    
    
    [System.Serializable]
    public enum WorkItem
    {
        Vegetables = 1,
        Meat = 2,
        Update = 3,
        Iron = 4,
        SellMarket = 5,
        BuyGoods = 6,

        Max
    }
    
    
    // [System.Serializable]
    // public enum TradingFarmItems
    // {
    //     Lechugas = 1,
    //     Patatas = 2,
    //     Tomates = 3,
    //     Acelgas = 4,
    //     
    //     Max
    // }
    //
    // [System.Serializable]
    // public enum TradingSmithyItems
    // {
    //     Lechugas = 1,
    //     Patatas = 2,
    //     Tomates = 3,
    //     Acelgas = 4,
    //     
    //     Max
    // }

    
    
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
        DecreaseGoodsSupplies = 7,


        ControllerStateChanged = 4995,
        AchievementUnlock = 5000,

        UserReset = 5500,
        UserLoggedIn = 5501,
        UserLoggedOut = 5502

    }

}



