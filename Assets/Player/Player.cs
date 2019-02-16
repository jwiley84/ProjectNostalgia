using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable {

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float currentHealthPoints = 100f;
    [SerializeField] float maxManaPoints = 100f;
    [SerializeField] float currentManaPoints = 100f;

    public Inventory inventory;


    public float healthAsPercentage
    {
        get { return currentHealthPoints / maxHealthPoints; }
    }

    public float manaAsPercentage
    {
        get { return currentManaPoints / maxManaPoints; }
    }

    public float CurrentManaPoints
    {
        get { return currentManaPoints; }
        set { currentManaPoints = value; }
    }

    public float CurrentHealthPoints
    {
        get { return currentHealthPoints; }
        set { currentHealthPoints = value; }
    }

    /// <summary>
    /// THIS IS THE JANKIEST POS I'VE EVER WRITTEN, SINCE IT FUNCITONS AROUND 'COLLIDING' WITH MY MANA BARREL
    /// UGH, WAIT, THIS IS THE FUNCTIONALITY I'LL NEED FOR MY HEALTH FOUNTAINS/RIVERS
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) //checking if the item we're running into has a box collider
    {
       if (other.tag == "Item")
        {
            inventory.AddItem(other.GetComponent<Item>());
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
    }
}
