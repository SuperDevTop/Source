using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;

    public int ServerLimit = 250;

    int multiplayerSceneIndex = 1;
    string gameVersion = "0.1";
    bool bConnected = false;
    string strServername;

    [HideInInspector]
    public string username;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(gameVersion);
    }

    void OnConnectedToMaster()
    {
        Debug.Log("We are now connected to the " + PhotonNetwork.CloudRegion + " server!");

        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.JoinLobby();
    }

    // Update is called once per frame
    void Update()
    {
        if( PhotonNetwork.connectedAndReady && bConnected == false )
        {
            bConnected = true;
            GetComponent<UIManager>().loginScreen.SetActive(true);
            GetComponent<UIManager>().showLoading(false);

            if( PhotonNetwork.FindFriends(new string[] { "TestUser" }) )
            {
                Debug.Log("Friends:" + PhotonNetwork.Friends);
            }
            else
                Debug.Log("Get Friends failed");
        }
    }

    public void CreateOrJoin(string servername)
    {
        strServername = servername;

        Debug.Log("trying to create a game server:" + servername + ", username:" + username);

        PhotonNetwork.playerName = username;

        PhotonNetwork.CreateRoom(strServername, new RoomOptions()
        {
            MaxPlayers = (byte)ServerLimit,
            IsVisible = true,
            CleanupCacheOnLeave = false,
        }, null);

        GetComponent<UIManager>().showLoading(true);
    }

    void OnPhotonCreateRoomFailed()
    {
        // Display error:
        Debug.Log("Creating a room failed.");

        PhotonNetwork.JoinRoom(strServername);
    }

    void OnPhotonJoinRoomFailed()
    {
        Debug.Log("Join room failed.");
        GetComponent<UIManager>().showLoading(false);
    }

    void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        GetComponent<UIManager>().showLoading(false);
        StartGame();
    }

    private void StartGame()
    {
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("Starting Game");
            PhotonNetwork.LoadLevel(multiplayerSceneIndex);
        }
    }
}
