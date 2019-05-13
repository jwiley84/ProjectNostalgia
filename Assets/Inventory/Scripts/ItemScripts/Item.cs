using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    #region Properties
    public ItemType ItemType { get; set; }

    public Quality Quality { get; set; }

    public string SpriteNeutral { get; set; }

    public string SpriteHighlighted { get; set; }

    public string ItemName { get; set; }

    public string Description { get; set; }

    public int MaxSize { get; set; }

    #endregion

    public Item()
    {
        //this is the empty constructor for the XML serializer
    }

    public Item(string itemName,string description, ItemType itemType, Quality quality, string spriteNeutral, string spriteHighlighted, int maxSize)
    {
        // THIS means the thing that is BUILT with this CONSTRUCTOR
        // this.ItemName sets the ItemName from above with what will be passed in
        // when this item is constructed.
        this.ItemName = itemName;
        this.Description = description;
        this.ItemType = itemType;
        this.Quality = quality;
        this.SpriteNeutral = spriteNeutral;
        this.SpriteHighlighted = spriteHighlighted;
        this.MaxSize = maxSize;
    }

    public virtual string GetTooltip()
    {
        string stats = string.Empty;
        string color = string.Empty;
        string newLine = string.Empty;

        if(Description != string.Empty)
        {
            newLine = "\n";
        }

        switch(Quality)
        {
            case Quality.WEIRD:
                color = "magenta";
                break;
            case Quality.AWESOME:
                color = "purple";
                break;
            case Quality.RARE:
                color = "orange";
                break;
            case Quality.GLOWY:
                color = "aqua";
                break;
            default:
                break;
        }

        return string.Format("<size=16><color=" + color + ">{0}</color></size><size=14><color=#add8e6ff><i>" + newLine + "{1} </i></color></size>", ItemName, Description);
        
    }

    public abstract void Use(Slot slot, ItemScript item);
    
}
