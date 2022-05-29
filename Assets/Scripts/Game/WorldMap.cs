using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldMap : MonoBehaviour
{
    public GameObject[] tips;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkHover();
    }

    void checkHover()
    {
        GameObject[] tips = GameObject.FindGameObjectsWithTag("tip");

        foreach (GameObject tip in tips)
        {
            tip.SetActive(false);
        }

        if (GetComponent<GameUIManager>().newsPanel.activeInHierarchy)  //added on 11/22
            return;

        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        for (int i = 0; i < raycastResults.Count; i++)
        {
            if (raycastResults[i].gameObject.tag == "location")
            {
                raycastResults[i].gameObject.transform.Find("Image").gameObject.SetActive(true);
                break;
            }
        }
    }
}
