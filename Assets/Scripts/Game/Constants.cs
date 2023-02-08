using UnityEngine;


public class Constants
{

        
        public const int LanguageVersionId = 1;
        public const int DataVersionId = 1;
        public const int PreferencesVersionId = 1;
        public const string SaveStateName = "SaveState";
        public const string PreferencesNameData = "UserData";
        public const string SaveGameNameData = "SaveGame";
        public const string SaveExtension = ".data";

        static public string GreenColor = "[c][00ff00ff]";
        public const float OneDayTime = 1440f; //60 minutes X 24 hours
        public const string Speed1 = "1X";
        public const string Speed2 = "2X";
        public const string Speed4 = "4X";
        public const string Speed5 = "5X";
        
        //this MUST match whatever value you set within the poolcontroller child gameobject within the scene or scenes where is at
        public static Vector3 Static_OutsideScreenPos = new Vector3(-5000f, -5000f, -5000f);

        
        public const uint EmptyId = 0; //used for any "non-used" stuff

        public const string NamespaceBaseEntityNameWithAppend = "EG.Core.Entity.";
        public const string NamespaceBaseComponentsNameWithAppend = "EG.Core.ComponentsSystem.";
        


        public class ParametersToParse
        {
                public const string TotalScenes = "TotalScenes";
                public const string TotalUnityScenes = "TotalUnityScenes";
                public const string UnityScene = "UnityScene";
                public const string TotalParameters = "TotalParameters";
                public const string ParameterExtra = "ParameterExtra";
                public const string SceneName = "SceneName";
                public const string Bootstrap = "Bootstrap";
                public const string NextFlowAction = "NextFlowAction";
                public const string Restartable = "Restartable";
                public const string FlowToExecute = "FlowToExecute";
                public const string Number = "Number";
                public const string Id = "Id";
        }
        
                
        public class FlowStates
        {
                //some hardcoded strings for easy access to the gameplay states
                public const string LoadNextLevel = "LoadNextLevel";
                public const string StartNewLevel = "StartNewLevel";
                public const string Tutorial = "Tutorial";
                public const string Intro = "Intro";
                public const string StartNewLevelWithTutorial = "StartNewLevelWithTutorial";
                public const string CheckNextLevelIsTutorial = "CheckNextLevelIsTutorial";
                public const string GameplayTutorial = "GameplayTutorial";
                
                //some hardcoded strings for easy access to the core flow states
                public const string FlowExitFromGameplay = "FlowExitFromGameplay";
                public const string FlowMenuSceneToLoadGameplay = "FlowMenuSceneToLoadGameplay";
                public const string FlowMenuSceneToContinueGameplay = "FlowMenuSceneToContinueGameplay";
                public const string FlowMenuSceneToLevelGameplayTutorial = "FlowMenuSceneToLevelGameplayTutorial";
        }


        //used for translations or other stuff
        public class NakedStrings
        {
                //common stuff to all games
                public const string LoadingText = "LoadingText";
                public const string ControllerDisconnectedTitle = "ControllerDisconnectedTitle";
                public const string ControllerDisconnectedDescription = "ControllerDisconnectedDescription";
                public const string ControllerPressButtonToChangeGamepad = "ControllerPressButtonToChangeGamepad";

                public const string EntityType = "EntityType";


                //audio files
                public const string MenuMusic = "MenuMusic";
                public const string AudioFX_UI = "UIs";
                public const string AudioFX_Common = "CommonLevels";
                public const string AudioFX_Intro = "Intro";
                public const string AudioFX_Level1 = "Level1";
                public const string AudioFX_Tutorial = "Tutorial";

                //animations
                public const string AnimationInitToIdle = "Init";
                public const string AnimationIdle = "Idle";
                public const string AnimationRestart = "Restart";
                
                //misc
                public const string Empty = "Empty";
        }



        public class KeyboardImages
        {
                public const string Button_A = "PC/KEY_Ctrl";
                public const string Button_B = "PC/KEY_Esc";
                public const string Button_X = "PC/KEY_Alt";
                public const string Button_Y = "XboxOne/XB1_Y";
                public const string Button_STICK_LEFT = "PC/KEY_XboxOne/XB1_LeftStick";
                public const string Button_STICK_RIGHT = "PC/KEY_XboxOne/XB1_RightStick";
                public const string Button_LEFT = "PC/KEY_ArrowLeft";
                public const string Button_RIGHT = "PC/KEY_ArrowRight";
                public const string Button_RT = "PC/KEY_X";
                public const string Button_LT = "PC/KEY_Z";
                public const string Button_RB = "PC/KEY_X";
                public const string Button_LB = "PC/KEY_Z";
                public const string Button_UP = "PC/KEY_ArrowUp";
                public const string Button_DOWN = "PC/KEY_ArrowDown";
                public const string Button_START = "PC/KEY_EnterLarge";
                public const string Button_SELECT = "PC/KEY_Esc";
                public const string Button_DPAD = "XboxOne/XB1_DPad";
                public const string Button_L3 = "XboxOne/XB1_LeftStick"; // L3";
                public const string Button_R3 = "XboxOne/XB1_RightStick"; // R3";
        }



        //name of the sprites defined within the atlas or resources folder, whatever...
        public class ControllerImages
        {

#if UNITY_XBOXONE
        public const string Button_A = "XboxOne/XB1_A";          
        public const string Button_B = "XboxOne/XB1_B";
        public const string Button_X = "XboxOne/XB1_X";
        public const string Button_Y = "XboxOne/XB1_Y";
        public const string Button_STICK_LEFT = "XboxOne/XB1_LeftStick";
        public const string Button_STICK_RIGHT = "XboxOne/XB1_RightStick";
        public const string Button_LEFT = "XboxOne/XB1_DPad_Left";
        public const string Button_RIGHT = "XboxOne/XB1_DPad_Right";
        public const string Button_RT = "XboxOne/XB1_RT";
        public const string Button_LT = "XboxOne/XB1_LT";
        public const string Button_RB = "XboxOne/XB1_RB";
        public const string Button_LB = "XboxOne/XB1_LB";
        public const string Button_UP = "XboxOne/XB1_DPad_Up";
        public const string Button_DOWN = "XboxOne/XB1_DPad_Down";
        public const string Button_START = "XboxOne/XB1_Menu";
        public const string Button_SELECT = "XboxOne/XB1_View";
        public const string Button_DPAD = "XboxOne/XB1_DPad";
        public const string Button_L3 = "XboxOne/XB1_L3";
        public const string Button_R3 = "XboxOne/XB1_R3";

#elif UNITY_STANDALONE || UNITY_EDITOR
                public const string Button_A = "XboxOne/XB1_A";
                public const string Button_B = "XboxOne/XB1_B";
                public const string Button_X = "XboxOne/XB1_X";
                public const string Button_Y = "XboxOne/XB1_Y";
                public const string Button_STICK_LEFT = "XboxOne/XB1_LeftStick";
                public const string Button_STICK_RIGHT = "XboxOne/XB1_RightStick";
                public const string Button_LEFT = "XboxOne/XB1_DPad_Left";
                public const string Button_RIGHT = "XboxOne/XB1_DPad_Right";
                public const string Button_RT = "XboxOne/XB1_RT";
                public const string Button_LT = "XboxOne/XB1_LT";
                public const string Button_RB = "XboxOne/XB1_RB";
                public const string Button_LB = "XboxOne/XB1_LB";
                public const string Button_UP = "XboxOne/XB1_DPad_Up";
                public const string Button_DOWN = "XboxOne/XB1_DPad_Down";
                public const string Button_START = "XboxOne/XB1_Menu";
                public const string Button_SELECT = "XboxOne/XB1_View";
                public const string Button_DPAD = "XboxOne/XB1_DPad";
                public const string Button_L3 = "XboxOne/XB1_LeftStick"; // L3";
                public const string Button_R3 = "XboxOne/XB1_RightStick"; // R3";

#elif UNITY_PS4
        public const string Button_A = "PS4/PS4_Cross";
        public const string Button_B = "PS4/PS4_Circle";
        public const string Button_X = "PS4/PS4_Square";
        public const string Button_Y = "PS4/PS4_Triangle";
        public const string Button_STICK_LEFT = "PS4/PS4_LeftStick";
        public const string Button_STICK_RIGHT = "PS4/PS4_RightStick";
        public const string Button_LEFT = "PS4/PS4_Dpad_Left";
        public const string Button_RIGHT = "PS4/PS4_Dpad_Right";
        public const string Button_RT = "PS4/PS4_R2";
        public const string Button_LT = "PS4/PS4_L2";
        public const string Button_RB = "PS4/PS4_R1";
        public const string Button_LB = "PS4/PS4_L1";
        public const string Button_UP = "PS4/PS4_Dpad_Up";
        public const string Button_DOWN = "PS4/PS4_Dpad_Down";
        public const string Button_START = "PS4/PS4_Options";
        public const string Button_SELECT = "PS4/PS4_TouchPad";
        public const string Button_DPAD = "PS4/PS4_Dpad";
        public const string Button_L3 = "PS4/PS4_L3";
        public const string Button_R3 = "PS4/PS4_R3";

#elif UNITY_SWITCH
        public const string Button_A = "Switch/Switch_A";
        public const string Button_B = "Switch/Switch_B";
        public const string Button_X = "Switch/Switch_X";
        public const string Button_Y = "Switch/Switch_Y";
        public const string Button_STICK_LEFT = "Switch/Switch_Stick";
        public const string Button_STICK_RIGHT = "Switch/Switch_Stick";
        public const string Button_LEFT = "Switch/Switch_Left";
        public const string Button_RIGHT = "Switch/Switch_Right";
        public const string Button_RT = "Switch/Switch_R";
        public const string Button_LT = "Switch/Switch_L";
        public const string Button_RB = "Switch/Switch_ZR";
        public const string Button_LB = "Switch/Switch_ZL";
        public const string Button_UP = "Switch/Switch_Up";
        public const string Button_DOWN = "Switch/Switch_Down";
        public const string Button_START = "Switch/Switch_Home";
        public const string Button_SELECT = "Switch/Switch_Capture";
        public const string Button_DPAD = "Switch/Switch_Plus";
        public const string Button_L3 = "Switch/Switch_L3";
        public const string Button_R3 = "Switch/Switch_R3";

#endif
        }

}
