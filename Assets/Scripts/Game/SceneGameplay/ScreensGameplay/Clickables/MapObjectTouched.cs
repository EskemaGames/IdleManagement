using System;
using EG.Core;
using EG.Core.Entity;
using EG.Core.Interfaces;
using EG.Core.Scenes;
using EG.Core.Ui;
using SuperTiled2Unity;
using UnityEngine;


public class MapObjectTouched : MonoBehaviour, IMapObjectTouched, ISelectableObject
{
    
    private string screenName = String.Empty;
    private string buildingName = String.Empty;


    private void Awake()
    {
        ParseProperties();
    }

    
    
    public void IExecute()
    {

        EG_ScreenBase scr = EG_Core.Self().GetScreen(screenName);
        
        if (!scr.IsOpening && !scr.IsClosing && !scr.IsOpen)
        {
            GameplayTestScene scene = EG_Core.Self().CurrentScene as GameplayTestScene;
            GameplayTestController gp = scene.GameplayController;
        
            BuildingLogicData b = gp.GetBuilding(buildingName);
        
            object[] args = new object[] {b};
            scr.GetType().GetMethod("Init").Invoke(scr, args);
            scr.Open();
        
            //EG_Core.Self().PushScreen(scr);
        
        }
    }

    public void IDeselect()
    {
        
    }
    
    private void ParseProperties()
    {
        var customProps = gameObject.GetComponent<SuperCustomProperties>();
    
        if (customProps != null)
        {
            foreach (var property in customProps.m_Properties)
            {
                if (property.m_Name.Equals("ScreenUI"))
                {
                    Debug.Log(property.GetValueAsString());
                    screenName = property.GetValueAsString();
                }
                
                if (property.m_Name.Equals("BuildingName"))
                {
                    Debug.Log(property.GetValueAsString());
                    buildingName = property.GetValueAsString();
                }
            }
        }

    }

}
