using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    public int targetLocation;
    public string entranceName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("collision!");


        if (other.gameObject.CompareTag("player") && other.gameObject.GetComponent<CharacterMovementController>().isMyPlayer())
        {
            if (GameManager.instance.clickedEntrance == this)
            {
                GameManager.instance.HideEntranceTip();
                GameManager.instance.changeLocation(targetLocation);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("collision!");


        if (other.gameObject.CompareTag("player") && other.gameObject.GetComponent<CharacterMovementController>().isMyPlayer())
        {
            if (GameManager.instance.clickedEntrance == this)
            {
                GameManager.instance.HideEntranceTip();
                GameManager.instance.changeLocation(targetLocation);
            }
        }
    }

    void OnMouseDown()
    {
        GameObject[] uiComponents = GameObject.FindGameObjectsWithTag("ui_component");
        if (uiComponents != null && uiComponents.Length > 1)        //except chat panel
            return;

        if (gameObject.tag == "entrance")
        {
            GameObject[] tips = GameObject.FindGameObjectsWithTag("tip");
            if (tips != null && tips.Length > 0)
            {
                return;
            }

            Debug.Log("OnMouseDown" + targetLocation);
            GameManager.instance.clickedEntrance = this;
        }
        else if (gameObject.tag == "pixel_park_sign")
        {
            GameManager.instance.GetComponent<GameUIManager>().showNews();
        }
    }

    void OnMouseOver()
    {
        if (GameManager.instance.GetComponent<GameUIManager>().newsPanel.activeInHierarchy)  //added on 11/22
            return;

        if (gameObject.tag == "entrance")
            GameManager.instance.InitEntranceTip(entranceName);
    }

    void OnMouseExit()
    {
        if (gameObject.tag == "entrance")
            GameManager.instance.HideEntranceTip();
    }
}