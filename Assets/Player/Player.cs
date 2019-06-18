using System.Collections;
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
    
    [SerializeField] Projectile arrowProjectile;
    [SerializeField] Projectile spellProjectile;
    Projectile projectileInstance;

    public Inventory inventory;
    public Inventory charPanel;
    public GameObject levelChange;

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
    string attackType = string.Empty;

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

    public GameObject currentTarget;

    CameraRaycaster cameraRaycaster;

    float _mLastHitTime = 0f;
    public float currentHealthPoints;
    public bool isDed = false;
    public bool HasProjectile = false;
    private Rigidbody rb;
    bool inRange = false;

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
        rb = this.GetComponent<Rigidbody>();

    }
    void Update()//418
    {
        //REFACTORING lines 110-114
        //HandleMovement();
        if (Input.GetMouseButtonDown(0))
        {
            MoveToCursor();
            
        }
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

        }//this girl is driving me nuts
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("I hit space");
            //speed = 0;
            rb.velocity = Vector3.zero;
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
                    attackType = "1hmeleeAttack";
                    HasProjectile = false;
                    break;
                case ItemType.RANGED:
                    //case: bow:
                    //change maxAttackRange to high
                    maxAttackRange = 40f;
                    //change attack damage to low
                    damage = 200f;
                    //change speed to med
                    minTimeBetweenHits = 2f;
                    attackType = "rangedAttack";
                    HasProjectile = true;
                    break;
                case ItemType.MAGIC:
                    //case fire
                    //change maxAttackRange to med
                    maxAttackRange = 40f;
                    //change attack damage to high
                    damage = 7f;
                    //change speed to med
                    minTimeBetweenHits = 0.7f;
                    attackType = "magicAttack";
                    HasProjectile = true;
                    break;
                case ItemType.TWOHAND:
                    //case TWOHAND:
                    //change maxAttackRange to med
                    maxAttackRange = 1.5f;
                    //change attack damage to high
                    damage = 10f;
                    //change speed to med
                    minTimeBetweenHits = 1f;
                    attackType = "2hmeleeAttack";
                    HasProjectile = false;
                    break;
                default:
                    attackType = "unarmedAttack";
                    HasProjectile = false;
                    break;
            }
        }
    }
    void OnMouseClick(RaycastHit raycastHit, int layerHit)
    {
        if (layerHit == enemyLayer)
        {
            print(attackType);
            this.currentTarget = raycastHit.collider.gameObject;
            if ((raycastHit.point - transform.position).magnitude > maxAttackRange)
            {
                print("this was seen");
                MoveToCursor();
            }
            if ((currentTarget.transform.position - transform.position).magnitude < maxAttackRange)
            {
                GetComponent<PlayerMovement>().Stop();
            }
            transform.LookAt(currentTarget.transform);
            var enemyComponent = currentTarget.GetComponent<Enemy>();
            if (GetComponent<PlayerMovement>().frozen)
            {
                if (Time.time - _mLastHitTime > minTimeBetweenHits)
                {
                    GetComponent<Animator>().SetTrigger(attackType);
                    rb.freezeRotation = true;
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                    Hit();
                    _mLastHitTime = Time.time;
                }
            }
        }
    }


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
        else if (other.tag == "questObject")
        {
            StartCoroutine(tempPause());
        }
    }

    private IEnumerator tempPause()
    {
        yield return new WaitForSeconds(2);
        levelChange.GetComponent<LevelChanger>().winClick();
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
        if (currentHealthPoints <= 0)
        {
            GetComponent<Animator>().SetTrigger("die");
            var enemy = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var mob in enemy)
            {
                mob.GetComponent<Enemy>().CancelInvoke();
            }
            var extraProjectiles = GameObject.FindGameObjectsWithTag("Projectile");
            foreach (var item in extraProjectiles)
            {
                Destroy(item);
            }
            //var levelChange = GameObject.FindGameObjectWithTag("levelChanger");
            levelChange.GetComponent<LevelChanger>().onDead();
        }
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


    public int yShift = 0;
    public void LaunchProjectile(Transform target)
    {
        //print(target.name);
        if (attackType == "rangedAttack")
        {
            projectileInstance = Instantiate(arrowProjectile, new Vector3(transform.position.x, transform.position.y + yShift, transform.position.z), Quaternion.identity);

        }
        else if (attackType == "magicAttack")
        {
            projectileInstance = Instantiate(spellProjectile, new Vector3(transform.position.x, transform.position.y + yShift, transform.position.z), Quaternion.identity);
        }

        projectileInstance.SetTarget(target, damage);
    }

    void Hit()
    {
        if (currentTarget == null) { return; }
        if (HasProjectile)
        {
            LaunchProjectile(currentTarget.transform);
        }
        else
        {
            currentTarget.GetComponent<Enemy>().TakeDamage(damage);
        }

    }

    //void Shoot()
    //{
    //    Hit();
    //}

    #region newSceneMovement
    //REFACTORING
    private void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);
        if (hasHit)
        {
            GetComponent<PlayerMovement>().MoveTo(hit.point);
        }
    }

    #endregion
}
