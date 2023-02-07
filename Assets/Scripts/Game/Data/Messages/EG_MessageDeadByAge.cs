namespace EG
{
    namespace Core.Messages
    {
        [System.Serializable]
        public class EG_MessageDeadByAge : EG_Message
        {

            public uint EntityId { get; private set; }
            public uint CategoryId { get; private set; }

            //lazy constructor
            public EG_MessageDeadByAge()
            {
                IsLocal = true;
                SenderId = 0;
                EntityId = 0;
            }

            public EG_MessageDeadByAge(uint anId) : base(0)
            {
                EntityId = anId;
            }

            public void SetData(uint anId, uint aCategoryId)
            {
                IsLocal = true;
                EntityId = anId;
                CategoryId = aCategoryId;
            }
        }

    }
}
