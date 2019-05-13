using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterPanel : Inventory
{

    public Slot[] equipmentSlots;

    private static CharacterPanel instance;

    public static CharacterPanel PanelInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<CharacterPanel>();
            }
            return CharacterPanel.instance;
        }
    }

    public Slot WeaponSlot
    {
        get { return equipmentSlots[8]; } //Arik's [x] will be different
    }

    public Slot OffHandSlot
    {
        get { return equipmentSlots[11]; } //Arik's [x] will be different
    }
    public override void CreateLayout() { }
    private void Awake()
    {
        equipmentSlots = transform.GetComponentsInChildren<Slot>(); //make sure you have the right getcomponents!!!!
    }

    public void EquipItem(Slot slot, ItemScript item)
    {
        if (item.Item.ItemType == ItemType.MAINHAND || item.Item.ItemType == ItemType.TWOHAND && OffHandSlot.isEmpty)
        {
            Slot.SwapItems(slot, WeaponSlot);
        }
        
        
        else 
        {
            Slot.SwapItems(slot, Array.Find(equipmentSlots, x => x.canContain == item.Item.ItemType));
        }
        CalcStats(); //wait till end for this!

    }
    public override void ShowToolTip(GameObject slot)
    {
        Slot tmpSlot = slot.GetComponent<Slot>();
        //if (!tmpSlot.isEmpty && InventoryManager.Instance.HoverObject == null)
        print("I hit CharacterPanel showtooltip");

        if (slot.GetComponentInParent<Inventory>().IsOpen && !tmpSlot.isEmpty && InventoryManager.Instance.HoverObject == null && !InventoryManager.Instance.selectStackSize.activeSelf)
        {//added the "isOpen" to beginning of the above line
            InventoryManager.Instance.visualTextObject.text = tmpSlot.CurrentItem.GetToolTip();
            InventoryManager.Instance.sizeTextObject.text = InventoryManager.Instance.visualTextObject.text;

            InventoryManager.Instance.toolTipObject.SetActive(true);
            float xPos = slot.transform.position.x + slotPaddingLeft;
            float yPos = slot.transform.position.y - slot.GetComponent<RectTransform>().sizeDelta.y - slotPaddingTop;

            InventoryManager.Instance.toolTipObject.transform.position = new Vector2(xPos, yPos);
        }

    }

    public void CalcStats()
    {
        int agility = 0;
        int strength = 0;
        int intellect = 0;
        int stamina = 0;

        foreach (Slot slot in equipmentSlots)
        {
            if (!slot.isEmpty)
            {
                Equipment e = (Equipment)slot.CurrentItem.Item;
                agility += e.Agility;
                strength += e.Strength;
                stamina += e.Stamina;
                intellect += e.Intellect;
            }
        }
        Player.Instance.SetStats(agility, strength, stamina, intellect);
    }
}
