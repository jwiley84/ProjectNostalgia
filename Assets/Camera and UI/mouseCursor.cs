using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseCursor : MonoBehaviour {

    [SerializeField] Texture2D walkCursor = null; //#1!!! FIRST!
    [SerializeField] Vector2 cursorHotspot = new Vector2(96, 96); //#1!!! FOURTH!
    [SerializeField] Texture2D targetCursor = null; //#1!!! FIRST!
    [SerializeField] Texture2D unknownCursor = null; //#1!!! FIRST!

    CameraRaycaster cameraRaycaster;

    // Use this for initialization
    void Start()
    {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        cameraRaycaster.layerChangeObservers += OnLayerUpdate;
    }
    void OnLayerUpdate()
    {
        //print(cameraRaycaster.layerHit); //#1!!! delete this line! SECOND!
        Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto); //#1!!! THIRD

        switch (cameraRaycaster.layerHit)
        {
            case Layer.Walkable:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto); //#1!!! THIRDwalkCursor = "Walk"';
                break;
            case Layer.Enemy:
                Cursor.SetCursor(targetCursor, cursorHotspot, CursorMode.Auto); //#1!!! THIRD
                break;
            default:
                Cursor.SetCursor(unknownCursor, cursorHotspot, CursorMode.Auto); //#1!!! THIRD
                return;
        }
    }
}
