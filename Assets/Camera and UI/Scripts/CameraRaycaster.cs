using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;
//THERE'S SOME NEW STUFF HERE


public class CameraRaycaster : MonoBehaviour
{

    // INSPECTOR PROPERTIES RENDERED BY CUSTOM EDITOR SCRIPT
    [SerializeField] int[] layerPriorities; //new
    [SerializeField] string stringToPrint = "THIS IS THE SONG THAT NEVER ENDS";
    
    float maxRaycastDepth = 100f; //new and changed name
    int topPriorityLayerLastFrame = -1; //default layer will give the ? cursor //new


    //ALL NEW
    //Setup delegates for broadcasting layer changes to other classes
    public delegate void OnCursorLayerChange (int newLayer); //declare new delegate type
    public event OnCursorLayerChange notifyLayerChangeObservers; //instantiate an observer set, to which other things can subscribe

    public delegate void OnClickPriorityLayer(RaycastHit raycastHit, int layerHit);
    public event OnClickPriorityLayer notifyMouseClickObservers;
    //END ALL NEW
    
    void Update()
    { 
        //check if pointer is over ann interactable UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            NotifyObserversIfLayerChanged(5);
            return; //stop looking for other objects
        }

        // Raycast to max depth, every frame, as things move around under mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] raycastHits = Physics.RaycastAll(ray, maxRaycastDepth);

        RaycastHit? priorityHit = FindTopPriorityHit(raycastHits);
        if (!priorityHit.HasValue) //if the hit has no priority 
        {
            NotifyObserversIfLayerChanged(0); //broadcast default layer
            return;
        }

        //notify delegates of layer change
        var layerHit = priorityHit.Value.collider.gameObject.layer;
        NotifyObserversIfLayerChanged(layerHit);

        //notify delegates of highest priority hit
        if (Input.GetMouseButton(0))
        {
            //if (priorityHit.HasValue)
            //{
            //    print(layerHit);
            //}
            notifyMouseClickObservers(priorityHit.Value, layerHit);
        }

        //print(stringToPrint);
    }
    
    RaycastHit? FindTopPriorityHit (RaycastHit[] raycastHits)
    {
        List<int> layersofHitColliders = new List<int>();
        foreach (RaycastHit hit in raycastHits)
        {
            layersofHitColliders.Add(hit.collider.gameObject.layer);
        }
        foreach (int layer in layerPriorities)
        {
            foreach (RaycastHit hit in raycastHits)
            {
                if (hit.collider.gameObject.layer == layer)
                {
                    //print(layer);
                    return hit; //boom. done
                }
            }
        }
        return null; // cannot make gameobjects? nullable.
    }

    void NotifyObserversIfLayerChanged(int newLayer)
    {
        if (newLayer != topPriorityLayerLastFrame)
        {
            topPriorityLayerLastFrame = newLayer;
            //notifyLayerChangeObservers(newLayer);
        }
    }
}
