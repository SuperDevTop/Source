using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera theCamera;
    public float xFollowSpeed_RightToLeft = 1.5f;
    public float xFollowSpeed_LeftToRight = 0.7f;
    public float yFollowSpeed = 7;
    public Vector2 followOffset;
    float camWidth;
    float camHeight;
    float zPos;
    Vector3 curPos;
    Vector3 lastPlayerPos;
    public CharacterMovementController target;
    Vector3 initPosition;
    public int ScreenWidth = 1180;
    public int cameraMovementAreaWidth = 200;

    void OnEnable()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        zPos = transform.position.z;

        //target = GameManager.instance.getMyCharacter();

        initPosition = transform.position;
        GameManager.instance.setCameraController(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        GameMap map = GameManager.instance.maps[GameManager.instance.chosenMap];

        //Debug.Log("chosenMap:" + GameManager.instance.chosenMap + ", scroll:" + map.isScrollRequired);
        if( target == null )
            target = GameManager.instance.getMyCharacter();

        // Get camera view's width and height:
     
        if (map.isScrollRequired)
        {
            if (target)
            {

                // Forget target if dead:
                lastPlayerPos = target.transform.position;
                //lastMousePos = target.mousePos;
            }

            Vector3 screenPosition = theCamera.WorldToScreenPoint(target.transform.position);

            //Debug.Log(screenPosition.x);
                // Get target and mouse position:
                Vector3 targetPos = target ? target.transform.position : lastPlayerPos;

            float xFollowSpeed = 0.0f;
            if (screenPosition.x < cameraMovementAreaWidth)
                xFollowSpeed = xFollowSpeed_RightToLeft;
            else if (screenPosition.x > (ScreenWidth - cameraMovementAreaWidth))
                xFollowSpeed = xFollowSpeed_LeftToRight;
            //Vector3 targetMousePos = target ? gm.gameStarted ? target.mousePos : target.transform.position : lastMousePos;
            //if (screenPosition.x < cameraMovementAreaWidth || screenPosition.x > (ScreenWidth - cameraMovementAreaWidth))
            //{

                Vector3 finalPos = targetPos;
                curPos.x = Mathf.Lerp(curPos.x, finalPos.x, Time.deltaTime * xFollowSpeed);
                curPos.y = Mathf.Lerp(curPos.y, finalPos.y, Time.deltaTime * yFollowSpeed);
                curPos.z = zPos;
                transform.position = curPos;



                camHeight = theCamera.orthographicSize * 2;
                camWidth = camHeight * theCamera.aspect;

            
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, map.boundOffset.x - (map.bounds.x - camWidth / 2), map.boundOffset.x + (map.bounds.x - camWidth / 2)), 
                                            0, zPos);
            //}
        }
    }

    public void initCamPosition()
    {
        transform.position = initPosition;
    }
}
