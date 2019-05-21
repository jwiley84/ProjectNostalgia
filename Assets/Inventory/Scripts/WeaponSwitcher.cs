using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{

    public GameObject sword; // = GameObject.Find("equippedSword");
    public GameObject bow; // = GameObject.Find("equippedBow");
    public GameObject staff; // = GameObject.Find("equippedStaff");

    //find cannot find inactive things, so turn them all on in the game, then use this to shut them off

    // Start is called before the first frame update
    void Start()
    {
        sword.SetActive(false);
        bow.SetActive(false);
        staff.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!CharacterPanel.PanelInstance.WeaponSlot.isEmpty)
        {
            ItemType weapon = CharacterPanel.PanelInstance.WeaponSlot.CurrentItem.Item.ItemType;

            switch (weapon)
            {
                case ItemType.MAINHAND:
                    sword.SetActive(true);
                    bow.SetActive(false);
                    staff.SetActive(false);
                    break;
                case ItemType.TWOHAND:
                    break;
                case ItemType.RANGED:
                    sword.SetActive(false);
                    bow.SetActive(true);
                    staff.SetActive(false);
                    break;
                case ItemType.MAGIC:
                    sword.SetActive(false);
                    bow.SetActive(false);
                    staff.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }
}
