using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    CharacterMovementController characterControllr;
    [Header("Floating Name bar:")]
    public Transform floatingUIPanel;
    public FloatingBar floatingNameBarPrefab;
    public FloatingBar floatingNameBar_AdminPrefab;

    [Header("Change Password Pin:")]
    public InputField currentPassword;
    public InputField currentPin;
    public InputField newPassword;
    public InputField confirmNewPassword;
    public InputField newPin;
    public InputField confirmNewPin;
    public GameObject changePasswordPinSuccess;

    [Header("Error Popup:")]
    public GameObject errorPopup;
    public Text errorMessage;

    [Header("Chat Mode:")]
    public Toggle[] chatModes;
    public GameObject changeChatModeSuccess;
    public GameObject changeChatModeFailure;
    int chatMode;

    [Header("News Panel:")]
    public GameObject newsPanel;

    [Header("Chat:")]
    public InputField chatContent;
    public Transform speechBubblePanel;
    public SpeechBubble bubblePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnNameBar(CharacterMovementController cc)
    {
        characterControllr = cc;

        FloatingBar fltb;
        if((int)cc.GetOwner().CustomProperties["isAMod"] == 0)
            fltb = Instantiate(floatingNameBarPrefab, floatingUIPanel);
        else
            fltb = Instantiate(floatingNameBar_AdminPrefab, floatingUIPanel);

        fltb.owner = cc;
    }

    public void onSaveNewPasswordPin()
    {
        if (!ValidationManager.validatePassword(newPassword.text))
        {
            showErrorPopup("Your new password does not meet our requirements, please select a different one.");
            return;
        }

        if( newPassword.text != confirmNewPassword.text )
        {
            showErrorPopup("Your passwords do not match, please try again.");
            return;
        }
        if (!ValidationManager.validatePincode(newPin.text))
        {
            showErrorPopup("Your new pin does not meet our requirements. It may not be consecutive.");
            return;
        }

        if (newPin.text != confirmNewPin.text)
        {
            showErrorPopup("Your new pins do not match, please try again.");
            return;
        }

        GetComponent<NetworkManager>().changePasswordPin(PhotonNetwork.player.NickName, currentPassword.text, currentPin.text, newPassword.text, newPin.text);
    }

    void showErrorPopup(string strError)
    {
        errorMessage.text = "No changes were saved.\n\nThere was an error: " + strError
                            + "\n\nIf you are experiencing issues, please contact the support.";
        errorPopup.SetActive(true);
    }

    public void onChangePasswordResult(bool bSuccess)
    {
        if (!bSuccess)
        {
            showErrorPopup("Invalid password or pincode.");
        }
        else
        {
            changePasswordPinSuccess.SetActive(true);
        }
    }

    public void initChatMode()
    {
        GetComponent<NetworkManager>().getChatMode(PhotonNetwork.player.NickName);
    }

    public void SetChatMode(int chatMode)
    {
        if (chatMode == -1 || chatMode >= chatModes.Length)
            chatMode = 0;

        chatModes[chatMode].isOn = true;
    }

    public  void onSaveChatMode()
    {
        GetComponent<NetworkManager>().saveChatMode(PhotonNetwork.player.NickName, chatMode);
    }

    public void onChatModeChanged(int value)
    {
        chatMode = value;
    } 

    public void onSaveChatModeResult(bool bSuccess)
    {
        if (bSuccess)
            changeChatModeSuccess.SetActive(true);
        else
            changeChatModeSuccess.SetActive(false);
    }

    public void showNews()
    {
        newsPanel.SetActive(true);
    }

    public void onSendChat()
    {
        if (chatContent.text.Trim().Length == 0)
            return;

        CharacterMovementController mine = GameManager.instance.getMyCharacter();
        if( mine != null )
        {
            mine.sendChat(chatContent.text);
        }
        else
        {
            Debug.Log(" my character equals null");
        }
        chatContent.text = "";
    }

    public void onReceiveChat(string message, CharacterMovementController owner)
    {
        foreach (Transform child in speechBubblePanel)
        {
            if( child.gameObject.GetComponent<SpeechBubble>().owner == owner )
            {
                Destroy(child.gameObject);
            }
        }

        SpeechBubble speechBubble = Instantiate(bubblePrefab, speechBubblePanel);
        speechBubble.initSpeechBubble(message, owner);
    }

    string chatMsgTempVal;
    public void chatMessageValidator()
    {
        int isAMod = (int)GameManager.instance.getMyCharacter().GetOwner().CustomProperties["isAMod"];

        bool bAllowNumber = isAMod > 0;

        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && (Input.GetKey(KeyCode.V) || Input.GetKeyUp(KeyCode.V)))
        {
            Debug.Log("Paste --- Not allowed");

            //chatContent.onValueChanged.RemoveListener(chatMessageValidator);
            chatContent.text = chatMsgTempVal;
            //chatContent.onValueChanged.RemoveListener(chatMessageValidator);
        }
        else
        {
            Debug.Log("on value changed:" + chatContent.text);
            if (!ValidationManager.validateChatMessage(chatContent.text, bAllowNumber))
                chatContent.text = chatContent.text.Remove(chatContent.text.Length - 1);

            chatMsgTempVal = chatContent.text;
        }
    }
}
