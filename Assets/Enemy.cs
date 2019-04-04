using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour {

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float attackRadius = 10f;
    [SerializeField] float moveRadius = 10f;
    [SerializeField] GameObject projectileToUse; //NEW
    [SerializeField] GameObject projectileSocket;
    [SerializeField] float damagePerShot = 8f;
    [SerializeField] float secondsBetweenShots = 0.9f;//Part 3

    bool isAttacking = false; //Part 3
    float currentHealthPoints;
    AICharacterControl aiCharacterControl = null;
    GameObject player = null;

    public float healthAsPercentage
    {
        get
        {
            return currentHealthPoints / maxHealthPoints;
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        aiCharacterControl = GetComponent<AICharacterControl>();
        currentHealthPoints = maxHealthPoints;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= attackRadius && !isAttacking) //&& on is Part 3
        {
            isAttacking = true;
            //print(gameObject.name + " is attacking!");
            SpawnProjectile(); //TODO slow this down
            InvokeRepeating("SpawnProjectile", 0f, secondsBetweenShots); //Part 3
        }
        if (distanceToPlayer > attackRadius) //PART 3
        {
            isAttacking = false;
            CancelInvoke();
        }
        if (distanceToPlayer <= moveRadius)
        {
            aiCharacterControl.SetTarget(player.transform);
        }
        else
        {
            aiCharacterControl.SetTarget(transform);
        }
    }

    void SpawnProjectile()
    {
        GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
        //print("WTF");
        //after putting in the above line, have arik star up his game for shiggles
        Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
        projectileComponent.damageCasued = damagePerShot;
        //ALL THIS WAS SETTING UP THE PROJECTILE AND IT'S DAMAGE
        //projectileComponent.damageCasued(damagePerShot); //Part 3

        //SETTING UP THE VECTOR TO THE PLAYER
        Vector3 unitVectorToPlayer = (player.transform.position - projectileSocket.transform.position).normalized;
        
        //SET IT'S SPEED, THEN VELOCITY BASED ON SPEED
        float projectileSpeed = projectileComponent.projectleSpeed;
        print("me " + projectileSpeed);
        newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
    }


    private void OnDrawGizmos()
    {
        //draw movement gizmos
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, moveRadius);

        //Draw AttackSphere
        Gizmos.color = new Color(255f, 0f, 0, 0.5f);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        if (currentHealthPoints <= 0) { Destroy(gameObject); } //HIHI I'M NEW
    }
}
