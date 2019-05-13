using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType { CONSUMABLE, MAINHAND, TWOHAND, OFFHAND, HEAD, NECK, CHEST, RING, BRACERS, LEGS, BOOTS, TRINKETS, GENERIC, GENERICWEAPON }
public enum Quality {  WEIRD, AWESOME, RARE, GLOWY };

public class ItemScript : MonoBehaviour {


    public Sprite spriteNeutral;
    public Sprite spriteHighlighted;

    private Item item;

    //public Item Item { get => item; set => item = value; } //part one

    public Item Item //part 2
    {
        get { return item; }
        set
        {

            item = value;
            spriteHighlighted = Resources.Load<Sprite>(value.SpriteHighlighted);
            spriteNeutral = Resources.Load<Sprite>(value.SpriteNeutral);

        }
    }

    //public void Start()
    //{
    //    player = FindObjectOfType<Player>();
    //}
    public void Use(Slot slot)
    {
        item.Use(slot, this);

        /*switch (type)
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
        }*/
    }
    
    //TOOLTIP
    ///summary
    /// create item for tooltip
    ///summary
    public string GetToolTip()
    {
        return item.GetTooltip();
        
    }
}

    