
namespace EG
{
    namespace Core.Interfaces
    {
        
        public interface IUpdateTimedSystems
        {
            void IOnUpdate(float time = 1f);
            void IOnFixedUpdate(float time = 1f);
            void IOnLateUpdate(float time = 1f);
            void IOnGamePaused(bool ispaused);
        }
        
        public interface IGameTime
        {
            void IOnYearPassed(uint aYear);
            void IOnDayPassed(uint aDay);
        }

        public interface IEntityView
        {
            
        }


    }
}