using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocationShow : MonoBehaviour
{
    public TextMeshProUGUI areaLabel;
    public string areaName;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            areaLabel.text = areaName;
        } else if (collision.CompareTag("Item") || collision.CompareTag("MeleeWeapon"))
        {
            collision.GetComponent<RemoveWhenRaid>().enabled = !((areaName == "Safe zone" && collision.transform.parent.name == "Items") || (collision.transform.parent.name != "Items"));
        }
    }
}
