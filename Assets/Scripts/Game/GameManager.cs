using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject players;
    public Transform spawnPoint;
    public static GameManager instance;
    [HideInInspector]public GameUIManager uiManager;

    public GameMap[] maps;
    [HideInInspector]public int chosenMap = 0;
    [HideInInspector] public CameraController cameraController;

    [HideInInspector] public List<CharacterMovementController> characterControllers = new List<CharacterMovementController>();
    [HideInInspector] public Entrance clickedEntrance;

    int prevLocation = 0;

    public EntranceTip entranceTip;

    void Awake()
    {
        instance = this;
        chosenMap = 0;
        uiManager = GetComponent<GameUIManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnCharacter(); //commented for single player

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < characterControllers.Count; i++)
        {
            if (characterControllers[i] == null)
            {
                characterControllers.RemoveAt(i);
                i--;
                continue;
            }
            int targetLocation = (int)characterControllers[i].GetOwner().CustomProperties["location"];

            //Debug.Log("username:" + characterControllers[i].GetOwner().NickName + "," + characterControllers[i].GetOwner().CustomProperties);
            characterControllers[i].showOrHide(targetLocation == chosenMap);
        }
    }

    void SpawnCharacter()
    {
        //int maxSortingOrder = -300;

        //SpriteRenderer[] sprites = FindObjectsOfType(typeof(SpriteRenderer)) as SpriteRenderer[];
        //foreach(SpriteRenderer sprite in sprites)
        //{
        //    if (maxSortingOrder < sprite.sortingOrder)
        //        maxSortingOrder = sprite.sortingOrder;
        //}
        //Debug.Log("maxSortingOrder:" + maxSortingOrder);
        //maxSortingOrder++;

        PhotonPlayer player = PhotonNetwork.player;

        int gender = (int)player.CustomProperties["gender"];
        //int skin = (int)player.CustomProperties["skin"];
        //int eyes = (int)player.CustomProperties["eyes"];
        //int hair = (int)player.CustomProperties["hair"];
        //int shirt = (int)player.CustomProperties["shirt"];
        //int pants = (int)player.CustomProperties["pants"];
        //int shoes = (int)player.CustomProperties["shoes"];
        //int board = (int)player.CustomProperties["board"];
        //int hairaccessory = (int)player.CustomProperties["hairaccessory"];
        //int handaccessory = (int)player.CustomProperties["handaccessory"];
        //int outfit = (int)player.CustomProperties["outfit"];
        //int costume = (int)player.CustomProperties["costume"];

        string prefabName = "female2";
        if (gender == 0)
            prefabName = "male2";

        GameObject playerAvatar = PhotonNetwork.Instantiate(Path.Combine("Prefabs", prefabName), getSpawnPosition(), Quaternion.identity, 0);

        //if (playerAvatar != null)
        //{
        //    foreach (Transform child in playerAvatar.transform)
        //    {
        //        foreach (Transform grandchild in child)
        //        {
        //            if (grandchild.GetComponent<SpriteRenderer>())
        //            {
        //                //Debug.Log("sortingOrder:" + grandchild.GetComponent<SpriteRenderer>().sortingOrder);
        //                grandchild.GetComponent<SpriteRenderer>().sortingOrder += maxSortingOrder;
        //            }
        //        }
        //    }
        //    playerAvatar.GetComponent<CharacterCustomization>().customize(skin, eyes, hair, shirt, pants, shoes, board, hairaccessory, handaccessory, outfit, costume);
        //}

        ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
        h.Add("location", 0);
        PhotonNetwork.player.SetCustomProperties(h);
    }

    public void changeLocation(int location)
    {
        Debug.Log("ChangeLocaion:" + location);
        prevLocation = chosenMap;
        chosenMap = location;

        for (int i = 0; i < maps.Length; i++)
            maps[i].gameObject.SetActive(false);

        maps[chosenMap].gameObject.SetActive(true);

        CharacterMovementController myCharacter = getMyCharacter();
        if (myCharacter != null)
            myCharacter.initPosition(getSpawnPosition());

        if (cameraController != null)
            cameraController.initCamPosition();

        ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
        h.Add("location", chosenMap);
        PhotonNetwork.player.SetCustomProperties(h);
        //PhotonNetwork.player.CustomProperties["location"] = chosenMap;

    }

    public CharacterMovementController getMyCharacter()
    {
        if (characterControllers == null)
            return null;

        for( int i = 0; i < characterControllers.Count; i ++ )
        {
            if (characterControllers[i].isMyPlayer())
                return characterControllers[i];
        }
        return null;
    }

    public void setCameraController(CameraController cc)
    {
        cameraController = cc;
    }

    Vector3 getSpawnPosition()
    {
        int index = 0;
        for( int i = 0; i < maps[chosenMap].prevLocations.Length; i ++ )
        {
            if( maps[chosenMap].prevLocations[i] == prevLocation )
            {
                index = i;
                break;
            }
        }
        return maps[chosenMap].spawnPositions[index].position;
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Debug.Log("Other player was disconnected!");

        GameObject[] players = GameObject.FindGameObjectsWithTag("player");
        if (players != null)
        {
            foreach (GameObject player in players)
            {
                Debug.Log("OwnerID:" + player.GetComponent<PhotonView>().ownerId);
                if (player.GetComponent<PhotonView>().ownerId == otherPlayer.ID)
                {
                    PhotonNetwork.Destroy(player);
                }
            }
        }
    }

    public void InitEntranceTip(string entranceName)
    {
        GameObject[] tips = GameObject.FindGameObjectsWithTag("tip");
        if( tips != null && tips.Length > 0 )
        {
            entranceTip.gameObject.SetActive(false);
            return;
        }

        entranceTip.setName(entranceName);
        Canvas.ForceUpdateCanvases();
        entranceTip.GetComponent<HorizontalLayoutGroup>().enabled = false;
        entranceTip.GetComponent<HorizontalLayoutGroup>().enabled = true;

        entranceTip.gameObject.SetActive(true);
        entranceTip.gameObject.transform.position = Input.mousePosition + new Vector3(0, 30, 0);
    }

    public void HideEntranceTip()
    {
        entranceTip.gameObject.SetActive(false);
    }
}
