using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    #region Fields

    private Stack<ItemScript> items; // this is for keeping count
    public Text stackText; // this is to change the text on the game

    public Sprite slotEmpty; //this is to make sure we can indicate an empty slot
    public Sprite slotHighlight; //this is to fill the slot

    private CanvasGroup canvasGroup;

    public ItemType canContain; // 5/9
    #endregion


    #region Properties

    public bool isEmpty
    {
        get { return items.Count == 0; }
    }

    public ItemScript CurrentItem
    {
        get { return items.Peek(); }
    }

    public bool isAvailable
    {
        get { return CurrentItem.Item.MaxSize > items.Count; }
    }

    public Stack<ItemScript> Items
    {
        get { return items; }
        set { items = value; }
    }
    #endregion
    
    void Awake()
    {
        items = new Stack<ItemScript>(); //instansiate a new stack
    }
    
    // Use this for initialization
    void Start () {

        
        RectTransform slotRect = GetComponent<RectTransform>(); //this is to get the slot itself
        RectTransform txtRect = stackText.GetComponent<RectTransform>(); //this is to get the text object on the slot

        //scale factor for text sizing

        int txtScaleFactor = (int)(slotRect.sizeDelta.x * 0.60); //makes the lettering no bigger then 60% of the slot
        stackText.resizeTextMaxSize = txtScaleFactor; //sets the text object's max size to the factor above
        stackText.resizeTextMinSize = txtScaleFactor; //sets the text object's min size to the factor above

        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x); //resizes the text object to be the same size as the slot it's on
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);

        //418
        if (transform.parent != null)
        {
            canvasGroup = transform.parent.GetComponent<CanvasGroup>();
            //print(canvasGroup);
        }
       
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void ChangeSprite(Sprite neutral, Sprite hightlightSprite)
    {
        GetComponent<Image>().sprite = neutral; //get the neutral/empty sprite

        SpriteState st = new SpriteState(); //what is the state of the slot?

        st.highlightedSprite = hightlightSprite; //if the slot state is highlighted, use the highlighted sprite

        st.pressedSprite = neutral; //if the slot is pressed, use the neutral version
        //print("this is the sprite state");
        //print(st.ToString());
        
        GetComponent<Button>().spriteState = st; //get the button's sprites's current state and set to st

    }

    public void AddItem(ItemScript item)
    {
        items.Push(item); //add the item to the stack of them
        //Debug.Log("I hit add item");

        // TODO
        //this will need to be revised if the item is stackable, but there's only one on the stack at the moment. Fix later.

        if (items.Count >= 2)//only show the text if the stack is bigger then two
        {
            stackText.text = items.Count.ToString();
        }//REMEMBER TO GO BACK TO THIS

        //change sprite
        ChangeSprite(item.spriteNeutral, item.spriteHighlighted);
    }

    public void AddItems(Stack<ItemScript> items)
    {
        this.items = new Stack<ItemScript>(items); //this line will take the items currently in the slot and replace them with new ones
        stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;
        ChangeSprite(CurrentItem.spriteNeutral, CurrentItem.spriteHighlighted);
    }

    private void UseItem()
    {
        //takes item that occupies the slot an uses it

        if (!isEmpty)
        {
            items.Peek().Use(this);//removes from stack

            //updates text on item
            stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty; //smart if statement or one-line if statement

            if (isEmpty)
            {
                ChangeSprite(slotEmpty, slotHighlight);
                transform.parent.GetComponent<Inventory>().EmptySlots++;
                //Inventory.EmptySlots++;
            }
        }
    }

    public Stack<ItemScript> RemoveItems(int amt)
    {
        Stack<ItemScript> tmp = new Stack<ItemScript>();

        for (int i = 0; i < amt; i++)
        {
            tmp.Push(items.Pop()); //adding to our temp stack the top item from the slot
        }

        stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;
        return tmp;
    } //check

    public ItemScript RemoveItem()
    {
        ItemScript tmp;
        tmp = items.Pop();
        stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;
        return tmp;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
   
        /*if (eventData.)*/
        if (eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("Hover") && canvasGroup.alpha > 0)//418 (replaced Inventory.CanvasGroup)
        {
            UseItem();
        }
        else if (eventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftShift) && !isEmpty && !GameObject.Find("Hover"))
        {
            //print("This is the left shift");
            Vector2 position;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(InventoryManager.Instance.canvas.transform as RectTransform, Input.mousePosition, InventoryManager.Instance.canvas.worldCamera, out position);

            InventoryManager.Instance.selectStackSize.SetActive(true);

            InventoryManager.Instance.selectStackSize.transform.position = InventoryManager.Instance.canvas.transform.TransformPoint(position);

            InventoryManager.Instance.SetStackInfo(items.Count);
        }
    }
    //don't add until you get to this point on the iventory script that says to
    public void ClearSlot()
    {
        items.Clear();
        ChangeSprite(slotEmpty, slotHighlight);
        stackText.text = string.Empty;
    }

    public static void SwapItems(Slot from, Slot to)
    {
        ItemType movingType = from.CurrentItem.Item.ItemType;

        if (to != null && from != null)
        {
            if (movingType == ItemType.MAINHAND || movingType == ItemType.TWOHAND && CharacterPanel.PanelInstance.OffHandSlot.isEmpty)
            {
                movingType = ItemType.GENERICWEAPON;
            }
            if (to.canContain == ItemType.GENERIC || movingType == to.canContain)
            {
                if (movingType != ItemType.OFFHAND || CharacterPanel.PanelInstance.WeaponSlot.isEmpty || CharacterPanel.PanelInstance.WeaponSlot.CurrentItem.Item.ItemType != ItemType.TWOHAND)
                {
                    Stack<ItemScript> tmpTo = new Stack<ItemScript>(to.Items); //changed 
                    to.AddItems(from.Items); // changed

                    if (tmpTo.Count == 0) //if you move a stack to an empty slot
                    {
                        to.transform.parent.GetComponent<Inventory>().EmptySlots--;//new
                        from.ClearSlot(); //changed.
                    }
                    else
                    {
                        from.AddItems(tmpTo); //changed.
                    }
                }
            }
        }
    }
}
