
namespace EG
{
    namespace Core.Interfaces
    {
        public interface IWorkItem
        {
            void Init(uint anAmount, uint anId);
            void Reset();
            uint Amount { get; }
            uint GetId { get; }
            IWorkItem Clone();
        }


        public interface IWorkData
        {
            void Init(uint anId, uint aDelay, uint aResult, IWorkItem anItem);
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