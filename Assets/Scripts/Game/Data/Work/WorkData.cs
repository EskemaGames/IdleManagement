using EG.Core.Interfaces;


namespace EG
{
    namespace Core.Works
    {

        public class WorkData : IWorkData
        {
            private uint timeToWorkAmount = 0;
            private uint delayTimeToWorkAmount = 0;
            private uint resultFromWork = 0;
            private IWorkItem item = null;
            
            public uint TimeToWorkAmount => timeToWorkAmount;
            public uint DelayTimeToWorkAmount => delayTimeToWorkAmount;
            public uint ResultFromWork => resultFromWork;
            public IWorkItem Item => item;


            public void Init(uint aTime, uint aDelay, uint aResult, IWorkItem anItem)
            {
                timeToWorkAmount = aTime;
                delayTimeToWorkAmount = aDelay;
                resultFromWork = aResult;
                item = anItem;
            }

            public void Reset()
            {
                timeToWorkAmount = 0;
                delayTimeToWorkAmount = 0;
                resultFromWork = 0;
                item?.Reset();
            }


        }
    }
}
