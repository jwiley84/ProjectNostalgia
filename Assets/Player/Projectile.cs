using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField] float damageCasued = 10f;

    private void OnTriggerEnter(Collider collider)
    {

        print("Projectile hit " + collider.gameObject);

        //this portion after github push. I"m going to do github over and over and over until Arik gets used to it
        Component damagabeComponent = collider.gameObject.GetComponent(typeof(IDamagable));
        print("Component = " + damagabeComponent);
        if (damagabeComponent)
        {
            (damagabeComponent as IDamagable).TakeDamage(damageCasued);
        }
    }
}
