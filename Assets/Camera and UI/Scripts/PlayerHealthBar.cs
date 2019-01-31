using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class PlayerHealthBar : MonoBehaviour
{

    RawImage healthBarRawImage;
    Player player;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<Player>(); //this is looking for a script
        healthBarRawImage = GetComponent<RawImage>(); //this is the rawimage this script is attached to
    }

    // Update is called once per frame
    void Update()
    {
        float xValue = -(player.healthAsPercentage / 2f) - 0.5f;
        healthBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
    }
}
