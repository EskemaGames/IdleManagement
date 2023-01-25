using EG.Core.Interfaces;


namespace EG
{
    namespace Core.Works
    {
        public class ItemWorkData : IWorkItem
        {
            private uint id = 0; //id taken from here == GameEnums.WorkItem
            private uint amount = 0;

            public void Init(uint anAmount, uint anId)
            {
                amount = anAmount;
                id = anId;
            }

            public void Reset()
            {
                amount = 0;
                id = 0;
            }

            public uint Amount => amount;
            public uint GetId => id;
        }
        
        
        

    }
}
