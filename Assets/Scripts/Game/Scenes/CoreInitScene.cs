using EG.Core.BootLoader;

namespace EG
{
    namespace Core.Scenes
    {
        public class CoreInitScene : EG_Scene
        {
            private LoadSceneBootActions coreBootActions;
            private string nextFlow = System.String.Empty;
            private string bootStrapActionsFileName = System.String.Empty;


            public override void Configure(EG_SceneData aData)
            {
                nextFlow = aData.GetNextFlow;
                bootStrapActionsFileName = aData.GetBootStrapName;
            }

            public override void OnEnter(bool isadditive = false)
            {
                base.OnEnter(); //we have finished our enter thus we call the base to set the flag

                //load the actions (if any) here as the core scene was loaded on start
                //as the first scene that unity boots
                coreBootActions = new LoadSceneBootActions();
                coreBootActions.Init(bootStrapActionsFileName, OnActionsDone);
            }

            public override void OnExit()
            {
                coreBootActions?.Destroy();
                coreBootActions = null;

                base.OnExit();
            }

            private void OnActionsDone()
            {
                coreBootActions?.Destroy();
                coreBootActions = null;

                EG_Core.Self().ShowLoadingScreen("");

                EG_Core.Self().ChangeToGameCoreFlow(nextFlow);
            }




        }
        
    }
}