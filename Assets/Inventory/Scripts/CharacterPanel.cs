using System;
using UnityEngine;

public class CharacterPanel : Inventory
{

    public Slot[] equipmentSlots;

    private static CharacterPanel instance;
    private bool twoHanded = false; // ATTACK!  


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
        // ATTACK! Added MASSIVE () SECTION BELOW

        if (item.Item.ItemType == ItemType.MAINHAND || (item.Item.ItemType == ItemType.TWOHAND || item.Item.ItemType == ItemType.RANGED || item.Item.ItemType == ItemType.MAGIC) && OffHandSlot.isEmpty )
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

    public override void SaveInventory()
    {
        print("i'm saving the Char panel!");
        string content = string.Empty; //stolen
        for (int i = 0; i < equipmentSlots.Length; i++) // type 'for' and tabtab
        {
            if (!equipmentSlots[i].isEmpty) // if + tabtab
            {
                content += i + "-" + equipmentSlots[i].Items.Peek().Item.ItemName + ";"; //stolen and altered
                PlayerPrefs.SetString("CharPanel", content); //stolen and altered
                PlayerPrefs.Save(); //stolen
            }
        }
    }

    public override void LoadInventory()
    {
        print("i'm loading the Char panel!");
        //foreach (Slot slot in equipmentSlots)
        //{
        //    slot.ClearSlot();
        //}

        string content = PlayerPrefs.GetString("CharPanel"); //stolen and altered
        string[] splitContent = content.Split(';'); //stolen
        int test = splitContent.Length;
        for (int i = 0; i < splitContent.Length - 1; i++)
        {
            string[] splitValues = splitContent[i].Split('-'); //stolen

            #region testing
            foreach (var item in splitValues)
            {
                print(item);
            }
            #endregion


            int index = Int32.Parse(splitValues[0]); //stolen
            string itemName = splitValues[1]; //stolen
            GameObject loadedItem = Instantiate(InventoryManager.Instance.itemObject); //stolen

            loadedItem.AddComponent<ItemScript>(); //stolen

            if (index == 8 || index == 11) //remember, Arik has different indexes for his weapon slot 
            {
                loadedItem.GetComponent<ItemScript>().Item = InventoryManager.Instance.ItemContainer.Weapons.Find(x => x.ItemName == itemName);
                
                if (index == 8)
                {
                    //this is the logic to swapthe the weapons
                }
            }
            else
            {
                loadedItem.GetComponent<ItemScript>().Item = InventoryManager.Instance.ItemContainer.Equipment.Find(x => x.ItemName == itemName);
            }

            equipmentSlots[index].AddItem(loadedItem.GetComponent<ItemScript>());
            Destroy(loadedItem);
            CalcStats();
        }
    }
}
