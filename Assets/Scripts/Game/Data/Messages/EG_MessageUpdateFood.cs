namespace EG
{
    namespace Core.Messages
    {
        [System.Serializable]
        public class EG_MessageUpdateFood : EG_Message
        {

            public uint CategoryId { get; private set; }
            public uint Amount { get; private set; }

            //lazy constructor
            public EG_MessageUpdateFood()
            {
                IsLocal = true;
                SenderId = 0;
                CategoryId = 0;
            }

            public EG_MessageUpdateFood(uint aCategoryId, uint anAmount) : base(0)
            {
                CategoryId = aCategoryId;
                Amount = anAmount;
            }

            public void SetData(uint aCategoryId, uint anAmount, uint aSenderId)
            {
                IsLocal = true;
                SenderId = aSenderId;
                CategoryId = aCategoryId;
                Amount = anAmount;
            }
        }

    }
}
