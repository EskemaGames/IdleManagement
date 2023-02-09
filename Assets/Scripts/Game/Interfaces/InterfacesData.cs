
namespace EG
{
    namespace Core.Interfaces
    {
        public interface IWorkItem
        {
            void Init(uint anAmount, uint anId, uint aCost);
            void Reset();
            uint Amount { get; }
            uint Cost { get; }
            uint GetId { get; }
            IWorkItem Clone();
        }


        public interface IWorkData
        {
            void Init(uint anId, uint aDelay, IWorkItem anItem);
            void SetWorkTime(uint aTime);
            void Reset();
            uint TimeToWorkAmount { get; }
            uint DelayTimeToWorkAmount { get; }
            uint ResultFromWork { get; }
            uint Id { get; }
            IWorkItem Item { get; }
            IWorkData Clone();
        }




    }
}