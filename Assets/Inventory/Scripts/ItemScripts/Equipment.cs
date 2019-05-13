using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
    public int Intellect { get; set; }//intelligence
    public int Stamina { get; set; }//constitution
    public int Strength { get; set; }
    public int Agility { get; set; } //dexterity

    public Equipment()
    {

    }

    public Equipment(string itemName, string description, ItemType itemType, Quality quality, string spriteNeutral, string spriteHighlighted, int maxSize, int intellect, int stamina, int strength, int agility) 
        : base(itemName, description, itemType, quality, spriteNeutral, spriteHighlighted, maxSize)
    {
        this.Intellect = intellect;
        this.Stamina = stamina;
        this.Strength = strength;
        this.Agility = agility;
    }

    public override void Use(Slot slot, ItemScript item)
    {
        CharacterPanel.PanelInstance.EquipItem(slot, item);
    }

    public override string GetTooltip()
    {
        string stats = string.Empty;//new
        if (Strength > 0)
        {
            stats += "\n" + Strength.ToString() + " Strength";
        }
        if (Intellect > 0)
        {
            stats += "\n" + Intellect.ToString() + " Intellect";
        }
        if (Agility > 0)
        {
            stats += "\n" + Agility.ToString() + " Agility";
        }
        if (Stamina > 0)
        {
            stats += "\n" + Stamina.ToString() + " Staamina";
        }
        
        //Gets the tooltip from the base class
        string itemTip = base.GetTooltip();

        //Returns the complete tooltip
        return string.Format("{0}" + "<size=14>{1}</size>", itemTip, stats);
    }
}
