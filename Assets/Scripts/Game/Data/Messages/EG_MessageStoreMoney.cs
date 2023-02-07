namespace EG
{
    namespace Core.Messages
    {
        [System.Serializable]
        public class EG_MessageStoreMoney : EG_Message
        {

            public uint CategoryEnumId { get; private set; }
            public uint Amount { get; private set; }

            //lazy constructor
            public EG_MessageStoreMoney()
            {
                IsLocal = true;
                SenderId = 0;
                CategoryEnumId = 0;
            }

            public EG_MessageStoreMoney(uint aCategoryId, uint anAmount) : base(0)
            {
                CategoryEnumId = aCategoryId;
                Amount = anAmount;
            }

            public void SetData(uint aCategoryId, uint anAmount, uint aSenderId)
            {
                IsLocal = true;
                SenderId = aSenderId;
                CategoryEnumId = aCategoryId;
                Amount = anAmount;
            }
        }

    }
}
