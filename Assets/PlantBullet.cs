﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.tag == "Projectile")
        {
            if (collision.gameObject.tag == "Player" && collision.gameObject.layer == 9)
            {
                Destroy(gameObject);
            }
        }
    }
}
