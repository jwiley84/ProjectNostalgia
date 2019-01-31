using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeIssues : MonoBehaviour {

    public int slots, rows, slotSize, slotPaddingLeft, slotPaddingTop, inventoryWidth, inventoryHeight;
    public RectTransform inventoryRect;
    public GameObject invImage;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //calculate width of inventory image
        inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;
        //calculate height
        inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;

        //get rectTransform
        inventoryRect = GetComponent<RectTransform>();
        RectTransform invImageRect = invImage.GetComponent<RectTransform>();


        // RectTransform slotRect = GetComponent<RectTransform>(); //this is to get the slot itself
        //RectTransform txtRect = stackText.GetComponent<RectTransform>(); //this is to get the text object on the slot

        //scale factor for text sizing

        int invPanelScaleFactor = (int)(inventoryRect.sizeDelta.x * 0.90); //makes the lettering no bigger then 60% of the slot

        invImageRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, invPanelScaleFactor); //resizes the text object to be the same size as the slot it's on
        invImageRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryRect.sizeDelta.y);
        //inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
        //inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);
        //before moving on, drag this script onto inventory image.
    }
}
