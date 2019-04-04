using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
//using UnityEngine.Mathf;

public class Inventory : MonoBehaviour {


    #region Variables
    public int rows;
    public int slots;
    private static int emptySlots;
    private float hoverYOffset;
    //position and size of inventory
    private RectTransform inventoryRect;
    public GameObject invImage;
    private float inventoryWidth, inventoryHeight;
    //private RectTransform parentInvWindow;
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

    //gaps between items
    public float slotPaddingLeft, slotPaddingTop;
    public float slotSize;
    public GameObject slotPrefab;
    private static Slot from, to;
    private List<GameObject> allSlots;
    //MOUSEOVER STUFF
    public GameObject iconPrefab;
    private static GameObject hoverObject;
    public Canvas canvas;
    //DONE //MOUSEOVER STUFF
    //DELETY STUFF
    public EventSystem eventSystem;
    //DONE DELETY STUFF

    

   
    //FADY STUFF
    private static CanvasGroup canvasGroup;
    private bool fadingIn;
    private bool fadingOut;
    public float fadeTime;
    //end fady stuff
    
    private static GameObject clicked;

    //stack splitter
    public GameObject selectStackSize;
    
    public Text stackText;
    private int splitAmt;
    private int maxStackCount;
    private static Slot movingSlot; //this is to store the items when we're moving them around
    
    private static Inventory instance;

    //TOOLTIP
    private static GameObject toolTip;
    public GameObject toolTipObject;
    private static Text sizeText;
    private static Text visualText;
    public Text sizeTextObject;
    public Text visualTextObject;
    //private static GameObject selectStackSizeStatic; //this is a solvable bug
    public GameObject frustration;
    private bool hopeIsDead = true;
        #endregion

    #region Properties
    public static int EmptySlots
    {
        get { return emptySlots; }
        set { emptySlots = value; }
    }

    public static CanvasGroup CanvasGroup
    {
        get { return Inventory.canvasGroup; }
    }

    public static Inventory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Inventory>();
            }
            return Inventory.instance;
        }
    }
    #endregion


    // Use this for initialization
    void Start () {

        canvasGroup = transform.parent.GetComponent<CanvasGroup>();
        CreateLayout();

        movingSlot = GameObject.Find("MovingSlot").GetComponent<Slot>();
        //TOOLTIP
        toolTip = toolTipObject;
        sizeText = sizeTextObject;
        visualText = visualTextObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //DELETY STUFF
        if (Input.GetMouseButtonUp(0)) //0 is left button
        {
            if (!eventSystem.IsPointerOverGameObject(-1) && from != null) //mouse pointer is -1
            //so if my pointer is NOT over a game object, and i let up on the left button (above IF)
            {
                from.ClearSlot();
                Destroy(GameObject.Find("Hover"));
                to = null;
                from = null;
                emptySlots++;
            }
            else if (!eventSystem.IsPointerOverGameObject(-1) && !movingSlot.isEmpty)
            {
                movingSlot.ClearSlot();
                Destroy(GameObject.Find("Hover"));
            }
        }
        //DONE DELETY STUFF
        //MOUSEOVER STUFF
        if (hoverObject != null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
            //Adds the offset to the position
            position.Set(position.x, position.y - hoverYOffset);
            hoverObject.transform.position = canvas.transform.TransformPoint(position);
        }
        //DONE //MOUSEOVER STUFF
        if (Input.GetKeyDown("i"))
        {
            if (canvasGroup.alpha > 0)
            {
                StartCoroutine(fadeOut());
                PutItemBack();
            }
            else
            {
                StartCoroutine(fadeIn());
            }
        }
        if (Input.GetKeyDown("="))
        {
            frustration.SetActive(!hopeIsDead);
            hopeIsDead = !hopeIsDead;
        }
    }

    //TOOLTIP
    public void ShowToolTip(GameObject slot)
    {
        Slot tmpSlot = slot.GetComponent<Slot>();
        if (!tmpSlot.isEmpty && hoverObject == null)
        //solvable bug
        //if (!tmpSlot.isEmpty && hoverObject == null && !selectStackSizeStatic.activeSelf )
        {
            //visualText.text = tmpSlot.currentItem.GetToolTip();
            sizeText.text = visualText.text;

            toolTip.SetActive(true);
            float xPos = slot.transform.position.x + slotPaddingLeft;
            float yPos = slot.transform.position.y - slot.GetComponent<RectTransform>().sizeDelta.y - slotPaddingTop;

            toolTip.transform.position = new Vector2(xPos, yPos);
        }
        
    }

    public void HideToolTip(GameObject slot)
    {
        toolTip.SetActive(false);
    }



    /// <summary>
    /// Saves the inventory and its content
    /// </summary>
    public void SaveInventory()
    {
        string content = string.Empty; //Creates a string for containing infor about the items inside the inventory

        for (int i = 0; i < allSlots.Count; i++) //Runs through all slots in the inventory
        {
            Slot tmp = allSlots[i].GetComponent<Slot>(); //Careates a reference to the slot at the current index

            if (!tmp.isEmpty) //We only want to save the info if the slot contains an item
            {
                //Creates a string with this format: SlotIndex-ItemType-AmountOfItems; this string can be read so that we can rebuild the inventory
                content += i + "-" + tmp.currentItem.type.ToString() + "-" + tmp.Items.Count.ToString() + ";";
            }
        }

        //Stores all the info in the PlayerPrefs
        PlayerPrefs.SetString("content", content);
        PlayerPrefs.SetInt("slots", slots);
        PlayerPrefs.SetInt("rows", rows);
        PlayerPrefs.SetFloat("slotPaddingLeft", slotPaddingLeft);
        PlayerPrefs.SetFloat("slotPaddingTop", slotPaddingTop);
        PlayerPrefs.SetFloat("slotSize", slotSize);
        //PlayerPrefs.SetFloat("xPos", inventoryRect.position.x);
        //PlayerPrefs.SetFloat("yPos", inventoryRect.position.y);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Loads the inventory
    /// </summary>
    public void LoadInventory()
    {
        //Loads all the inventory's data from the playerprefs
        string content = PlayerPrefs.GetString("content");
        slots = PlayerPrefs.GetInt("slots");
        rows = PlayerPrefs.GetInt("rows");
        slotPaddingLeft = PlayerPrefs.GetFloat("slotPaddingLeft");
        slotPaddingTop = PlayerPrefs.GetFloat("slotPaddingTop");
        slotSize = PlayerPrefs.GetFloat("slotSize");

        //Sets the inventorys position
        //inventoryRect.position = new Vector3(PlayerPrefs.GetFloat("xPos"), PlayerPrefs.GetFloat("yPos"), inventoryRect.position.z);

        //Recreates the inventory's layout
        CreateLayout();

        //Splits the loaded content string into segments, so that each index inthe splitContent array contains information about a single slot
        //e.g[0]0-MANA-3
        string[] splitContent = content.Split(';');

        //Runs through every single slot we have infor about -1 is to avoid an empty string error
        for (int x = 0; x < splitContent.Length - 1; x++)
        {
            //Splits the slot's information into single values, so that each index in the splitValues array contains info about a value
            //E.g[0]InventorIndex [1]ITEMTYPE [2]Amount of items
            string[] splitValues = splitContent[x].Split('-');

            int index = Int32.Parse(splitValues[0]); //InventorIndex 

            ItemType type = (ItemType)Enum.Parse(typeof(ItemType), splitValues[1]); //ITEMTYPE

            int amount = Int32.Parse(splitValues[2]); //Amount of items

            for (int i = 0; i < amount; i++) //Adds the correct amount of items to the inventory
            {
                switch (type)
                {
                    case ItemType.MANA: //Adds a manapotion
                        allSlots[index].GetComponent<Slot>().AddItem(mana.GetComponent<ItemScript>());
                        break;
                    case ItemType.HEALTH://Adds a healthpotion
                        allSlots[index].GetComponent<Slot>().AddItem(health.GetComponent<ItemScript>());
                        break;
                }
            }
        }
    }

    private void CreateLayout()
    {
        //Just In Case measure:
        //Destroyes the old slot's if we remake our inventory
        if (allSlots != null)
        {
            foreach (GameObject go in allSlots)
            {
                Destroy(go);
            }
        }

        //Instantiates the allSlot's list
        allSlots = new List<GameObject>();

        //Calculates the hoverYOffset by taking 1% of the slot size
        hoverYOffset = slotSize * 0.01f;
        
        emptySlots = slots;

        //calculate width of inventory image
        inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;
        //calculate height
        inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;

        //get rectTransform
        inventoryRect = GetComponent<RectTransform>();
        //RectTransform invImageRect = invImage.GetComponent<RectTransform>();    

        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);
        //before moving on, drag this script onto inventory image.



        //add slots
        int columns = slots / rows;
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                //instantiate means to programatically (via code) add game object to game
                //the (GameObject) casting makes it so we can fiddle with it.
                GameObject newSlot = (GameObject)Instantiate(slotPrefab);

                RectTransform slotRect = newSlot.GetComponent<RectTransform>();

                newSlot.name = "Slot";

                //set parent is because it's an image, and MUST be a child of th canvas
                newSlot.transform.SetParent(this.transform.parent);


                ///the side padding at the inventoryRect.localPosition needs to be adjusted.
                slotRect.localPosition = inventoryRect.localPosition + new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));
                //after this point, add the slot to the script on the inv gui, then hit play for shiggles
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor);
                newSlot.transform.SetParent(this.transform);

                allSlots.Add(newSlot);
            }
        }

    }

    private bool PlaceEmpty(ItemScript item) //this runs through every slot we instantiated in CreateLayout, and adds item to next avail != empty;
    {
        if (emptySlots > 0)
        {
            foreach (GameObject slot in allSlots)
            {
                Slot tmp = slot.GetComponent<Slot>(); //creates a reference to Slot.cs script of the slot we're currently looking at

                if (tmp.isEmpty)
                {
                    tmp.AddItem(item);
                    emptySlots--;
                    return true;
                }
            }
        }
        return false;
    }

    public bool AddItem(ItemScript item)
    {
        if (item.maxSize == 1)
        {
            PlaceEmpty(item);
            return true;
        }
        else
        {
            foreach (GameObject slot in allSlots)
            //the reason the game broke is because we have logic for if the slot is NOT empty and what to do, but no logic for if it IS
            //this logic runs through the whole inventory and check if an item can be stacked!
            {
                Slot tmp = slot.GetComponent<Slot>();

                if (!tmp.isEmpty)
                {
                    if (tmp.currentItem.type == item.type && tmp.isAvailable)
                    {
                        if (!movingSlot.isEmpty && clicked.GetComponent<Slot>() == tmp.GetComponent<Slot>())
                        {
                            continue;
                        }
                        else
                        {
                            tmp.AddItem(item); //Adds the item to the inventory
                            return true;
                        }
                    }
                }
            }
            //now, nothing can be stacked, so do this!
            if (emptySlots > 0)
            {
                PlaceEmpty(item);
            }
        }

        return false;
    }

    public void MoveItem(GameObject clicked)
    {
        Inventory.clicked = clicked;
        
        if (!movingSlot.isEmpty)
        {
            Slot tmp = clicked.GetComponent<Slot>();
            
            if (tmp.isEmpty)
            {
                tmp.AddItems(movingSlot.Items);
                movingSlot.Items.Clear();
                Destroy(GameObject.Find("Hover"));
            }
            else if (!tmp.isEmpty && movingSlot.currentItem.type == tmp.currentItem.type && tmp.isAvailable)
            {
                MergeStacks(movingSlot, tmp);
            }
        }
        //this is the item we just clicked on
        
        else if (from == null && canvasGroup.alpha == 1 && !Input.GetKey(KeyCode.LeftShift))
        {
            
            if (!clicked.GetComponent<Slot>().isEmpty && !GameObject.Find("Hover"))
            {
                from = clicked.GetComponent<Slot>();
                //from.GetComponent<Image>().color = Color.grey;
                //make sure using UnityEngine.UI;

                CreateHoverIcon();  
            }
        }
        
        else if (to == null && !Input.GetKey(KeyCode.LeftShift))
        {
            
            to = clicked.GetComponent<Slot>();
            Destroy(GameObject.Find("Hover"));
        }
        if (to != null && from != null)
        {
            if (!to.isEmpty && from.currentItem.type == to.currentItem.type && to.isAvailable)
            {
                MergeStacks(from, to);
            }
            else
            {
                Stack<ItemScript> tmpTo = new Stack<ItemScript>(to.Items); //pause here, go refactor slot.cs items to be public getter/setter (encapsulate AND USE field) + static
                to.AddItems(from.Items);

                if (tmpTo.Count == 0) //if you move a stack to an empty slot
                {
                    //go add this function to the slot.cs
                    from.ClearSlot();
                }
                else
                {
                    from.AddItems(tmpTo);
                }
            }
            

            //from.GetComponent<Image>().color = Color.clear;
            to = null;
            from = null;
            //MOUSEOVER STUFF
            Destroy(GameObject.Find("Hover"));
            //DONE MOUSEOVER STUFF
        }
    }
    private void CreateHoverIcon()
    {
        //MOUSEOVER STUFF
        hoverObject = (GameObject)Instantiate(iconPrefab);
        hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
        hoverObject.name = "Hover";
        

        RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
        RectTransform clickedTransform = clicked.GetComponent<RectTransform>();

        hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
        hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);
        //hoverTransform.position = new Vector3(3, -3);

        hoverObject.transform.SetParent(GameObject.Find("MainPlayerCanvas").transform, true);
        hoverObject.transform.localScale = clicked.gameObject.transform.localScale;
        //DONE MOUSEOVER STUFF

        hoverObject.transform.GetChild(0).GetComponent<Text>().text = movingSlot.Items.Count > 1 ? movingSlot.Items.Count.ToString() : string.Empty;
    }
    private void PutItemBack()
    {
        if (from != null)
        {
            Destroy(GameObject.Find("Hover"));
            from.GetComponent<Image>().color = Color.white;
            from = null;
        }
        else if (!movingSlot.isEmpty) //If we are carrying  split stack
        {
            //Removes the hover icon
            Destroy(GameObject.Find("Hover"));

            //Puts the items back one by one
            foreach (ItemScript item in movingSlot.Items)
            {
                clicked.GetComponent<Slot>().AddItem(item);
            }

            movingSlot.ClearSlot(); //Makes sure that the moving slot is empty
        }

        //Hides the UI for splitting a stack
        selectStackSize.SetActive(false);
    }
    public void SetStackInfo(int maxStackCount) //check
    {
        selectStackSize.SetActive(true);
        //toolTip.SetActive(false); //solvable bug
        splitAmt = 0;
        this.maxStackCount = maxStackCount;
        stackText.text = splitAmt.ToString();
    }

    public void SplitStack() //there are some really fucking bad issues here
    {
        selectStackSize.SetActive(false); //turn off mini UI
        
        movingSlot.Items = clicked.GetComponent<Slot>().RemoveItems(splitAmt); //items we just took when we hit OK
        from = clicked.GetComponent<Slot>();
        CreateHoverIcon();
        if (splitAmt == maxStackCount)
        {
            MoveItem(clicked);
            from.ClearSlot();
        }
        
    }

    public void ChangeStackText(int i)
    {
        splitAmt += i;
        if (splitAmt < 0)
        {
            splitAmt = 0;
        }
        if (splitAmt > maxStackCount)
        {
            splitAmt = maxStackCount;
        }

        stackText.text = splitAmt.ToString();
    }

    public void MergeStacks(Slot source, Slot destination)
    {
        
        int max = destination.currentItem.maxSize - destination.Items.Count;
        int count = source.Items.Count < max ? source.Items.Count : max;

        for (int i = 0; i < count; i++)
        {
            destination.AddItem(source.RemoveItem());
        }
        if (source.Items.Count == 0)
        {
            source.Items.Clear();
            source.stackText.text = string.Empty;
            Destroy(GameObject.Find("Hover"));
        }


    }
    private IEnumerator fadeOut()
    {
        if (!fadingOut)
        {
            fadingOut = true;
            fadingIn = false;
            StopCoroutine(fadeIn());

            float startAlpha = canvasGroup.alpha;
            float rate = 1.0f / fadeTime;
            float progress = 0.0f;

            while (progress < 1.0)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, progress);
                progress += rate * Time.deltaTime;

                yield return null;
                     
            }

            canvasGroup.alpha = 0;
            fadingOut = false;
        }
    }
    private IEnumerator fadeIn()
    {
        if (!fadingIn)
        {
            fadingOut = false;
            fadingIn = true;
            StopCoroutine(fadeOut());

            float startAlpha = canvasGroup.alpha;
            float rate = 1.0f / fadeTime;
            float progress = 0.0f;

            while (progress < 1.0)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, progress);
                progress += rate * Time.deltaTime;

                yield return null;

            }

            canvasGroup.alpha = 1;
            fadingIn = false;
        }
    }
}
