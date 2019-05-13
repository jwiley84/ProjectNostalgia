using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment
{
    public float AttackSpeed { get; set; }

    public Weapon()
    {

    }

    public Weapon(string itemName, string description, ItemType itemType, Quality quality, string spriteNeutral, string spriteHighlighted, int maxSize, int intellect, int stamina, int strength, int agility, float attackSpeed) 
        : base(itemName, description, itemType, quality, spriteNeutral, spriteHighlighted, maxSize, intellect, stamina, strength, agility)
    {
        this.AttackSpeed = attackSpeed;
    }
    public override void Use(Slot slot, ItemScript item)
    {
        CharacterPanel.PanelInstance.EquipItem(slot, item);
    }

    public override string GetTooltip()
    {
        string equipmentTip = base.GetTooltip();

        //Adds the attackspeed to the tooltip
        return string.Format("{0} \n <size=14> AttackSpeed: {1}</size>", equipmentTip, AttackSpeed);
    }
}
