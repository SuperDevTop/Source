using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerItem : MonoBehaviour
{
    public GameObject isOnline;
    public Text serverNameText;
    public Toggle[] toggleButtons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setDetail(bool online, string strServerName, int rate)  // 0 <= rate < 6
    {
        isOnline.SetActive(online);
        serverNameText.text = strServerName;

        for(int i = 0; i < toggleButtons.Length; i ++ )
        {
            toggleButtons[i].isOn = rate > i;
        }
    }

    public void onServerItemClicked()
    {
        LobbyManager.instance.CreateOrJoin(serverNameText.text);
    }
}
