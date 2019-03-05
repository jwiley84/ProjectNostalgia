using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float projectleSpeed; //NEW

    public float damageCasued; //HI I CHANGED

    //float damageCasued; //part 3

    //void SetDamage(float damage)
    //{
    //    damageCasued = damage;
    //}


    private void OnTriggerEnter(Collider collider)
    {

        //print("Projectile hit " + collider.gameObject);

        //this portion after github push. I"m going to do github over and over and over until Arik gets used to it
        Component damagabeComponent = collider.gameObject.GetComponent(typeof(IDamagable));
        if (damagabeComponent)
        {
            (damagabeComponent as IDamagable).TakeDamage(damageCasued);
        }
    }
}
