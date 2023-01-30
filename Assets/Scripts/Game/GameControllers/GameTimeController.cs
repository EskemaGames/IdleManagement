using UnityEngine;


namespace EG
{
    namespace Game
    {
        public class GameTimeController : IUpdateTimedSystems, IDestroyable, IRestart
        {
            //I just calculated a time that fits my needs, nothing special, the first number came from nowhere
            //and the rest are multiplied by the number, 2x, 3x, etc
            private enum GameSpeed
            {
                Speed1X = 300, // 400,
                Speed2X = 600, //800,
                Speed4X = 1200, //1600,
                Speed5X = 1500, //2000,
                Debug = 3500,
                Max = 0
            }

            private GameSpeed Next(GameSpeed currentTime)
            {
                switch (currentTime)
                {
                    case GameSpeed.Speed1X:
                        return GameSpeed.Speed2X;
                    case GameSpeed.Speed2X:
                        return GameSpeed.Speed4X;
                    case GameSpeed.Speed4X:
                        return GameSpeed.Speed5X;
                    case GameSpeed.Speed5X:
                        return GameSpeed.Speed1X;
                    default:
                        return GameSpeed.Speed1X;
                }
            }

            //so far time goes like this with the current formula: _currentGameTimeSpeed * Time.unscaledDeltaTime
            // 1 day it's N minutes of real time
            // 1 year it's N hours of real time

            private GameSpeed gameSpeed = GameSpeed.Speed1X;
            private uint currentDay = 1;
            private uint currentYear = 1;
            private float currentGameTimeSpeed = 0f;
            private float currentTime = 0f;
            private System.Action<uint> onDayPassed;
            private System.Action<uint> onYearPassed;

            
            public void Init(System.Action<uint> ondaypassed,System.Action<uint> onyearpassed)
            {
                currentDay = 1;
                currentYear = 1;
                currentTime = 0f;

                onDayPassed = ondaypassed;
                onYearPassed = onyearpassed;
                SetDefaultTime();
            }

            public void IDestroy()
            {
                onDayPassed = null;
                onYearPassed = null;
            }

            public void IDestroyUnity()
            {
                onDayPassed = null;
                onYearPassed = null;
            }

            public void IOnRestart()
            {
                onDayPassed = null;
                onYearPassed = null;
            }

            
            #region interface IUpdate

            //parameter not used here at all
            public void IOnUpdate(float time = 0f)
            {
                currentTime += currentGameTimeSpeed * Time.unscaledDeltaTime;

                if (currentTime >= 1440.0f)
                {
                    IncreaseDay();
                }
            }
            
            public void IOnFixedUpdate(float time = 1f){}
            
            public void IOnLateUpdate(float time = 1f){}
            
            public void IOnGamePaused(bool ispaused) { }
            
            #endregion



            private void IncreaseDay()
            {
                if (currentDay < 365)
                {
                    currentTime = 0f;
                    currentDay++;
                }
                else
                {
                    currentTime = 0f;
                    currentDay = 1;
                    IncreaseYear();
                }

                OnDayPassedClbck(currentDay);
            }

            private void IncreaseYear()
            {
                if (currentYear < uint.MaxValue)
                {
                    currentYear++;
                }
                else
                {
                    currentYear = 1;
                }

                OnYearPassedClbck(currentYear);
            }

            public void SetDebugTime()
            {
                gameSpeed = GameSpeed.Debug;
                currentGameTimeSpeed = (float) GameSpeed.Debug;
            }

            public void SetDefaultTime()
            {
                gameSpeed = GameSpeed.Speed1X;
                currentGameTimeSpeed = (float) gameSpeed;
            }

            public void SetGameSpeed(string newSpeed)
            {
                if (newSpeed.Equals("1X"))
                {
                    gameSpeed = GameSpeed.Speed1X;
                }
                else if (newSpeed.Equals("2X"))
                {
                    gameSpeed = GameSpeed.Speed2X;
                }
                else if (newSpeed.Equals("4X"))
                {
                    gameSpeed = GameSpeed.Speed4X;
                }
                else if (newSpeed.Equals("5X"))
                {
                    gameSpeed = GameSpeed.Speed5X;
                }
                else
                {
                    gameSpeed = GameSpeed.Speed1X;
                }

                currentGameTimeSpeed = (float) gameSpeed;
            }

            public void IncreaseTimeSpeed()
            {
                gameSpeed = Next(gameSpeed);
                currentGameTimeSpeed = (float) gameSpeed;
            }

            public float GetCurrentTimeSpeed()
            {
                return currentGameTimeSpeed * Time.unscaledDeltaTime;
            }

            public string GetGameSpeed()
            {
                switch (gameSpeed)
                {
                    case GameSpeed.Speed1X:
                        return "1X";
                    case GameSpeed.Speed2X:
                        return "2X";
                    case GameSpeed.Speed4X:
                        return "4X";
                    case GameSpeed.Speed5X:
                        return "5X";
                }

                return System.String.Empty;
            }


            #region callbacks for time passed

            private void OnDayPassedClbck(uint day)
            {
                onDayPassed?.Invoke(day);
            }
            
            private void OnYearPassedClbck(uint year)
            {
                onYearPassed?.Invoke(year);
            }

            #endregion
        }

    }
}