using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamagable
{

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
    
    #endregion

    #region Properties
    public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

    public float manaAsPercentage { get { return currentManaPoints / maxManaPoints; } }

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

        //HandleMovement(); // this is only temporary
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
        Armed();
    }
    public void Armed()
    {
        if (!CharacterPanel.PanelInstance.WeaponSlot.isEmpty) //ATTACK!
        {
            ItemType weapon = CharacterPanel.PanelInstance.WeaponSlot.CurrentItem.Item.ItemType;

            ////do a switch case check for weapon type
            switch (weapon)
            {
                case ItemType.MAINHAND:
                    //case: sword:
                    //change maxAttackRange to low
                    maxAttackRange = 1.5f;
                    //change attack damage to med
                    damage = 5f;
                    //change speed to Low
                    minTimeBetweenHits = 0.5f;
                    break;
                case ItemType.RANGED:
                    //case: bow:
                    //change maxAttackRange to high
                    maxAttackRange = 4f;
                    //change attack damage to low
                    damage = 2f;
                    //change speed to med
                    minTimeBetweenHits = 0.2f;
                    break;
                case ItemType.MAGIC:
                    //case fire:
                    //change maxAttackRange to med
                    maxAttackRange = 2f;
                    //change attack damage to high
                    damage = 7f;
                    //change speed to med
                    minTimeBetweenHits = 0.7f;
                    break;
                case ItemType.TWOHAND:
                    //case TWOHAND:
                    //change maxAttackRange to med
                    maxAttackRange = 1.5f;
                    //change attack damage to high
                    damage = 10f;
                    //change speed to med
                    minTimeBetweenHits = 1f;
                    break;
                default:
                    break;
            }
        }
    }
    void OnMouseClick(RaycastHit raycastHit, int layerHit)
    {
        if (layerHit == enemyLayer)
        {
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
            GameObject tmp = Instantiate(InventoryManager.Instance.itemObject); // ATTACK!! We refactored this to make it smaller
            tmp.AddComponent<ItemScript>();
            ItemScript newEquipment = tmp.GetComponent<ItemScript>();

            if (other.name == "HealthCube")
            {
                int itemCount = InventoryManager.Instance.ItemContainer.Consumables.Count;
                newEquipment.Item = InventoryManager.Instance.ItemContainer.Consumables[Rando(itemCount)];
            }
            else if (other.name == "WeaponsCube")
            {
                int itemCount = InventoryManager.Instance.ItemContainer.Weapons.Count;
                newEquipment.Item = InventoryManager.Instance.ItemContainer.Weapons[Rando(itemCount)];
            }
            else if (other.name == "EquipmentCube")
            {
                int itemCount = InventoryManager.Instance.ItemContainer.Weapons.Count;
                newEquipment.Item = InventoryManager.Instance.ItemContainer.Equipment[Rando(itemCount)];
            }
            inventory.AddItem(newEquipment);
            Destroy(tmp);
        }
        else if (other.tag == "Container")
        {
            storageChest = other.GetComponent<ChestScript>().chestInventory;
        }
    }

    private int Rando(int itemCount)
    {      
        int randomItem = UnityEngine.Random.Range(0, itemCount);
        return randomItem;
    }

    private void OnTriggerExit(Collider other)
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

    #region newSceneMovement
    public float speed;

    private void HandleMovement()
    {
        float translation = speed * Time.deltaTime;
        transform.Translate(new Vector3(Input.GetAxis("Horizontal") * translation, 0, Input.GetAxis("Vertical") * translation));
    }
    #endregion
}
