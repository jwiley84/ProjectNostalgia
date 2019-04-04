using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType { MANA, HEALTH }; 
//TOOLTIP
public enum Quality {  WEIRD, AWESOME, RARE, GLOWY };

public class ItemScript : MonoBehaviour {

    Player player;
    public ItemType type;
    //TOOLTIP
    public Quality quality;
    public float strength, intellect, agility, stamina;
    public string itemName;
    public string description;

    public Sprite spriteNeutral;
    public Sprite spriteHighlighted;
    public int maxSize;
    public int healthHeal;
    public int manaHeal;

    public void Start()
    {
        player = FindObjectOfType<Player>();
    }
    public void Use()
    {
        switch (type)
        {
            case ItemType.MANA:
                Debug.Log("U just used a mana pot!");
                if (player.CurrentManaPoints < player.MaxManaPoints)
                {
                    player.CurrentManaPoints++;
                }
                break;
            case ItemType.HEALTH:
                Debug.Log("U just used a health pot!");
                if (player.CurrentHealthPoints < player.MaxHealthPoints)
                {
                    if ((player.MaxHealthPoints - player.CurrentHealthPoints) > healthHeal)
                    {
                        player.CurrentHealthPoints += (player.MaxHealthPoints - player.CurrentHealthPoints);
                    }
                    else
                    {
                        player.CurrentHealthPoints += healthHeal;
                    }
                }
                break;
        }
    }
    
    //TOOLTIP
    ///summary
    /// create item for tooltip
    ///summary
    public string GetToolTip()
    {
        string stats = string.Empty;
        string color = string.Empty;
        string newLine = string.Empty;

        if(description != string.Empty)
        {
            newLine = "\n";
        }

        switch(quality)
        {
            case Quality.WEIRD:
                color = "magenta";
                break;
            case Quality.AWESOME:
                color = "purple";
                break;
            case Quality.RARE:
                color = "orange";
                break;
            case Quality.GLOWY:
                color = "aqua";
                break;
            default:
                break;
        }

        if (strength > 0 )
        {
            stats += "\n" + strength.ToString() + " Strength";
        }
        if (intellect > 0)
        {
            stats += "\n" + intellect.ToString() + " Intellect";
        }
        if (agility > 0)
        {
            stats += "\n" + agility.ToString() + " Agility";
        }
        if (stamina > 0)
        {
            stats += "\n" + stamina.ToString() + " Staamina";
        }

        return string.Format("<size=16><color=" + color + ">{0}</color></size><size=14><color=#add8e6ff><i>" + newLine + "{1} </i></color><color=#ffffffff>{2}</color></size>", itemName, description, stats);
    }
}

    