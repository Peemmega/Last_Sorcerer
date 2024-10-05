using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItems : MonoBehaviour
{
    [SerializeField] List<GameObject> spawnPos = new List<GameObject>();
    [SerializeField] List<GameObject> items = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        
    }


    void SpawnItem(GameObject item, Transform tran)
    {
        Instantiate(item, tran.position , Quaternion.identity, GameObject.Find("Items").transform);
    }

    public void SpawnItemsOnWave() { 
        foreach (var pos in spawnPos)
        {
            if (Random.Range(0, 100) <= 70)
            {
                SpawnItem(items[Random.Range(0,items.Count)], pos.transform);
            }
        }
    }
}
