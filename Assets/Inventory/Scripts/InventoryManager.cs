using Assets.Inventory.Scripts.ItemScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    #region Fields
    private static InventoryManager instance;

    // these are things that have been removed from Inventory.cs
    public GameObject slotPrefab;
    public GameObject iconPrefab;
    public GameObject toolTipObject;
    public GameObject selectStackSize;
    /// <summary>
    /// This object is used for instantiating items
    /// </summary>
    public GameObject itemObject;

    public Text sizeTextObject;
    public Text visualTextObject;
    public Text stackText;

    public Canvas canvas;

    public EventSystem eventSystem;

    private int splitAmt;
    private int maxStackCount;

    private Slot from;
    private Slot to;
    private Slot movingSlot;

    private GameObject clicked;
    private GameObject hoverObject;

    /// <summary>
    /// This item container contains all the items in the game
    /// </summary>
    private ItemContainer itemContainer = new ItemContainer();
    #endregion

    #region skip
    //skip these ones, I only use them for my project, not Arik's
    /// <summary>
    /// A prototy of out mana item
    /// This is used when loading a saved inventory
    /// </summary>
    public GameObject mana;

    /// <summary>
    /// A prototype of out healt potion
    /// This is used when loading a saved inventory
    /// </summary>
    public GameObject health;
    #endregion

    
    #region Properties
    public static InventoryManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryManager>();
            }

            return InventoryManager.instance;
        }

    }

    public Slot From
    {
        get
        {
            return from;
        }

        set
        {
            from = value;
        }
    }

    public Slot To
    {
        get
        {
            return to;
        }

        set
        {
            to = value;
        }
    }

    public GameObject Clicked
    {
        get
        {
            return clicked;
        }

        set
        {
            clicked = value;
        }
    }

    public int SplitAmt
    {
        get
        {
            return splitAmt;
        }

        set
        {
            splitAmt = value;
        }
    }

    public Slot MovingSlot
    {
        get
        {
            return movingSlot;
        }

        set
        {
            movingSlot = value;
        }
    }

    public GameObject HoverObject
    {
        get
        {
            return hoverObject;
        }

        set
        {
            hoverObject = value;
        }
    }

    public int MaxStackCount
    {
        get
        {
            return maxStackCount;
        }

        set
        {
            maxStackCount = value;
        }
    }


    public ItemContainer ItemContainer
    {
        get { return itemContainer; }
        set { itemContainer = value; }
    }

    #endregion

    #region Methods
    public void Start()
    {
        //Loads all the items from the XML document
        Type[] itemTypes = { typeof(Equipment), typeof(Weapon), typeof(Consumable) };
        XmlSerializer serializer = new XmlSerializer(typeof(ItemContainer), itemTypes);
        TextReader textReader = new StreamReader(Application.streamingAssetsPath + "/Items.xml");
        itemContainer = (ItemContainer)serializer.Deserialize(textReader);
        textReader.Close();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region StolenMethods
    //stole this one from Inventory.cs
    public void SetStackInfo(int maxStackCount) //check
    {
        selectStackSize.SetActive(true);
        //toolTipObject.SetActive(false); //solvable bug
        splitAmt = 0;
        this.maxStackCount = maxStackCount;
        stackText.text = splitAmt.ToString();
    }

    public void Save()
    {
        GameObject[] inventories = GameObject.FindGameObjectsWithTag("Inventory");
        foreach (GameObject inventory in inventories)
        {
            inventory.GetComponent<Inventory>().SaveInventory();
        }
    }

    public void Load()
    {
        GameObject[] inventories = GameObject.FindGameObjectsWithTag("Inventory");
        foreach (GameObject inventory in inventories)
        {
            inventory.GetComponent<Inventory>().LoadInventory();
        }
    }

    #endregion
}
