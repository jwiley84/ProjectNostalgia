using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{

    public Text nameText;
    public Text dialogText;

    public GameObject greetGUI;
    public Transform dialogBoxGUI;

    public float letterDelay = 0.1f;
    public float letterMultiplier = 0.5f;

    public KeyCode dialogInput = KeyCode.F;

    public string names;

    public string[] dialogLines;

    public bool letterIsMultiplied = false; 
    public bool dialogActive = false; //are you talking? (aka have you pressed f)
    public bool dialogEnded = false; //text box is open
    public bool outOfRange = true;

    //SKIP THESE
    //public AudioClip audioClip;
    //AudioSource audioSource;
    //when you do put this in, remember to put an audioClip on the DialogSystem under 'audioClip' NOT AUDIOSOURCE

    // Start is called before the first frame update
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        dialogText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterRangeOfNPC()
    {
        outOfRange = false;
        greetGUI.SetActive(true); //this is the press f sign
        if (dialogActive == true)
        {
            greetGUI.SetActive(false); //in other words, the 'press f' will stay active until 'f' is pressed
        }
    }

    public void npcChatStart()
    {
        outOfRange = false;
        dialogBoxGUI.gameObject.SetActive(true); //this is popping up the full box
        nameText.text = names;
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!dialogActive)
            {
                dialogActive = true;
                StartCoroutine(StartDialog());
            }
        }
        StartDialog();
    }

    private IEnumerator StartDialog()
    {
        if (outOfRange == false)
        {
            int dialogLength = dialogLines.Length;
            int currentDialogIndex = 0;

            while (currentDialogIndex < dialogLength || !letterIsMultiplied)
            {
                if (!letterIsMultiplied)
                {
                    letterIsMultiplied = true;
                    StartCoroutine(DisplayString(dialogLines[currentDialogIndex++]));

                    if (currentDialogIndex >= dialogLength)
                    {
                        dialogEnded = true;
                    }
                }
                yield return 0;
            }

            while (true)
            {
                if (Input.GetKeyDown(dialogInput) && dialogEnded == false)
                {
                    break;
                }
                yield return 0;
            }
            dialogEnded = false;
            dialogActive = false;
            DropDialog();
        }
    }

    private IEnumerator DisplayString(string stringToDisplay)
    {
        if (outOfRange == false)
        {
            int stringLength = stringToDisplay.Length;
            int currentCharacterIndex = 0;

            dialogText.text = "";

            while (currentCharacterIndex < stringLength)
            {
                dialogText.text += stringToDisplay[currentCharacterIndex];
                currentCharacterIndex++;

                if (currentCharacterIndex < stringLength)
                {
                    if (Input.GetKey(dialogInput))
                    {
                        yield return new WaitForSeconds(letterDelay * letterMultiplier);

                        //if (audioClip) audioSource.PlayOneShot(audioClip, 0.5f);
                    }
                    else
                    {
                        yield return new WaitForSeconds(letterDelay);

                        //if (audioClip) audioSource.PlayOneShot(audioClip, 0.5f);
                    }
                }
                else
                {
                    dialogEnded = false;
                    break;
                }
            }
            while(true)
            {
                if (Input.GetKeyDown(dialogInput))
                {
                    break;
                }
                yield return 0;
            }
            dialogEnded = false;
            letterIsMultiplied = false;
            dialogText.text = "";
        }
    }

    public void DropDialog() //if we leave the area or don't want to continue
    {
        greetGUI.SetActive(false); //make the box go away
        dialogBoxGUI.gameObject.SetActive(false);
    }

    public void OutOfRange()
    {
        outOfRange = true;
        if (outOfRange == true)
        {
            letterIsMultiplied = false;
            dialogActive = false;
            StopAllCoroutines();
            greetGUI.SetActive(false);
            dialogBoxGUI.gameObject.SetActive(false);
        }
    }

}
