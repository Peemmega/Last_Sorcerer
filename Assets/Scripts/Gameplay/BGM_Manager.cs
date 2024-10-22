using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_Manager : MonoBehaviour
{
    public AudioSource track01, track02;

    public void Swap()
    {
        float rate = Time.deltaTime * 0.01f;

        if (track01.volume == 0.5f)
        {
            for (float i = 0.5f; track01.volume > 0; i -= rate)
            {
                track01.volume = i;
                track02.volume += rate;
            }
            track01.volume = 0;
            track02.volume = 0.5f;
        }
        else
        {
            for (float i = 0.5f; track02.volume > 0; i -= rate)
            {
                track02.volume = i;
                track01.volume += rate;
            }
            track02.volume = 0;
            track01.volume = 0.5f;
        }
    }
}
