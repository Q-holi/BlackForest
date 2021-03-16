using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject trunkBulletPrefab;
    public GameObject plantPrefab;
    public GameObject bossBulletPrefab;
    public GameObject itemCoinPrefab;


    GameObject[] trunkBullet;
    GameObject[] plantBullet;
    GameObject[] bossBullet;

    GameObject[] itemCoin;

    GameObject[] targetPool;
    void Awake()
    {
        itemCoin = new GameObject[30];

        trunkBullet = new GameObject[100];
        plantBullet = new GameObject[100];
        bossBullet = new GameObject[200];

        Generate();
    }

    void Generate()
    {
        for (int index = 0; index< trunkBullet.Length; index++)
        {
            bossBullet[index] = Instantiate(bossBulletPrefab);
            bossBullet[index].SetActive(false);
            
        }
    }

    public GameObject MakeObj(string type)
    {
        
        switch (type)
        {
            case "bossBullet":
                targetPool = bossBullet;
                break;
        }
        for (int index = 0; index < targetPool.Length; index++)
        {
            if (!targetPool[index].activeSelf)
            {
                targetPool[index].SetActive(true);
                return targetPool[index];
            }
        }

        return null;
    }
}
