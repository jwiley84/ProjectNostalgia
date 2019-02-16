using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class PlayerManaBar : MonoBehaviour
{

    RawImage manaBarRawImage;
    Player player;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<Player>(); //this is looking for a script
        manaBarRawImage = GetComponent<RawImage>(); //this is the rawimage this script is attached to
    }

    // Update is called once per frame
    void Update()
    {
        float xValue = -(player.manaAsPercentage / 2f) - 0.5f;
        Debug.Log(xValue);
        manaBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
    }
}
