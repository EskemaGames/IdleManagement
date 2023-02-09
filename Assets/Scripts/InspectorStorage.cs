using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using EG.Core;
using UnityEngine;
using EG.Core.Util;


/// <summary>
/// Storage for all gameobjects to create pools, or to have a link somewhere
/// </summary>
public class InspectorStorage : EG_Singleton<InspectorStorage>
{

    [Header("Collision masks")] [SerializeField]
    private LayerMask[] collisionsMasks = null;

    //fake classes to mimic a dictionary for visualization purposes
    [System.Serializable]
    public struct PrefabObjects
    {
        public string NameType;
        public GameObject prefab;
    }


    [Header("Characters")] 
    [SerializeField] private PrefabObjects[] characterPrefabs = null;

    [SerializeField] private GameObject prefabWeapon = null;

    //pool within the inspector to be parsed and converted on runtime to game data
    private Dictionary<string, PrefabObjects> allObjectsPrefabs = new Dictionary<string, PrefabObjects>();



    #region private vars

    //
    //private vars not serialized
    //
    private int[] collisionLayers = null;
    private string[] collisionLayerNames = null;

    #endregion







    #region init and destroy

    protected override void Initialize(bool dontdestroy = false)
    {
        base.Initialize(true);

        BaseInit();
    }

    private void BaseInit()
    {
        DOTween.Init().SetCapacity(300, 10);


        collisionLayers = new int[collisionsMasks.Length];
        collisionLayerNames = new string[collisionsMasks.Length];

        for (var i = 0; i < collisionsMasks.Length; i++)
        {
            collisionLayers[i] = (int) Mathf.Log(collisionsMasks[i].value, 2f);
            var name = LayerMask.LayerToName(collisionLayers[i]);
            collisionLayerNames[i] = name;
        }

        for (var i = 0; i < characterPrefabs.Length; ++i)
        {
            allObjectsPrefabs.Add(characterPrefabs[i].NameType, characterPrefabs[i]);
        }
    }

    public override void OnDestroy()
    {
        System.Array.Clear(collisionsMasks, 0, collisionsMasks.Length);
        System.Array.Clear(collisionLayerNames, 0, collisionLayerNames.Length);
        
        System.Array.Clear(characterPrefabs, 0, characterPrefabs.Length);

        base.OnDestroy();
    }

    #endregion



    public void Restart()
    {
    }


    #region public getters

    public GameObject GetPrefab(string name)
    {
        var prefab = allObjectsPrefabs[name];
        return prefab.prefab;
    }


    public int GetLayerNumber(string layerName)
    {
        var number = -1;

        for (var i = 0; i < collisionLayerNames.Length; i++)
        {
            var name = collisionLayerNames[i]; // LayerMask.LayerToName(collisionLayers[i]);

            if (name.Equals(layerName))
            {
                number = collisionLayers[i];
            }
        }

        return number;
    }

    public LayerMask GetLayer(string layerName)
    {
        LayerMask lm = 0;

        for (var i = 0; i < collisionLayerNames.Length; i++)
        {
            var name = collisionLayerNames[i];

            if (name.Equals(layerName))
            {
                lm = 1 << collisionLayers[i];
            }
        }

        return lm;
    }

    #endregion


    #region buttons for names

    private enum ButtonNames
    {
        Button_A,
        Button_B,
        Button_X,
        Button_Y,
        Button_STICK_LEFT,
        Button_STICK_RIGHT,
        Button_LEFT,
        Button_RIGHT,
        Button_UP,
        Button_DOWN,
        Button_RT,
        Button_LT,
        Button_RB,
        Button_LB,
        Button_START,
        Button_SELECT,
        Button_DPAD,
        Button_L3,
        Button_R3
    }

    public string GetImageName(string aName)
    {
        if (aName.Equals(ButtonNames.Button_A.ToString()))
        {
            return Constants.ControllerImages.Button_A;
        }
        else if (aName.Equals(ButtonNames.Button_B.ToString()))
        {
            return Constants.ControllerImages.Button_B;
        }
        else if (aName.Equals(ButtonNames.Button_X.ToString()))
        {
            return Constants.ControllerImages.Button_X;
        }
        else if (aName.Equals(ButtonNames.Button_Y.ToString()))
        {
            return Constants.ControllerImages.Button_Y;
        }
        else if (aName.Equals(ButtonNames.Button_STICK_LEFT.ToString()))
        {
            return Constants.ControllerImages.Button_STICK_LEFT;
        }
        else if (aName.Equals(ButtonNames.Button_STICK_RIGHT.ToString()))
        {
            return Constants.ControllerImages.Button_STICK_RIGHT;
        }
        else if (aName.Equals(ButtonNames.Button_LEFT.ToString()))
        {
            return Constants.ControllerImages.Button_LEFT;
        }
        else if (aName.Equals(ButtonNames.Button_RIGHT.ToString()))
        {
            return Constants.ControllerImages.Button_RIGHT;
        }
        else if (aName.Equals(ButtonNames.Button_UP.ToString()))
        {
            return Constants.ControllerImages.Button_UP;
        }
        else if (aName.Equals(ButtonNames.Button_DOWN.ToString()))
        {
            return Constants.ControllerImages.Button_DOWN;
        }
        else if (aName.Equals(ButtonNames.Button_RT.ToString()))
        {
            return Constants.ControllerImages.Button_RT;
        }
        else if (aName.Equals(ButtonNames.Button_LT.ToString()))
        {
            return Constants.ControllerImages.Button_LT;
        }
        else if (aName.Equals(ButtonNames.Button_LB.ToString()))
        {
            return Constants.ControllerImages.Button_LB;
        }
        else if (aName.Equals(ButtonNames.Button_RB.ToString()))
        {
            return Constants.ControllerImages.Button_RB;
        }
        else if (aName.Equals(ButtonNames.Button_START.ToString()))
        {
            return Constants.ControllerImages.Button_START;
        }
        else if (aName.Equals(ButtonNames.Button_SELECT.ToString()))
        {
            return Constants.ControllerImages.Button_SELECT;
        }
        else if (aName.Equals(ButtonNames.Button_DPAD.ToString()))
        {
            return Constants.ControllerImages.Button_DPAD;
        }
        else if (aName.Equals(ButtonNames.Button_R3.ToString()))
        {
            return Constants.ControllerImages.Button_R3;
        }
        else if (aName.Equals(ButtonNames.Button_L3.ToString()))
        {
            return Constants.ControllerImages.Button_L3;
        }
        else
        {
            return System.String.Empty;
        }
    }


    public string GetImageKeyboardName(string aName)
    {
        if (aName.Equals(ButtonNames.Button_A.ToString()))
        {
            return Constants.KeyboardImages.Button_A;
        }
        else if (aName.Equals(ButtonNames.Button_B.ToString()))
        {
            return Constants.KeyboardImages.Button_B;
        }
        else if (aName.Equals(ButtonNames.Button_X.ToString()))
        {
            return Constants.KeyboardImages.Button_X;
        }
        else if (aName.Equals(ButtonNames.Button_Y.ToString()))
        {
            return Constants.KeyboardImages.Button_Y;
        }
        else if (aName.Equals(ButtonNames.Button_STICK_LEFT.ToString()))
        {
            return Constants.KeyboardImages.Button_STICK_LEFT;
        }
        else if (aName.Equals(ButtonNames.Button_STICK_RIGHT.ToString()))
        {
            return Constants.KeyboardImages.Button_STICK_RIGHT;
        }
        else if (aName.Equals(ButtonNames.Button_LEFT.ToString()))
        {
            return Constants.KeyboardImages.Button_LEFT;
        }
        else if (aName.Equals(ButtonNames.Button_RIGHT.ToString()))
        {
            return Constants.KeyboardImages.Button_RIGHT;
        }
        else if (aName.Equals(ButtonNames.Button_UP.ToString()))
        {
            return Constants.KeyboardImages.Button_UP;
        }
        else if (aName.Equals(ButtonNames.Button_DOWN.ToString()))
        {
            return Constants.KeyboardImages.Button_DOWN;
        }
        else if (aName.Equals(ButtonNames.Button_RT.ToString()))
        {
            return Constants.KeyboardImages.Button_RT;
        }
        else if (aName.Equals(ButtonNames.Button_LT.ToString()))
        {
            return Constants.KeyboardImages.Button_LT;
        }
        else if (aName.Equals(ButtonNames.Button_LB.ToString()))
        {
            return Constants.KeyboardImages.Button_LB;
        }
        else if (aName.Equals(ButtonNames.Button_RB.ToString()))
        {
            return Constants.KeyboardImages.Button_RB;
        }
        else if (aName.Equals(ButtonNames.Button_START.ToString()))
        {
            return Constants.KeyboardImages.Button_START;
        }
        else if (aName.Equals(ButtonNames.Button_SELECT.ToString()))
        {
            return Constants.KeyboardImages.Button_SELECT;
        }
        else if (aName.Equals(ButtonNames.Button_DPAD.ToString()))
        {
            return Constants.KeyboardImages.Button_DPAD;
        }
        else if (aName.Equals(ButtonNames.Button_R3.ToString()))
        {
            return Constants.KeyboardImages.Button_R3;
        }
        else if (aName.Equals(ButtonNames.Button_L3.ToString()))
        {
            return Constants.KeyboardImages.Button_L3;
        }
        else
        {
            return System.String.Empty;
        }

    }

    #endregion




    #region listener for keyboard events

    private bool isGamepad = true;

    public bool GetIsGamepadUsed => isGamepad;
    
    
    private void RegisterForKeyboard()
    {
        if (EG_Core.Self() != null)
        {
            EG_Core.Self().SubscribeToChangeKeyboardOrGamepad(OnLayoutChanged);
        }
    }
    
    private void UnRegisterForKeyboard()
    {
        if (EG_Core.Self() != null)
        {
            EG_Core.Self().UnSubscribeToChangeKeyboardOrGamepad(OnLayoutChanged);
        }
    }
    
    private void OnLayoutChanged(bool aValue, int anId)
    {
        //true is gamepad, false is keyboard
        //id is not used here
        isGamepad = aValue;
    }
    
    #endregion


    
 


}
