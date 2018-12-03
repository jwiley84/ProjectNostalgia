using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseCursor : MonoBehaviour {

    [SerializeField] Texture2D walkCursor = null; //#1!!! FIRST!
    [SerializeField] Vector2 cursorHotspot = new Vector2(96, 96); //#1!!! FOURTH!
    [SerializeField] Texture2D targetCursor = null; //#1!!! FIRST!
    [SerializeField] Texture2D unknownCursor = null; //#1!!! FIRST!

    [SerializeField] const int walkableLayerNumber = 8;
    [SerializeField] const int enemyLayerNumber = 9;

    CameraRaycaster cameraRaycaster;

    // Use this for initialization
    void Start()
    {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        cameraRaycaster.notifyLayerChangeObservers += OnLayerChanged;
    }
    void OnLayerChanged(int newLayer)
    {
        //print(cameraRaycaster.layerHit); 
        Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto); 

        switch (newLayer)
        {
            case walkableLayerNumber:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto); 
                break;
            case enemyLayerNumber:
                Cursor.SetCursor(targetCursor, cursorHotspot, CursorMode.Auto);
                break;
            default:
                Cursor.SetCursor(unknownCursor, cursorHotspot, CursorMode.Auto); 
                return;
        }
    }
}
