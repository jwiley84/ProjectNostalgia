using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Chest : MonoBehaviour
{
    public Transform chestBackground;
    public Transform chest;
    public int boxHeight;

    private ChestSystem containerSystem;

    public string containerType;

    [TextArea(5, 10)]
    public string[] contents;

    // Start is called before the first frame update
    void Start()
    {
        containerSystem = FindObjectOfType<ChestSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(chest.position); //this is where the box will pop up
        pos.y += boxHeight;
        chestBackground.position = pos;
    }

    public void OnTriggerStay(Collider other)
    {
        this.gameObject.GetComponent<Chest>().enabled = true;
        FindObjectOfType<ChestSystem>().EnterRangeOfContainer();
        if ((other.gameObject.tag == "Player") && Input.GetKeyDown(KeyCode.F))
        {
            this.gameObject.GetComponent<Chest>().enabled = true;
            containerSystem.containerType = containerType;
            containerSystem.contents = contents;
            FindObjectOfType<ChestSystem>().containerOpen();
        }
    }

    public void OnTriggerExit()
    {
        FindObjectOfType<ChestSystem>().OutOfRange();
        this.gameObject.GetComponent<Chest>().enabled = false;
    }
}
