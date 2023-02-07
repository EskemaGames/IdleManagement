using EG.Core.Interfaces;


namespace EG
{
    namespace Core.Works
    {

        public class WorkData : IWorkData
        {
            private uint id = 0; //gameenums.workaction is the id we pass around
            private uint timeToWorkAmount = 0;
            private uint delayTimeToWorkAmount = 0;
            private uint resultFromWork = 0;
            private IWorkItem item = null;

            public uint Id => id;
            public uint TimeToWorkAmount => timeToWorkAmount;
            public uint DelayTimeToWorkAmount => delayTimeToWorkAmount;
            public uint ResultFromWork => resultFromWork;
            public IWorkItem Item => item;

            public WorkData(){}
            
            public WorkData(IWorkData aData)
            {
                id = aData.Id;
                timeToWorkAmount = aData.TimeToWorkAmount;
                delayTimeToWorkAmount = aData.DelayTimeToWorkAmount;
                resultFromWork = aData.ResultFromWork;
                item = aData.Item.Clone();
            }
            
            public IWorkData Clone()
            {
                return new WorkData(this);
            }


            public void Init(uint anId, uint aDelay, uint aResult, IWorkItem anItem)
            {
                id = anId;
                timeToWorkAmount = 0;
                delayTimeToWorkAmount = aDelay;
                resultFromWork = aResult;
                item = anItem;
            }

            public void SetWorkTime(uint aTime)
            {
                timeToWorkAmount = aTime;
            }

            public void Reset()
            {
                id = 0;
                timeToWorkAmount = 0;
                delayTimeToWorkAmount = 0;
                resultFromWork = 0;
                item?.Reset();
            }


        }
    }
}
