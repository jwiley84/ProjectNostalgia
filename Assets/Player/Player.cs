using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamagable {

    #region Fields
    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float maxManaPoints = 100f;
    [SerializeField] float currentManaPoints = 100f;
    [SerializeField] float damage = 5f;  
    [SerializeField] float minTimeBetweenHits = 0.5f;  
    [SerializeField] float maxAttackRange = 1.5f;

    [SerializeField] int enemyLayer = 9;

    public Inventory inventory;
    public Inventory charPanel;

    public Text statsText;
    public int baseIntellect;
    public int baseAgility;
    public int baseStrength;
    public int baseStamina;
    private int intellect;
    private int agility;
    private int strength;
    private int stamina;

    private static Player instance; 

    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return Player.instance;
        }
    }

    private Inventory storageChest; //418

    GameObject currentTarget;  

    CameraRaycaster cameraRaycaster;  

    float _mLastHitTime = 0f;
    float currentHealthPoints;

    //this is where we're going to put variables to check for attack item (book, sword, bow)


    #endregion

    #region Properties
    public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; }}

    public float manaAsPercentage { get { return currentManaPoints / maxManaPoints; }}

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

    public float MaxHealthPoints
    {
        get { return maxHealthPoints; }
        set { maxHealthPoints = value; }
    }

    public float MaxManaPoints
    {
        get { return maxManaPoints; }
        set { maxManaPoints = value; }
    }
    #endregion

    #region Methods

    void Start() 
    {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
        currentHealthPoints = maxHealthPoints;
        SetStats(0, 0, 0, 0);
    }
    void Update()//418
    {
        if (Input.GetKeyDown("i"))//418
        {
            inventory.Open();
        }
        //add this after checking out the above section
        if (Input.GetKeyDown("e"))
        {
            if (storageChest != null) //why do we need this line? 
            {
                storageChest.Open();
            }
            
        }
        if (Input.GetKeyDown("c"))
        {
            if (charPanel != null) //why do we need this line? 
            {
                charPanel.Open();
            }

        }
    }

    void OnMouseClick(RaycastHit raycastHit, int layerHit) 
    {
        if (layerHit == enemyLayer)
        {
            //do a switch case check for weapon type
            //case: sword:
            //change maxAttackRange to low
            //change attack damage to med
            //change speed to Low

            //case: bow:
            //change maxAttackRange to high
            //change attack damage to low
            //change speed to med

            //case fire:
            //change maxAttackRange to med
            //change attack damage to high
            //change speed to med
            var enemy = raycastHit.collider.gameObject;
            print("huzzah! " + enemy);
            
            if ((enemy.transform.position - transform.position).magnitude > maxAttackRange)
            {
                return;
            }
            currentTarget = enemy;
            var enemyComponent = enemy.GetComponent<Enemy>();
            if (Time.time - _mLastHitTime > minTimeBetweenHits)
            {
                enemyComponent.TakeDamage(damage);
                _mLastHitTime = Time.time;
            }
            
        }
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
            ///Picks a random category, consumeable, equipment, weapons
            int randomType = UnityEngine.Random.Range(0, 3);

            //instantiates for adding to the inventory
            GameObject tmp = Instantiate(InventoryManager.Instance.itemObject);

            //Adds the item script to the object
            tmp.AddComponent<ItemScript>();

            //variable for selecting an item inside the category
            int randomItem;

            // allows access to the variables (the sprites) in itemscript
            //tmp.AddComponent<ItemScript>();

            ItemScript newEquipment = tmp.GetComponent<ItemScript>();

            switch (randomType) //Selects an item
            {
                case 0:
                    //Selects an item
                    randomItem = UnityEngine.Random.Range(0, InventoryManager.Instance.ItemContainer.Consumables.Count);

                    //Finds the item in the list
                    newEquipment.Item = InventoryManager.Instance.ItemContainer.Consumables[randomItem];
                    break;

                case 1:
                    randomItem = UnityEngine.Random.Range(0, InventoryManager.Instance.ItemContainer.Weapons.Count);
                    newEquipment.Item = InventoryManager.Instance.ItemContainer.Weapons[randomItem];
                    break;
                case 2:

                    randomItem = UnityEngine.Random.Range(0, InventoryManager.Instance.ItemContainer.Equipment.Count);
                    newEquipment.Item = InventoryManager.Instance.ItemContainer.Equipment[randomItem];
                    break;
            }

            inventory.AddItem(newEquipment);
            Destroy(tmp);
        }
       else if (other.tag == "Container") //Arik's will say 'chest'
        {
            storageChest = other.GetComponent<ChestScript>().chestInventory;  //418 yess this will yell for a bit
        }
    }

    private void OnTriggerExit(Collider other)//423? Maybe?
    {
        if (other.gameObject.tag == "Container")
        {
            if (storageChest.IsOpen)
            {
                storageChest.Open(); //this says open, and we should rename it to 'activate' or something because it also closes
            }
            storageChest = null;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        //if (currentHealthPoints <= 0) { Destroy(gameObject); } //HIHI I'M NEW
    }

    public void SetStats(int agility, int strength, int stamina, int intellect)
    {
        this.agility = agility + baseAgility;
        this.strength = strength + baseStrength;
        this.stamina = stamina + baseStamina;
        this.intellect = intellect + baseIntellect;

        statsText.text = string.Format("Stamina: {0}\nStrength: {1}\nAgility: {2}\nIntellect: {3}", stamina, strength, agility, intellect);
    }

    #endregion
}
