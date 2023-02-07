namespace EG
{
    namespace Core.Messages
    {
        [System.Serializable]
        public class EG_MessageConsumeFood : EG_Message
        {

            public uint CategoryId { get; private set; }
            public uint Amount { get; private set; }

            //lazy constructor
            public EG_MessageConsumeFood()
            {
                IsLocal = true;
                SenderId = 0;
                CategoryId = 0;
            }

            public EG_MessageConsumeFood(uint aCategoryId, uint anAmount) : base(0)
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
