using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Hover : MonoBehaviour
{
    //public Texture2D defaultCursor;
    public Texture2D handCursor;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    //public void OnMouseEnter()
    //{
    //    Cursor.SetCursor(handCursor, hotSpot, cursorMode);
    //}

    //public void OnMouseExit()
    //{
    //    Cursor.SetCursor(null, hotSpot, cursorMode);
    //}

    void Update()
    {
        checkHover();
    }

    void checkHover()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        int uicomponent_count = 0;

        bool bButtonFound = false;
        for (int i = 0; i < raycastResults.Count; i++)
        {
            if (raycastResults[i].gameObject.GetComponent<Button>() != null)
            {
                bButtonFound = true;
                break;
            }
            if (raycastResults[i].gameObject.tag == "ui_component")
                uicomponent_count++;
        }

        if (uicomponent_count == 0)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hitInfo != null && hitInfo.collider != null)
            {
                Debug.Log(hitInfo.collider.gameObject.tag);
                if (hitInfo.collider.gameObject.tag == "pixel_park_sign")
                    bButtonFound = true;
            }
        }

        if ( bButtonFound )
            Cursor.SetCursor(handCursor, hotSpot, cursorMode);
        else
            Cursor.SetCursor(null, hotSpot, cursorMode);
    }
}
