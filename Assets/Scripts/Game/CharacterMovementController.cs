using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMovementController : MonoBehaviour
{
    public float fSpeed = 2.0f;
    public float distanceThreshold = 0.0f;

    public float yOffset = 1.0f;


    Vector2 direction, targetPosition;

    bool isVisible = true;
    bool collideWithSky = false;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.uiManager.SpawnNameBar(this);
        GameManager.instance.characterControllers.Add(this);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if ( Input.GetMouseButtonDown(0) && isMyPlayer() )
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);

            bool bUIClick = false;
            Debug.Log("raycastResultCount:" + raycastResults.Count);
            for (int i = 0; i < raycastResults.Count; i++)
            {
                if (raycastResults[i].gameObject.tag == "location" || raycastResults[i].gameObject.tag == "ui_component" )
                {
                    bUIClick = true;
                    break;
                }
                Debug.Log(raycastResults[i].gameObject.tag);
            }

            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hitInfo != null && hitInfo.collider != null )
            {
                Debug.Log(hitInfo.collider.gameObject.tag);
                if (hitInfo.collider.gameObject.tag == "pixel_park_sign")
                    bUIClick = true;
            }

            if (bUIClick)
                return;


            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //Debug.Log(Input.mousePosition + ":" + targetPosition);

            targetPosition += new Vector2(0, yOffset);

            turnDirection(targetPosition.x < transform.position.x);
            //GetComponent<PhotonView>().RPC("TurnDirection", PhotonTargets.All, targetPosition.x < transform.position.x);

            direction = new Vector2(
                        targetPosition.x - gameObject.transform.position.x,
                        targetPosition.y - gameObject.transform.position.y);

            direction.Normalize();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "sky")
            collideWithSky = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "sky")
            collideWithSky = false;
    }

    //[PunRPC]
    //void TurnDirection(bool left)
    //{
    //    int direction = left ? 1 : -1;
    //    transform.localScale = new Vector3(direction, 1, 1);
    //}

    void turnDirection(bool left)
    {
        int direction = left ? 1 : -1;
        transform.localScale = new Vector3(direction, 1, 1);
    }

    public bool isMyPlayer()
    {
        //return true;
        PhotonView photonView = GetComponent<PhotonView>();
        if (photonView != null && photonView.owner == PhotonNetwork.player)
            return true;

        return false;
    }

    void FixedUpdate()
    {
        if (!isMyPlayer())
            return;


        if ( collideWithSky && targetPosition.y > transform.position.y)
        {
            initPosition(transform.position);
            return;
        }
        //Debug.Log(Vector3.Distance(transform.position, targetPosition));
        float distance = Vector3.Distance(transform.position, targetPosition);
        //Debug.Log("Distance:" + distance + ", threshold:" + distanceThreshold);
        if ( distance > distanceThreshold)
        {
            transform.position += new Vector3(Time.deltaTime * fSpeed * direction.x, Time.deltaTime * fSpeed * direction.y, 0);
        }

    }

    public bool isVisibleObject()
    {
        return isVisible;
    }

    public PhotonPlayer GetOwner()
    {
        PhotonView photonView = GetComponent<PhotonView>();
        if (photonView != null)
            return photonView.owner;

        return null;
    }

    public void initPosition(Vector3 position)
    {
        transform.position = position;
        targetPosition = position;
    }

    public void showOrHide(bool bShow)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(bShow);
        }
        isVisible = bShow;
    }

    public void sendChat(string message)
    {
        Debug.Log("sending chat:" + message);
        GetComponent<PhotonView>().RPC("Chat", PhotonTargets.All, message);
    }

    [PunRPC]
    void Chat(string message)
    {
        Debug.Log("Pun: chat received:" + message);
        GameManager.instance.GetComponent<GameUIManager>().onReceiveChat(message, this);
    }
}
