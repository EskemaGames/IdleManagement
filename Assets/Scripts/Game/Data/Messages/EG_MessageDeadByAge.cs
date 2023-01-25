namespace EG
{
    namespace Core.Messages
    {
        [System.Serializable]
        public class EG_MessageDeadByAge : EG_Message
        {

            public uint EntityId { get; private set; }

            //lazy constructor
            public EG_MessageDeadByAge()
            {
                IsLocal = true;
                SenderId = 0;
                EntityId = 0;
            }

            public EG_MessageDeadByAge(uint id) : base(0)
            {
                EntityId = id;
            }

            public void SetData(uint id)
            {
                IsLocal = true;
                EntityId = id;
            }
        }

    }
}
