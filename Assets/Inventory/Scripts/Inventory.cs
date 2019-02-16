using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using UnityEngine.Mathf;

public class Inventory : MonoBehaviour {


    /// <summary>
    /// start these with no numbers initially
    /// </summary>
    //position and size of inventory
    private RectTransform inventoryRect;
    public GameObject invImage;
    private float inventoryWidth, inventoryHeight;
    //private RectTransform parentInvWindow;
    public int slots;
    public int rows;
    //gaps between items
    public float slotPaddingLeft, slotPaddingTop;
    public float slotSize;
    public GameObject slotPrefab;
    private static Slot from, to;
    private List<GameObject> allSlots;
    //MOUSEOVER STUFF
    public GameObject iconPrefab;
    private static GameObject hoverObject;//THIS MIGHT BE THE ISSUE
    public Canvas canvas;
    //DONE //MOUSEOVER STUFF
    //DELETY STUFF
    public EventSystem eventSystem;
    //DONE DELETY STUFF

    //FADY STUFF
    public CanvasGroup canvasGroup;
    private bool fadingIn;
    private bool fadingOut;
    public float fadeTime;
    //end fady stuff
    private static int emptySlots;

    public static int EmptySlots
    {
        get { return emptySlots; }
        set { emptySlots = value; }
    }

    // Use this for initialization
    void Start () {
        CreateLayout();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //DELETY STUFF
        if (Input.GetMouseButtonUp(0)) //0 is left button
        {
            if (!eventSystem.IsPointerOverGameObject(-1) && from != null) //mouse pointer is -1
            //so if I've picked an item, and my pointer is NOT over a game object, and i let up on the left button (above IF)
            {
                from.ClearSlot();
                Destroy(GameObject.Find("Hover"));
                to = null;
                from = null;
                hoverObject = null;
                emptySlots++;
            }
        }
        //DONE DELETY STUFF
        //MOUSEOVER STUFF
        if (hoverObject != null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
            hoverObject.transform.position = canvas.transform.TransformPoint(position);
        }
        //DONE //MOUSEOVER STUFF
        if (Input.GetKeyDown("i"))
        {
            if (canvasGroup.alpha > 0)
            {
                StartCoroutine("fadeOut");
            }
            else
            {
                StartCoroutine("fadeIn");
            }
        }
    }

    private void CreateLayout()
    {
        //placing slots onto inventory
        //instansiate list
        allSlots = new List<GameObject>();

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

                allSlots.Add(newSlot);
            }
        }

    }

    private bool PlaceEmpty(Item item) //this runs through every slot we instantiated in CreateLayout, and adds item to next avail != empty;
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

    public bool AddItem(Item item)
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
                        tmp.AddItem(item);
                        return true;
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
        //this is the item we just clicked on
        if (from == null)
        {
            if (!clicked.GetComponent<Slot>().isEmpty)
            {
                from = clicked.GetComponent<Slot>();
                //from.GetComponent<Image>().color = Color.grey;
                //make sure using UnityEngine.UI;

                //MOUSEOVER STUFF
                hoverObject = (GameObject)Instantiate(iconPrefab);
                hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
                hoverObject.name = "Hover";

                RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
                RectTransform clickedTransform = clicked.GetComponent<RectTransform>();

                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);

                hoverObject.transform.SetParent(GameObject.Find("Canvas").transform, true);
                hoverObject.transform.localScale = from.gameObject.transform.localScale;
                //DONE MOUSEOVER STUFF
            }
        }
        else if (to == null)
        {
            to = clicked.GetComponent<Slot>();
            Destroy(GameObject.Find("Hover"));
        }
        if (to != null && from != null)
        {
            Stack<Item> tmpTo = new Stack<Item>(to.Items); //pause here, go refactor slot.cs items to be public getter/setter (encapsulate AND USE field) + static
            //HIHI! I accidentally said tmpFrom. Should be tmpTo
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

            //from.GetComponent<Image>().color = Color.clear;
            to = null;
            from = null;
            //MOUSEOVER STUFF
            hoverObject = null;
            //DONE MOUSEOVER STUFF
        }
    }

    private IEnumerator fadeOut()
    {
        if (!fadingOut)
        {
            fadingOut = true;
            fadingIn = false;
            StopCoroutine("fadeIn");

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
            StopCoroutine("fadeOut");

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
