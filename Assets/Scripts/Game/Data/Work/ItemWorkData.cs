using EG.Core.Interfaces;


namespace EG
{
    namespace Core.Works
    {
        public class ItemWorkData : IWorkItem
        {
            private uint id = 0; //id taken from here == GameEnums.WorkItem
            private uint amount = 0;
            private uint cost = 0;

            public ItemWorkData(){}

            public ItemWorkData(IWorkItem anItem)
            {
                id = anItem.GetId;
                amount = anItem.Amount;
                cost = anItem.Cost;
            }
            public IWorkItem Clone()
            {
                return new ItemWorkData(this);
            }

            
            public void Init(uint anAmount, uint anId, uint aCost)
            {
                amount = anAmount;
                id = anId;
                cost = aCost;
            }

            public void Reset()
            {
                amount = 0;
                id = 0;
            }

            public uint Amount => amount;
            public uint GetId => id;
            public uint Cost => cost;
        }
        
        
        

    }
}
