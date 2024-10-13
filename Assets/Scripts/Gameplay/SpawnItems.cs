using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpawnItems : MonoBehaviour
{
    [SerializeField] List<GameObject> spawnPos = new List<GameObject>();


    [Serializable]
    public class RateDrop
    {
        public GameObject item;
        public float rate;
    }


    [Serializable]
    public class SpawnList
    {
        public string name;
        public List<RateDrop> items = new List<RateDrop>();
    }

  
    public List<SpawnList> spawnLists;

    void SpawnItem(GameObject item, Transform tran)
    {
        Instantiate(item, tran.position , Quaternion.identity, GameObject.Find("Items").transform);
    }

    public void SpawnItemsOnWave() { 
        foreach (var pos in spawnPos)
        {

            int preset = 0;
            if (pos.name == "Farm")
                preset = 1;
            else if (pos.name == "WoodLand")
                preset = 2;
            else if (pos.name == "Shrine")
                preset = 3;

            GameObject item = spawnLists[preset].items[UnityEngine.Random.Range(0, (spawnLists[preset].items.Count - 1))].item;
            float rate = spawnLists[preset].items[UnityEngine.Random.Range(0, (spawnLists[preset].items.Count - 1))].rate;

            if (UnityEngine.Random.Range(0, 100) <= rate)
            {
                SpawnItem(item, pos.transform);
            }
        }
    }
}
