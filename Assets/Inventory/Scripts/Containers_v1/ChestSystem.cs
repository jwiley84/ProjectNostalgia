using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestSystem : MonoBehaviour
{
    public Text containerNameText;
    public Text contentsText;

    public GameObject openGUI;
    public RectTransform chestBoxGUI;

    public KeyCode interactInput = KeyCode.F;

    public string containerType;

    public string[] contents;

    public bool openActive = false; //this is the chest open
    public bool openEnded = false; //this is all the contents listed
    public bool outOfRange = true;

    //Container System Rework here
    private List<GameObject> allSlots;
    private int emptySlots;
    public int slots;
    public int rows;
    private float containerWidth, containerHeight;
    public float slotPaddingLeft, slotPaddingTop;
    public float slotSize;
    private RectTransform containerRect;
    public GameObject slotPrefab;
    public Canvas canvas;



    // Start is called before the first frame update
    void Start()
    {
        contentsText.text = "";
        //CreateLayout();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnterRangeOfContainer()
    {
        outOfRange = false;
        openGUI.SetActive(true);
        if (openActive == true)
        {
            openGUI.SetActive(false);
        }
    }

    public void containerOpen()
    {
        outOfRange = false;
        chestBoxGUI.gameObject.SetActive(true);  //<---------this is where I need to start fiddling with this to change to clickable inv
        containerNameText.text = containerType;
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!openActive)
            {
                openActive = true;
                StartCoroutine(OpenContainer());
            }
        }
        OpenContainer();
    }

    private IEnumerator OpenContainer()
    {
        if (outOfRange == false)
        {
            /*
            int contentAmt = contents.Length;
            int currentContentsIndex = 0;

            while (currentContentsIndex < contentAmt)
            {
                StartCoroutine(DisplayContents(contents[currentContentsIndex++]));

                if (currentContentsIndex >= contentAmt)
                {
                    openEnded = true;
                }
                yield return 0;
            }*/

            /*while (true)
            {
                if (Input.GetKeyDown(interactInput) && openEnded == false)
                {
                    break;
                }
                yield return 0;
            }*/
            print("THIS YEAH");
            CreateLayouts();
            openEnded = false;
            openActive = false;
            //DropContainer();
            yield return 0;

        }
    }

    private IEnumerator DisplayContents(string contentsToDisplay)
    {
        if (outOfRange == false)
        {
            int contentLength = contentsToDisplay.Length;
            int currentContentsIndex = 0;

            contentsText.text = "";

            while (currentContentsIndex < contentLength)
            {
                contentsText.text += contentsToDisplay[currentContentsIndex];
                currentContentsIndex++;

                if (currentContentsIndex < contentLength)
                {
                    yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    openEnded = false;
                    break;
                }
            }
            while (true)
            {
                if (Input.GetKeyDown(interactInput))
                {
                    break;
                }
                yield return 0;
            }
            openEnded = false;
            contentsText.text = "";
        }
    }

    public void DropContainer()
    {
        openGUI.SetActive(false);
        chestBoxGUI.gameObject.SetActive(false);
    }

    public void OutOfRange()
    {
        outOfRange = true;
        if (outOfRange = true)
        {
            openActive = false;
            StopAllCoroutines();
            openGUI.SetActive(false);
            chestBoxGUI.gameObject.SetActive(false);
        }
    }

    private void CreateLayout()
    {

        /* 
         * These are the notes for this section
         * 
         * I need to , when [F] (not held!)
         * Turn on the or create the chest layout, based on the type of container
         * 
         * I need to, like the inventory script, open the chest bg image,
         * child with a for loop, the amt of empty slots
         * if there is a list of items in the container list
         * 
         * change the empty slot for a full one, using the graphics from the art assets
         * Where will the art come from? 
         * Item.cs?
         */
        chestBoxGUI.gameObject.SetActive(true);
        //chestBoxGUI.gameObject.SetActive(false);
    }

    private void CreateLayouts()
    {
        chestBoxGUI.gameObject.SetActive(true);
        print("this");
        //placing slots onto inventory
        //instansiate list
        allSlots = new List<GameObject>();

        emptySlots = slots;

        //calculate width of inventory image
        containerWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;
        //calculate height
        containerHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;

        //get rectTransform
        containerRect = chestBoxGUI.GetComponent<RectTransform>();
        print(containerRect);
        //RectTransform invImageRect = invImage.GetComponent<RectTransform>();    

        containerRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, containerWidth);
        containerRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, containerHeight);
        //before moving on, drag this script onto inventory image.



        //add slots
        int columns = slots / rows;
        /*for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                //instantiate means to programatically (via code) add game object to game
                //the (GameObject) casting makes it so we can fiddle with it.
                GameObject newSlot = (GameObject)Instantiate(slotPrefab);

                RectTransform slotRect = newSlot.GetComponent<RectTransform>();

                newSlot.name = "Slotzy";

                //set parent is because it's an image, and MUST be a child of th canvas
                newSlot.transform.SetParent(this.transform.parent);


                ///the side padding at the inventoryRect.localPosition needs to be adjusted.
                slotRect.localPosition = containerRect.localPosition + new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));
                //after this point, add the slot to the script on the inv gui, then hit play for shiggles
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor);

                allSlots.Add(newSlot);
            }
        }*/

    }
}
