
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
        }


        public interface IWorkData
        {
            void Init(uint aTime, uint aDelay, uint aResult, IWorkItem anItem);
            void Reset();
            uint TimeToWorkAmount { get; }
            uint DelayTimeToWorkAmount { get; }
            uint ResultFromWork { get; }
            IWorkItem Item { get; }
        }




    }
}