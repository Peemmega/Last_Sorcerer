using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using UnityEngine.SocialPlatforms;
using static UnityEditor.Progress;

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

    static GameObject RandomItem(List<RateDrop> items)
    {
        float counter = 0;
        
        foreach (var i in items)
        {
            counter += i.rate;
        }

        float chosen = UnityEngine.Random.Range(0, counter);

        foreach (var i in items)
        {
            counter -= i.rate;
            if (chosen > counter)
            {
                return i.item;
            } 
        }
        return null;
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

            GameObject item = RandomItem(spawnLists[preset].items);

            if (item)
            {
               // Debug.Log(item.name);
                SpawnItem(item, pos.transform);
            } else
            {
                //Debug.Log("Nothing");
            }
        }
    }
}
