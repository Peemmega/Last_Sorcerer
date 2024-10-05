using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    Vector3 RandomizeIntensity = new Vector3(0.3f, .3f, 0);
    void OnEnable()
    {
        transform.position += new Vector3(0, 2, -1);
        transform.position += new Vector3(Random.Range(-RandomizeIntensity.x, RandomizeIntensity.x),
            Random.Range(-RandomizeIntensity.y, RandomizeIntensity.y),
            Random.Range(-RandomizeIntensity.z, RandomizeIntensity.z));

        StartCoroutine(toPool());
    }

    
    IEnumerator toPool()
    {
        yield return new WaitForSeconds(0.5f);
        transform.gameObject.SetActive(false);
    }
}