﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    private void OnTriggerEnter(Collider collider)
    {
        print("Projectile hit " + collider.gameObject);
    }
}
