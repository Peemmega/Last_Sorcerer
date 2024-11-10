using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDetect : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera cam;
    private GameObject lastTarget;

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mousePos);
        
        RaycastHit hit;
        LayerMask Mark = LayerMask.GetMask("Enemy","Item"); 
        bool targetItem = Physics.Raycast(ray, out hit,100, Mark);

        if (targetItem)
        {                   
            GameObject infoUI = hit.transform.Find("infoUI").gameObject;
            if (infoUI != null)
            {
                if (lastTarget != hit.transform.gameObject)
                {
                    CloseUI(lastTarget);
                    //Debug.Log(hit.transform.name);
                }

                lastTarget = hit.transform.gameObject;

                PickUpController pickUpController = lastTarget.GetComponent<PickUpController>();
                if (hit.transform.tag == "Monster")
                {
                    infoUI.SetActive(true);
                } else
                {
                    infoUI.SetActive((pickUpController != null && lastTarget.transform.parent == GameObject.Find("Items").transform) || (pickUpController == null));
                }
            }
        }
        else
        {
            CloseUI(lastTarget);
        }

        static void CloseUI(GameObject lastTarget)
        {
            if (lastTarget != null)
            {
                GameObject infoUI = lastTarget.transform.Find("infoUI").gameObject;
                if (infoUI != null)
                {
                    infoUI.SetActive(false);
                    lastTarget = null;
                }
            }
        }
    }
}
