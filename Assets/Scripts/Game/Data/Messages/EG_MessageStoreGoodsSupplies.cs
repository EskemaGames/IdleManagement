namespace EG
{
    namespace Core.Messages
    {
        [System.Serializable]
        public class EG_MessageStoreGoodsSupplies : EG_Message
        {

            public uint CategoryEnumId { get; private set; }
            public uint Amount { get; private set; }

            //lazy constructor
            public EG_MessageStoreGoodsSupplies()
            {
                IsLocal = true;
                SenderId = 0;
                CategoryEnumId = 0;
            }

            public EG_MessageStoreGoodsSupplies(uint aCategoryId, uint anAmount) : base(0)
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
