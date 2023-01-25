

namespace EG
{
    namespace Core.Entity
    {
        public class EntityState
        {

            private GameEnums.EntityStates stateId = GameEnums.EntityStates.Max;
            private float time = 0f;
            private float delayTime = 0f;
            private bool finished = false;
            private bool isBusy = false;

            private float tmpTimer = 0f;
            private float tmpDelayTimer = 0f;
            private float tmpInitTimer = 0f;
            private float tmpInitDelayTimer = 0f;

            private System.Action<float, float> onUpdateProgress = null;
            private System.Action<float, float> onUpdateProgressDelay = null;
            private System.Action onComplete = null;
            
            public bool IsFinished => finished;
            public bool IsBusy => isBusy;
            public float GetTime => time;
            public float GetDelayTime => delayTime;
            public GameEnums.EntityStates GetCurrentState => stateId;


            #region constructor

            public EntityState()
            {
            }

            public EntityState(EntityState aState)
            {
                stateId = aState.stateId;
                time = aState.time;
                delayTime = aState.delayTime;
                finished = aState.finished;
                isBusy = aState.isBusy;
                tmpTimer = aState.tmpTimer;
                tmpDelayTimer = aState.tmpDelayTimer;
                tmpInitTimer = aState.tmpInitTimer;
                tmpInitDelayTimer = aState.tmpInitDelayTimer;

                onUpdateProgress = aState.onUpdateProgress;
                onUpdateProgressDelay = aState.onUpdateProgressDelay;
                onComplete = aState.onComplete;
            }

            public EntityState Clone()
            {
                return new EntityState(this);
            }

            #endregion


            #region init and destroy

            public void Init(
                float aDays,
                float aDelayDays,
                System.Action<float, float> anUpdateProgress,
                System.Action<float, float> anUpdateDelayProgress,
                System.Action aOnComplete)
            {
                time = aDays * Constants.OneDayTime;
                delayTime = aDelayDays * Constants.OneDayTime;

                tmpTimer = 0f;
                tmpDelayTimer = 0f;
                tmpInitTimer = time;
                tmpInitDelayTimer = delayTime;

                onUpdateProgress = anUpdateProgress;
                onUpdateProgressDelay = anUpdateDelayProgress;
                onComplete = aOnComplete;

                finished = false;
            }

            public void Destroy()
            {
                onUpdateProgress = null;
                onUpdateProgressDelay = null;
                onComplete = null;
            }

            #endregion


            #region update

            public void OnUpdate(float aDeltaTime)
            {
                if (finished) return;
                
                ConsumeTime(aDeltaTime);
            }

            public void OnCancelUpdate()
            {
                onComplete = null;
                finished = true;
                isBusy = false;
                onUpdateProgress = null;
                onUpdateProgressDelay = null;
            }

            private void OnFinish()
            {
                onComplete?.Invoke();
                onComplete = null;

                onUpdateProgress = null;
                onUpdateProgressDelay = null;
            }

            #endregion


            private void OnProgressUpdate(float aTimeStep, float anInitTime)
            {
                onUpdateProgress?.Invoke(aTimeStep, anInitTime);
            }

            private void OnProgressUpdateDelay(float aTimeStep, float anInitTime)
            {
                onUpdateProgressDelay?.Invoke(aTimeStep, anInitTime);
            }
            
            private void ConsumeTime(float aDeltaTime)
            {
                if (delayTime < 0f)
                {
                    if (time > 0f && time != -1f)
                    {
                        time -= aDeltaTime;
                        tmpTimer += aDeltaTime;
                        OnProgressUpdate(tmpTimer, tmpInitTimer);
                    }
                    else
                    {
                        finished = true;
                        isBusy = false;
                        OnFinish();
                    }
                }
                else if (delayTime > 0f)
                {
                    delayTime -= aDeltaTime;
                    tmpDelayTimer += aDeltaTime;
                    OnProgressUpdateDelay(tmpDelayTimer, tmpInitDelayTimer);
                }

            }
        }

    }
}