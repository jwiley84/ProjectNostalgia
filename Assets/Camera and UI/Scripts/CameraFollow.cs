using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    GameObject player;

	// Use this for initialization
	void Start () {
        //TODO
        player = GameObject.FindGameObjectWithTag("Player");
        //print(player.ToString());
	}
	
	// Update is called once per frame
	void Update () {
		//TODO
	}

    void LateUpdate()
    {   //this runs after the player moves, hence LateUpdate()

        //pseudo code
        //check player x, set camera x to player x
        //check player z, set camera arm to player z
        //why are we skipping y??

        transform.position = player.transform.position;

    }
}
