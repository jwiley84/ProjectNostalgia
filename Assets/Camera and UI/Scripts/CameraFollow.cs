using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    GameObject player;
    public int scootch = 15;

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
        var test = new Vector3(player.transform.position.x - scootch, 0, player.transform.position.z);
        //print("test" + test);
        transform.position = test;


    }
}
