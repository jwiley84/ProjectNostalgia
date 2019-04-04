using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]//yeah don't do this alot
public class NPC : MonoBehaviour
{
    public Transform dialogBackground;
    public Transform npcCharacter;
    public int boxHeight;

    private DialogSystem dialogSystem;

    public string name;

    [TextArea(5, 10)]
    public string[] sentences;
    
    void Start()
    {
        dialogSystem = FindObjectOfType<DialogSystem>();    
    }
    
    void Update()
    {
        //dialogBackground.position = Camera.main.WorldToScreenPoint(npcCharacter.position + Vector3.up * 7f);
        Vector3 pos = Camera.main.WorldToScreenPoint(npcCharacter.position); //this is where the box will pop up
        pos.y += boxHeight;
        dialogBackground.position = pos;
    }

    public void OnTriggerStay(Collider other)
    {
        this.gameObject.GetComponent<NPC>().enabled = true; //turns on script (go turn it off on the NPC RIGHT NOW)
        FindObjectOfType<DialogSystem>().EnterRangeOfNPC(); //call this function in DS script
        if ((other.gameObject.tag == "Player") && Input.GetKeyDown(KeyCode.F))
        {
            this.gameObject.GetComponent<NPC>().enabled = true; //turn this on
            dialogSystem.names = name; //calls the names from DS
            dialogSystem.dialogLines = sentences; //sets the dialogs from the current sentences
            FindObjectOfType<DialogSystem>().npcChatStart(); //set name of NPC
        }
    }

    public void OnTriggerExit()
    {
        FindObjectOfType<DialogSystem>().OutOfRange();
        this.gameObject.GetComponent<NPC>().enabled = false;
    }
}
