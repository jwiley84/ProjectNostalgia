using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable {

   
    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float currentHealthPoints = 100f;
    [SerializeField] float maxManaPoints = 100f;
    [SerializeField] float currentManaPoints = 100f;
    [SerializeField] float damage = 5f; //HI I'M NEW
    [SerializeField] float minTimeBetweenHits = 0.5f; //HI I'M NEW
    [SerializeField] float maxAttackRange = 1.5f; //HI I'M NEW
    [SerializeField] int enemyLayer = 9; //HI I'M NEW

    public Inventory inventory;
    GameObject currentTarget; //HI I'M NEW
    CameraRaycaster cameraRaycaster; //HI I'M NEW
    float _mLastHitTime = 0f; //HI I'M NEW


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

    void Start() //HI I'M NEW
    {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
    }


    void OnMouseClick(RaycastHit raycastHit, int layerHit) //HI I'M NEW
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
            inventory.AddItem(other.GetComponent<Item>());
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
    }
}
