using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{

    private Stack<Item> items; // this is for keeping count
    public Text stackText; // this is to change the text on the game

    public Sprite slotEmpty; //this is to make sure we can indicate an empty slot
    public Sprite slotHighlight; //this is to fill the slot

    public bool isEmpty
    {
        get { return items.Count == 0; }
    }

    public Item currentItem
    {
        get { return items.Peek(); }
    }

    public bool isAvailable
    {
        get { return currentItem.maxSize > items.Count; }
    }

    public Stack<Item> Items
    {
        get { return items; }
        set { items = value; }
    }

    // Use this for initialization
    void Start () {

        items = new Stack<Item>(); //instansiate a new stack
        RectTransform slotRect = GetComponent<RectTransform>(); //this is to get the slot itself
        RectTransform txtRect = stackText.GetComponent<RectTransform>(); //this is to get the text object on the slot

        //scale factor for text sizing

        int txtScaleFactor = (int)(slotRect.sizeDelta.x * 0.60); //makes the lettering no bigger then 60% of the slot
        stackText.resizeTextMaxSize = txtScaleFactor; //sets the text object's max size to the factor above
        stackText.resizeTextMinSize = txtScaleFactor; //sets the text object's min size to the factor above

        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x); //resizes the text object to be the same size as the slot it's on
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
       
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

        GetComponent<Button>().spriteState = st; //get the button's sprites's current state and set to st

    }

    public void AddItem(Item item)
    {
        items.Push(item); //add the item to the stack of them


        // TODO
        //this will need to be revised if the item is stackable, but there's only one on the stack at the moment. Fix later.

        if (items.Count > 2)//only show the text if the stack is bigger then two
        {
            stackText.text = items.Count.ToString();
        }//REMEMBER TO GO BACK TO THIS

        //change sprite
        ChangeSprite(item.spriteNeutral, item.spriteHighlighted);
    }

    public void AddItems(Stack<Item> items)
    {
        this.items = new Stack<Item>(items); //this line will take the items currently in the slot and replace them with new ones
        stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;
        ChangeSprite(currentItem.spriteNeutral, currentItem.spriteHighlighted);
    }

    private void UseItem()
    {
        //takes item that occupies the slot an uses it

        if (!isEmpty)
        {
            items.Pop().Use();//removes from stack

            //updates text on item
            stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty; //smart if statement or one-line if statement

            if (isEmpty)
            {
                ChangeSprite(slotEmpty, slotHighlight);
                Inventory.EmptySlots++;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("Hover"))
        {
            UseItem();
        }
    }
    //don't add until you get to this point on the iventory script that says to
    public void ClearSlot()
    {
        items.Clear();
        ChangeSprite(slotEmpty, slotHighlight);
        stackText.text = string.Empty;
    }
}
