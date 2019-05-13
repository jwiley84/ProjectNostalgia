using UnityEngine;
using System.Collections;
using System;
using System.Xml.Serialization;
using System.IO;
using Assets.Inventory.Scripts.ItemScripts;

/// <summary>
/// The item category
/// </summary>
public enum Category { CONSUMEABLE, EQUIPMENT, WEAPON }

/// <summary>
/// This script creates items
/// </summary>
public class ItemManager : MonoBehaviour
{
    public Category categeory;

    public ItemType itemType;

    public Quality quality;

    public string spriteNeutral;

    public string spriteHighlighted;

    public string itemName;

    public string description;

    public int maxSize;

    public int intellect;

    public int agility;

    public int stamina;

    public int strength;

    public float attackSpeed;

    public int health;

    public int mana;
    
    public void CreateItem()
    {
        //Creates a container for the items
        ItemContainer itemContainer = new ItemContainer();

        //Defines the item types that we can serialize
        Type[] itemTypes = { typeof(Equipment), typeof(Weapon), typeof(Consumable) };

        //Creates a file stream, so that we can read the xml file
        FileStream fs = new FileStream(Path.Combine(Application.streamingAssetsPath, "Items.xml"), FileMode.Open);

        //Creates a serializer so that we can serialize the items
        XmlSerializer serializer = new XmlSerializer(typeof(ItemContainer), itemTypes);

        //Deserializes the items into the container
        itemContainer = (ItemContainer)serializer.Deserialize(fs);

        //Serializes the stream
        serializer.Serialize(fs, itemContainer);

        //Closes the stream
        fs.Close();

        switch (categeory) //Adds the correct item to the itemContainer
        {
            case Category.EQUIPMENT:
                itemContainer.Equipment.Add(new Equipment(itemName, description, itemType, quality, spriteNeutral, spriteHighlighted, maxSize, intellect, agility, stamina, strength));
                break;
            case Category.WEAPON:
                itemContainer.Weapons.Add(new Weapon(itemName, description, itemType, quality, spriteNeutral, spriteHighlighted, maxSize, intellect, agility, stamina, strength, attackSpeed));
                break;
            case Category.CONSUMEABLE:
                itemContainer.Consumables.Add(new Consumable(itemName, description, itemType, quality, spriteNeutral, spriteHighlighted, maxSize, health, mana));
                break;
        }

        //Opens the file
        fs = new FileStream(Path.Combine(Application.streamingAssetsPath, "Items.xml"), FileMode.Create);

        //Adds the item
        serializer.Serialize(fs, itemContainer);

        fs.Close();

    }
}
