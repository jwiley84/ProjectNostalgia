using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item
{
    public int Health { get; set; }

    public int Mana { get; set; }

    public Consumable()
    {

    }

    public Consumable(string itemName, string description, ItemType itemType, Quality quality, string spriteNeutral, string spriteHighlighted, int maxSize, int health, int mana) 
        : base(itemName, description, itemType, quality, spriteNeutral, spriteHighlighted, maxSize)
    {
        this.Health = health;
        this.Mana = mana;
    }

    public override void Use(Slot slot, ItemScript item)
    {
        throw new System.NotImplementedException();
    }

    public override string GetTooltip()
    {
        string stats = string.Empty;

        if (Health > 0) //Adds health to the tooltip if it is larger than 0
        {
            stats += "\n Restores " + Health.ToString() + " Health";
        }
        if (Mana > 0) //Adds mana to the tooltip if it is larger than 0
        {
            stats += "\n Restores " + Mana.ToString() + " Mana";
        }

        //Gets the tooltip from the base class
        string itemTip = base.GetTooltip();

        //Returns the complete tooltip
        return string.Format("{0}" + "<size=14>{1}</size>", itemTip, stats);
    }
}
