using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySFX : MonoBehaviour
{    void Start()
    {
        //SFX
        GameObject sfx = new GameObject("sfx");
        sfx.transform.parent = GameObject.Find("SFX").transform;
        sfx.AddComponent<AudioSource>();
        sfx.GetComponent<AudioSource>().clip = this.GetComponent<AudioSource>().clip;
        sfx.GetComponent<AudioSource>().pitch = Random.Range(0.7f, 1.3f);
        sfx.GetComponent<AudioSource>().Play();
        Destroy(sfx, 3f);
    }
}
