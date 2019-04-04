using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Container : MonoBehaviour

{
    public Transform chestBackground;
    public Transform chest;
    public int boxHeight;

    private ContainerSystem containerSystem;

    public string containerType;

    [TextArea(5, 10)]
    public string[] contents;

    // Start is called before the first frame update
    void Start()
    {
        containerSystem = FindObjectOfType<ContainerSystem>();
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
        this.gameObject.GetComponent<Container>().enabled = true;
        FindObjectOfType<ContainerSystem>().EnterRangeOfContainer();
        if ((other.gameObject.tag == "Player") && Input.GetKeyDown(KeyCode.F))
        {
            this.gameObject.GetComponent<Container>().enabled = true;
            containerSystem.containerType = containerType;
            containerSystem.contents = contents;
            FindObjectOfType<ContainerSystem>().containerOpen();
        }
    }

    public void OnTriggerExit()
    {
        FindObjectOfType<ContainerSystem>().OutOfRange();
        this.gameObject.GetComponent<Container>().enabled = false;
    }
}
