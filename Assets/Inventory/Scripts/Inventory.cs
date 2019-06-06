using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Mathf;

public class Inventory : MonoBehaviour
{


    #region Fields

    public int rows;
    public int slots;
    private int emptySlots;
    private float hoverYOffset;
    //position and size of inventory
    private RectTransform inventoryRect;
    public GameObject invImage;
    private float inventoryWidth, inventoryHeight;
    //private RectTransform parentInvWindow;

    //gaps between items
    public float slotPaddingLeft, slotPaddingTop;
    public float slotSize;
    private List<GameObject> allSlots;

    //FADY STUFF
    public CanvasGroup canvasGroup;//418
    private bool fadingIn;
    private bool fadingOut;
    public float fadeTime;
    //end fady stuff

    private static Inventory instance;

    //TOOLTIP
    //private static GameObject selectStackSizeStatic; //this is a solvable bug
    public GameObject frustration;
    private bool hopeIsDead = true;
    private bool isOpen; //423
    public static bool mouseInside = false;

    #endregion

    #region Properties
    public int EmptySlots
    {
        get { return emptySlots; }
        set { emptySlots = value; }
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

    public bool IsOpen { get => isOpen; set => isOpen = value; } //418
    #endregion

    #region ifTime-DebugInvisibleMoving
    public void PointerExit()
    {
        mouseInside = false;
        HideToolTip();
    }

    public void PointerEnter()
    {
        if (canvasGroup.alpha > 0)
        {
            mouseInside = true;
        }
    }

    #endregion


    // Use this for initialization
    void Start()
    {

        // canvasGroup = transform.parent.GetComponent<CanvasGroup>(); // this line to be deleted
        CreateLayout();

        InventoryManager.Instance.MovingSlot = GameObject.Find("MovingSlot").GetComponent<Slot>();
        isOpen = false; //423
    }

    // Update is called once per frame
    void Update()
    {
        //DELETY STUFF
        if (Input.GetMouseButtonUp(0))
        {
            //if (!InventoryManager.Instance.eventSystem.IsPointerOverGameObject(-1) && InventoryManager.Instance.From != null)
            if (!mouseInside && InventoryManager.Instance.From != null)//423
            {
                InventoryManager.Instance.From.ClearSlot();
                Destroy(GameObject.Find("Hover"));
                InventoryManager.Instance.To = null;
                InventoryManager.Instance.From = null;
                emptySlots++;
            }
            else if (!InventoryManager.Instance.eventSystem.IsPointerOverGameObject(-1) && !InventoryManager.Instance.MovingSlot.isEmpty)
            {
                InventoryManager.Instance.MovingSlot.ClearSlot();
                Destroy(GameObject.Find("Hover"));
            }
        }
        //DONE DELETY STUFF
        //MOUSEOVER STUFF
        if (InventoryManager.Instance.HoverObject != null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(InventoryManager.Instance.canvas.transform as RectTransform, Input.mousePosition, InventoryManager.Instance.canvas.worldCamera, out position);
            //Adds the offset to the position
            position.Set(position.x, position.y - hoverYOffset);
            InventoryManager.Instance.HoverObject.transform.position = InventoryManager.Instance.canvas.transform.TransformPoint(position);
        }
        //DONE MOUSEOVER STUFF

        if (Input.GetKeyDown("="))
        {
            frustration.SetActive(!hopeIsDead);
            hopeIsDead = !hopeIsDead;
        }
    }

    public void Open()
    {
        //if (Input.GetKeyDown("i")) //these don't need to be here, but I need you to see them to tell Arik
        //{
        if (canvasGroup.alpha > 0)
        {

            StartCoroutine(fadeOut());
            PutItemBack();
            HideToolTip();
            isOpen = false; //423
        }
        else
        {
            isOpen = true;//423
            StartCoroutine(fadeIn());
        }
        //}
    }

    public virtual void ShowToolTip(GameObject slot)
    {
        Slot tmpSlot = slot.GetComponent<Slot>();

        if (slot.GetComponentInParent<Inventory>().isOpen && !tmpSlot.isEmpty && InventoryManager.Instance.HoverObject == null && !InventoryManager.Instance.selectStackSize.activeSelf)
        {//added the "isOpen" to beginning of the above line
            InventoryManager.Instance.visualTextObject.text = tmpSlot.CurrentItem.GetToolTip();
            InventoryManager.Instance.sizeTextObject.text = InventoryManager.Instance.visualTextObject.text;

            InventoryManager.Instance.toolTipObject.SetActive(true);
            float xPos = slot.transform.position.x + slotPaddingLeft;
            float yPos = slot.transform.position.y - slot.GetComponent<RectTransform>().sizeDelta.y - slotPaddingTop;

            InventoryManager.Instance.toolTipObject.transform.position = new Vector2(xPos, yPos);
        }

    }

    public void HideToolTip()//418 We don't need the slot because the InventoryManager fixess
    {
        InventoryManager.Instance.toolTipObject.SetActive(false);
    }

    public virtual void CreateLayout()
    {
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
                GameObject newSlot = (GameObject)Instantiate(InventoryManager.Instance.slotPrefab);

                RectTransform slotRect = newSlot.GetComponent<RectTransform>();

                newSlot.name = "Slot";

                //set parent is because it's an image, and MUST be a child of th canvas
                newSlot.transform.SetParent(this.transform.parent);
                Console.WriteLine("HA!");


                ///the side padding at the inventoryRect.localPosition needs to be adjusted.
                slotRect.localPosition = inventoryRect.localPosition + new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));
                //after this point, add the slot to the script on the inv gui, then hit play for shiggles
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * InventoryManager.Instance.canvas.scaleFactor);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * InventoryManager.Instance.canvas.scaleFactor);
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
        if (item.Item.MaxSize == 1)
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
                    if (tmp.CurrentItem.Item.ItemName == item.Item.ItemName && tmp.isAvailable)
                    {
                        if (!InventoryManager.Instance.MovingSlot.isEmpty && InventoryManager.Instance.Clicked.GetComponent<Slot>() == tmp.GetComponent<Slot>())
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
        // 5/9 This on is Arik's
        CanvasGroup cg = clicked.transform.parent.GetComponent<CanvasGroup>();

        // 5/9 this one is mine
        CanvasGroup jjCG = clicked.transform.parent.parent.GetComponent<CanvasGroup>();

        //for this one, after the ||, remember Arik's only needs 2 parents, not three
        if (jjCG != null && jjCG.alpha > 0 || clicked.transform.parent.parent.parent.GetComponent<CanvasGroup>().alpha > 0) //changed
        {
            InventoryManager.Instance.Clicked = clicked;
            if (!InventoryManager.Instance.MovingSlot.isEmpty)
            {
                Slot tmp = InventoryManager.Instance.Clicked.GetComponent<Slot>();

                if (tmp.isEmpty)
                {
                    tmp.AddItems(InventoryManager.Instance.MovingSlot.Items);
                    InventoryManager.Instance.MovingSlot.Items.Clear();
                    Destroy(GameObject.Find("Hover"));
                }
                else if (!tmp.isEmpty && InventoryManager.Instance.MovingSlot.CurrentItem.Item.ItemName == tmp.CurrentItem.Item.ItemName && tmp.isAvailable)
                {
                    MergeStacks(InventoryManager.Instance.MovingSlot, tmp);
                }
            }
            //this is the item we just clicked on

            //else if (InventoryManager.Instance.From == null && canvasGroup.alpha == 1 && !Input.GetKey(KeyCode.LeftShift))//423
            else if (InventoryManager.Instance.From == null && clicked.transform.parent.GetComponent<Inventory>().isOpen && !Input.GetKey(KeyCode.LeftShift))
            {

                if (!clicked.GetComponent<Slot>().isEmpty && !GameObject.Find("Hover"))
                {
                    InventoryManager.Instance.From = clicked.GetComponent<Slot>();
                    //InventoryManager.Instance.From.GetComponent<Image>().color = Color.grey;
                    //make sure using UnityEngine.UI;

                    CreateHoverIcon();
                }
            }

            else if (InventoryManager.Instance.To == null && !Input.GetKey(KeyCode.LeftShift))
            {

                InventoryManager.Instance.To = clicked.GetComponent<Slot>();
                Destroy(GameObject.Find("Hover"));
            }
            if (InventoryManager.Instance.To != null && InventoryManager.Instance.From != null)
            {
                if (!InventoryManager.Instance.To.isEmpty && InventoryManager.Instance.From.CurrentItem.Item.ItemName == InventoryManager.Instance.To.CurrentItem.Item.ItemName && InventoryManager.Instance.To.isAvailable)
                {
                    MergeStacks(InventoryManager.Instance.From, InventoryManager.Instance.To);
                }
                else
                {
                    // everything that was here is now in Slot.cs
                    Slot.SwapItems(InventoryManager.Instance.From, InventoryManager.Instance.To);
                }


                //InventoryManager.Instance.From.GetComponent<Image>().color = Color.clear;
                InventoryManager.Instance.To = null;
                InventoryManager.Instance.From = null;
                //MOUSEOVER STUFF
                Destroy(GameObject.Find("Hover"));
                //DONE MOUSEOVER STUFF
            }
        }

    }
    private void CreateHoverIcon()
    {
        //MOUSEOVER STUFF
        InventoryManager.Instance.HoverObject = (GameObject)Instantiate(InventoryManager.Instance.iconPrefab);
        InventoryManager.Instance.HoverObject.GetComponent<Image>().sprite = InventoryManager.Instance.Clicked.GetComponent<Image>().sprite;
        InventoryManager.Instance.HoverObject.name = "Hover";


        RectTransform hoverTransform = InventoryManager.Instance.HoverObject.GetComponent<RectTransform>();
        RectTransform clickedTransform = InventoryManager.Instance.Clicked.GetComponent<RectTransform>();

        hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
        hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);
        //hoverTransform.position = new Vector3(3, -3);

        InventoryManager.Instance.HoverObject.transform.SetParent(GameObject.Find("MainPlayerCanvas").transform, true);
        InventoryManager.Instance.HoverObject.transform.localScale = InventoryManager.Instance.Clicked.gameObject.transform.localScale;
        //DONE MOUSEOVER STUFF

        InventoryManager.Instance.HoverObject.transform.GetChild(0).GetComponent<Text>().text = InventoryManager.Instance.MovingSlot.Items.Count > 1 ? InventoryManager.Instance.MovingSlot.Items.Count.ToString() : string.Empty;
    }

    private void PutItemBack()
    {
        if (InventoryManager.Instance.From != null)
        {
            Destroy(GameObject.Find("Hover"));
            InventoryManager.Instance.From.GetComponent<Image>().color = Color.white;
            InventoryManager.Instance.From = null;
        }
        else if (!InventoryManager.Instance.MovingSlot.isEmpty) //If we are carrying  split stack
        {
            //Removes the hover icon
            Destroy(GameObject.Find("Hover"));

            //Puts the items back one by one
            foreach (ItemScript item in InventoryManager.Instance.MovingSlot.Items)
            {
                InventoryManager.Instance.Clicked.GetComponent<Slot>().AddItem(item);
            }

            InventoryManager.Instance.MovingSlot.ClearSlot(); //Makes sure that the moving slot is empty
        }

        //Hides the UI for splitting a stack
        InventoryManager.Instance.selectStackSize.SetActive(false);
    }

    public void SplitStack() //there are some really fucking bad issues here
    {
        InventoryManager.Instance.selectStackSize.SetActive(false); //turn off mini UI

        InventoryManager.Instance.MovingSlot.Items = InventoryManager.Instance.Clicked.GetComponent<Slot>().RemoveItems(InventoryManager.Instance.SplitAmt); //items we just took when we hit OK
        InventoryManager.Instance.From = InventoryManager.Instance.Clicked.GetComponent<Slot>();
        CreateHoverIcon();
        if (InventoryManager.Instance.SplitAmt == InventoryManager.Instance.MaxStackCount)
        {
            MoveItem(InventoryManager.Instance.Clicked);
            InventoryManager.Instance.From.ClearSlot();
        }

    }

    public void ChangeStackText(int i)
    {
        InventoryManager.Instance.SplitAmt += i;
        if (InventoryManager.Instance.SplitAmt < 0)
        {
            InventoryManager.Instance.SplitAmt = 0;
        }
        if (InventoryManager.Instance.SplitAmt > InventoryManager.Instance.MaxStackCount)
        {
            InventoryManager.Instance.SplitAmt = InventoryManager.Instance.MaxStackCount;
        }

        InventoryManager.Instance.stackText.text = InventoryManager.Instance.SplitAmt.ToString();
    }

    public void MergeStacks(Slot source, Slot destination)
    {

        int max = destination.CurrentItem.Item.MaxSize - destination.Items.Count;
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
    /// <summary>
    /// Saves the inventory and its content
    /// </summary>
    public virtual void SaveInventory()
    {
        print("i'm saving the inventory!");
        string content = string.Empty; //Creates a string for containing info about the items inside the inventory

        for (int i = 0; i < allSlots.Count; i++) //Runs through all slots in the inventory
        {
            Slot tmp = allSlots[i].GetComponent<Slot>(); //Careates a reference to the slot at the current index

            if (!tmp.isEmpty) //We only want to save the info if the slot contains an item
            {
                //Creates a string with this format: SlotIndex-ItemType-AmountOfItems; this string can be read so that we can rebuild the inventory
                content += i + "-" + tmp.CurrentItem.Item.ItemName.ToString() + "-" + tmp.Items.Count.ToString() + ";";
            }
        }

        //Stores all the info in the PlayerPrefs
        PlayerPrefs.SetString(gameObject.name + "content", content);
        PlayerPrefs.SetInt(gameObject.name + "slots", slots);
        PlayerPrefs.SetInt(gameObject.name + "rows", rows);
        PlayerPrefs.SetFloat(gameObject.name + "slotPaddingLeft", slotPaddingLeft);
        PlayerPrefs.SetFloat(gameObject.name + "slotPaddingTop", slotPaddingTop);
        PlayerPrefs.SetFloat(gameObject.name + "slotSize", slotSize);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Loads the inventory
    /// </summary>
    public virtual void LoadInventory()
    {
        //Loads all the inventory's data InventoryManager.Instance.From the playerprefs
        string content = PlayerPrefs.GetString(gameObject.name + "content");
        slots = PlayerPrefs.GetInt(gameObject.name + "slots");
        rows = PlayerPrefs.GetInt(gameObject.name + "rows");
        slotPaddingLeft = PlayerPrefs.GetFloat(gameObject.name + "slotPaddingLeft");
        slotPaddingTop = PlayerPrefs.GetFloat(gameObject.name + "slotPaddingTop");
        slotSize = PlayerPrefs.GetFloat(gameObject.name + "slotSize");

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

            string itemName = splitValues[1]; //ITEMTYPE THIS MIGHT BE FUCKED

            int amount = Int32.Parse(splitValues[2]); //Amount of items

            Item tmp = null;

            for (int i = 0; i < amount; i++) //Adds the correct amount of items to the inventory
            {
                GameObject loadedItem = Instantiate(InventoryManager.Instance.itemObject);

                if (tmp == null)
                {
                    tmp = InventoryManager.Instance.ItemContainer.Consumables.Find(item => item.ItemName == itemName);
                }
                if (tmp == null)
                {
                    tmp = InventoryManager.Instance.ItemContainer.Equipment.Find(item => item.ItemName == itemName);
                }
                if (tmp == null)
                {
                    tmp = InventoryManager.Instance.ItemContainer.Weapons.Find(item => item.ItemName == itemName);
                }

                loadedItem.AddComponent<ItemScript>();
                loadedItem.GetComponent<ItemScript>().Item = tmp;
                var test = loadedItem.GetComponent<ItemScript>();
                allSlots[index].GetComponent<Slot>().AddItem(loadedItem.GetComponent<ItemScript>());
                var wut = allSlots[index];
                Destroy(loadedItem);
            }
        }
    }
}
