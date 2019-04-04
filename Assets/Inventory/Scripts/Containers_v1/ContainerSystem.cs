using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerSystem : MonoBehaviour
{
    public Text containerNameText;
    public Text contentsText;

    public GameObject openGUI;
    public Transform chestBoxGUI;

    public KeyCode interactInput = KeyCode.F;

    public string containerType;

    public string[] contents;

    public bool openActive = false; //this is the chest open
    public bool openEnded = false; //this is all the contents listed
    public bool outOfRange = true;

    // Start is called before the first frame update
    void Start()
    {
        contentsText.text = "";
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
            }

            while (true)
            {
                if (Input.GetKeyDown(interactInput) && openEnded == false)
                {
                    break;
                }
                yield return 0;
            }

            openEnded = false;
            openActive = false;
            DropContainer();
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
}
