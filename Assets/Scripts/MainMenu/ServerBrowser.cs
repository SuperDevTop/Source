using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Photon.Pun;
//using Photon.Realtime;

public class ServerBrowser : MonoBehaviour
{
    public float browserRefreshRate = 3f;
    public Transform serverItemHandler;
    public ServerItem serverItemPrefab;
    public Text listStatusText;
    public string[] defaultServerName;
    public int maxPlayerCount = 250;

    RoomInfo[] rooms = new RoomInfo[0];
    RoomInfo[] lastRooms = new RoomInfo[0];

    bool isShown = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( isShown == false && GameObject.Find("ServerSelection") != null )
        {
            isShown = true;
            
            GetComponent<NetworkManager>().getOutfit(LobbyManager.instance.username);
            StartCoroutine("RefreshBrowser");
        }
    }

    IEnumerator RefreshBrowser()
    {
        while (true)
        {
            /**********************/
            // Fetch game list:
            rooms = PhotonNetwork.GetRoomList();

            // Clear UI list if room list changed:
            if (lastRooms != rooms)
            {
                foreach (Transform t in serverItemHandler)
                {
                    Destroy(t.gameObject);
                }
                lastRooms = rooms;

                if (rooms.Length > 0)
                {
                    for (int i = 0; i < rooms.Length; i++)
                    {
                        ServerItem serverItem = Instantiate(serverItemPrefab, serverItemHandler);
                        serverItem.setDetail(true, rooms[i].Name, (rooms[i].PlayerCount * 5) / maxPlayerCount);
                    }
                }

                for (int i = 0; i < defaultServerName.Length; i++)
                {
                    bool bFound = false;
                    for (int j = 0; j < rooms.Length; j++)
                    {
                        if(rooms[j].Name == defaultServerName[i])
                        {
                            bFound = true;
                            break;
                        }
                    }
                    if (!bFound)
                    {
                        ServerItem serverItem = Instantiate(serverItemPrefab, serverItemHandler);
                        serverItem.setDetail(false, defaultServerName[i], 0);
                    }
                }
            }
            /***************************/

            // Wait for refresh rate before repeating:
            yield return new WaitForSecondsRealtime(browserRefreshRate);
        }
    }
}
