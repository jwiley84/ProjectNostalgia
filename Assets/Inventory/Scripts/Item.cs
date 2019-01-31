using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType { MANA, HEALTH }; 

public class Item : MonoBehaviour {

    Player player;
    public ItemType type; 
    public Sprite spriteNeutral;
    public Sprite spriteHighlighted;
    public int maxSize;

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
                player.CurrentManaPoints++;
                break;
            case ItemType.HEALTH:
                Debug.Log("U just used a health pot!");
                player.CurrentHealthPoints++;
                break;

        }
    }
    

}
