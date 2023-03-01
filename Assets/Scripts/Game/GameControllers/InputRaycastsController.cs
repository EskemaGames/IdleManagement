using UnityEngine;


public class InputRaycastsController : IUpdateTimedSystems
{

    private Camera camera = null;
    private int layerRaymask;
    private System.Action<GameObject, GameEnums.EventTouchCode> onResponseInput;

    
    #region init and destroy
    
    public void Init(Camera aCamera, int interactablesMask, System.Action<GameObject, GameEnums.EventTouchCode> aCallback)
    {
        layerRaymask = interactablesMask;
        camera = aCamera;
        onResponseInput = aCallback;
    }

    public void Destroy()
    {
        onResponseInput = null;
    }
  
    #endregion
    
    

    public void IOnUpdate(float time = 1)
    {
        HandleInput();
    }

    public void IOnFixedUpdate(float time = 1) { }
    public void IOnLateUpdate(float time = 1) { }
    public void IOnGamePaused(bool ispaused) { }

    

    private void HandleInput()
    {
        if (!Input.GetMouseButtonUp(0)) return;
        
        if (UICamera.isOverUI) return;
        
        RaycastHit2D hit = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 500f, layerRaymask);
 
        if(hit.collider != null)
        {
            if (hit.collider.gameObject.layer == InspectorStorage.Self().GetLayerNumber(Constants.NakedStrings.MapObjectsLayer))
            {
                OnEntityClick(hit.collider.gameObject, GameEnums.EventTouchCode.MapObjects);
                return;
            }
            
            if (hit.collider.gameObject.layer == InspectorStorage.Self().GetLayerNumber(Constants.NakedStrings.EntitiesLayer))
            {
                OnEntityClick(hit.collider.gameObject, GameEnums.EventTouchCode.EntitySelected);
                return;
            }
            
            if (hit.collider.gameObject.layer == InspectorStorage.Self().GetLayerNumber(Constants.NakedStrings.CollidablesLayer))
            {
                OnEntityClick(hit.collider.gameObject, GameEnums.EventTouchCode.Collidables);
                return;
            }
        }
        

        if (Input.GetMouseButtonUp(0))
        {
            OnClearEntitySelection();
        }

    }



    private void OnEntityClick(GameObject aGameObject, GameEnums.EventTouchCode anEventCode)
    {
        onResponseInput?.Invoke(aGameObject, anEventCode);
    }

    private void OnClearEntitySelection()
    {
        onResponseInput?.Invoke(null, GameEnums.EventTouchCode.EntityDeselected);
    }
    
}
