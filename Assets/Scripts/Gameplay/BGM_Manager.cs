using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_Manager : MonoBehaviour
{
    public AudioSource track01, track02;
    int track = 1;

    void Update()
    {
        float rate = Time.deltaTime * 0.1f;
        if (track == 2 && track01.volume > 0)
        {
            track01.volume -= rate;
            track02.volume += rate;
        }
        else if (track == 1 && track02.volume > 0)
        {
            track02.volume -= rate;
            track01.volume += rate;
        }
    }

    public void Swap()
    {
        if (track01.volume == 0.5f)
        {
            track = 2;
        }
        else
        {
            track = 1;
        }
    }
}
