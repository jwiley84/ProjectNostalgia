using System.Collections;
using System.Collections.Generic;
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

    public Text sizeTextObject;
    public Text visualTextObject;
    public Text stackText;

    public Canvas canvas;

    public EventSystem eventSystem;

    private int splitAmt;
    private int maxStackCount;

    private Slot from; //removed the static keyword
    private Slot to; //broke this apart from the from, to above
    private Slot movingSlot; //removed the static keyword

    private GameObject clicked; //removed the static keyword
    private GameObject hoverObject;//removed the static keyword

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


    // these are properties stolen from Inventory.cs

    #endregion

    #region Methods
    // Start is called before the first frame update
    void Start()
    {
        
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
    #endregion
}
