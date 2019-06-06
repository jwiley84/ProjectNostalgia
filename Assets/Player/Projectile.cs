using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    #region old Projectile Stuff
    // OLD PROJECTILE
    //public float projectleSpeed; //NEW
    //public float damageCasued; 


    //private void OnTriggerEnter(Collider collider)
    //{
    //    //this says if we enter a collider, check if it's an IDamagable one (Player)
    //    Component damagabeComponent = collider.gameObject.GetComponent(typeof(IDamagable));
    //    if (damagabeComponent)
    //    {
    //        (damagabeComponent as IDamagable).TakeDamage(damageCasued);
    //    }
    //}

    #endregion

    #region New Projectile Stuff

    [SerializeField] Transform target = null;
    [SerializeField] float projectileSpeed = 1;
    float damage;
    [SerializeField] float targetYShift;

    void Update()
    {
        if (target == null)
        {
            return;
        }

        transform.LookAt(GetAimPosition());
        transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
    }
     
    private Vector3 GetAimPosition()
    {
        CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
        if (targetCapsule == null)
        {
            return target.position;
        }
        return target.position + Vector3.up * ((targetCapsule.height / 2) - targetYShift);

    }

    public void SetTarget(Transform target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Enemy>())
        {
            other.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if(other.GetComponent<Player>())
        {
            other.GetComponent<Player>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    #endregion
}
