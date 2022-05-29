using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boomlagoon.JSON;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    public string URL = "http://localhost/PixelPark/public/index.php/";
    UIManager uiManager;

    float pingDelay = 2.0f;
    string multipleLogin = "\"multiple_login\"";

    void Start()
    {
        uiManager = GetComponent<UIManager>();

        StartCoroutine(sendPing());
    }

    public void tryLogin(string username, string password)
    {
        if( username.Length == 0 || password.Length == 0 )
        {
            uiManager.showInvalidUsernamePassword();
            return;
        }

        StartCoroutine(Login(username, password));

        uiManager.showLoading(true);
    }

    IEnumerator Login(string username, string password)
    {
        string uri = URL + "signin?username=" + username + "&password=" + password;
        Debug.Log(uri);
        UnityWebRequest www = UnityWebRequest.Get(uri);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //JSONObject json = JSONObject.Parse(www.downloadHandler.text);
            //string result = json["result"].ToString();
            string result = www.downloadHandler.text;
            if (result == "\"success\"")
            {
                GetComponent<LobbyManager>().username = username;
                uiManager.showServerSelection();
            }
            else if (result == "\"pin verify required\"")
            {
                uiManager.showPinVerify();
            }
            else if (result == "\"failure\"")
            {
                uiManager.showInvalidUsernamePassword();
            }
            else if (result == multipleLogin)
            {
                uiManager.setErrorMessage("This account is already logged in.");
            }
        }
        uiManager.showLoading(false);
    }

    public void createAccount(string username, string emailAddress)
    {
        StartCoroutine(SendCreateAccount(username, emailAddress));
        uiManager.showLoading(true);
    }

    IEnumerator SendCreateAccount(string username, string email)
    {
        string uri = URL + "signup?username=" + username + "&email=" + email;
        Debug.Log(uri);
        UnityWebRequest www = UnityWebRequest.Get(uri);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //JSONObject json = JSONObject.Parse(www.downloadHandler.text);
            //string result = json["result"].ToString();
            string result = www.downloadHandler.text;
            if (result == "\"success\"")
            {
                uiManager.showVerifyAccountScreen();
            }
            else if (result == "\"username duplicated\"")
            {
                uiManager.setErrorMessage("Username already in use.");
            }
            else if (result == "\"email duplicated\"")
            {
                uiManager.setErrorMessage("Your email has been used too many times.");
            }
            else if (result == "\"db error\"")
            {
                uiManager.setErrorMessage("An error occured while accessing database. Please try again later.");
            }
        }
        uiManager.showLoading(false);
    }

    public void verifyAccount(string username, string password, string pincode, string email, string birthday, int gender, int skin, string securitycode)
    {
        

        StartCoroutine(SendVerifyAccount(username, password, pincode, email, birthday, gender, skin, securitycode));

        uiManager.showLoadingAfterCreate(true);
    }

    IEnumerator SendVerifyAccount(string username, string password, string pincode, string email, string birthday, int gender, int skin, string securitycode)
    {
        string uri = URL + "verifyaccount?username=" + username + "&password=" + password + "&pincode=" + pincode + "&email=" + email 
                                + "&birthday=" + birthday + "&gender=" + gender + "&skin=" + skin + "&securitycode=" + securitycode;
        Debug.Log(uri);
        UnityWebRequest www = UnityWebRequest.Get(uri);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //JSONObject json = JSONObject.Parse(www.downloadHandler.text);
            //string result = json["result"].ToString();
            uiManager.showLoadingAfterCreate(false);

            string result = www.downloadHandler.text;
            if (result == "\"success\"")
            {
                uiManager.showActivationSuccess();
            }
            else
            {
                uiManager.showActivationFailure();
            }
        }
    }

    public void pinVerification(string username, string password, string pincode)
    {
        
        StartCoroutine(SendPinVerification(username, password, pincode));
    }

    IEnumerator SendPinVerification(string username, string password, string pincode)
    {
        string uri = URL + "pinverify?username=" + username + "&password=" + password + "&pincode=" + pincode;
        Debug.Log(uri);
        UnityWebRequest www = UnityWebRequest.Get(uri);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //JSONObject json = JSONObject.Parse(www.downloadHandler.text);
            //string result = json["result"].ToString();
            uiManager.showLoadingAfterCreate(false);

            string result = www.downloadHandler.text;

            if (result == multipleLogin)
            {
                uiManager.setErrorMessage("This account is already logged in.");
            }
            else
                uiManager.onPinVerificationResult(result == "\"success\"");
        }
    }

    public void emailVerification(string username, string email)
    {
        StartCoroutine(SendEmailVerification(username, email));
        uiManager.showLoading(true);
    }

    IEnumerator SendEmailVerification(string username, string email)
    {
        string uri = URL + "emailverify?username=" + username + "&email=" + email;
        Debug.Log(uri);
        UnityWebRequest www = UnityWebRequest.Get(uri);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string result = www.downloadHandler.text;

            if (result == multipleLogin)
            {
                uiManager.setErrorMessage("This account is already logged in.");
            }
            else
                uiManager.onEmailVerificationResult(result == "\"success\"");
        }
        uiManager.showLoading(false);
    }

    public void changePasswordPin(string username, string password, string pincode)
    {
        StartCoroutine(SendChangePassword(username, password, pincode));
        uiManager.showLoading(true);
    }

    IEnumerator SendChangePassword(string username, string password, string pincode)
    {
        string uri = URL + "changepassword?username=" + username + "&password=" + password + "&pincode=" + pincode;
        Debug.Log(uri);
        UnityWebRequest www = UnityWebRequest.Get(uri);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string result = www.downloadHandler.text;
            uiManager.onChangePasswordResult(result == "\"success\"");
        }
        uiManager.showLoading(false);
    }

    public void verifyEmailCode(string username, string code)
    {
        

        StartCoroutine(SendVerifyEmailCode(username, code));
        uiManager.showLoading(true);
    }

    IEnumerator SendVerifyEmailCode(string username, string code)
    {
        string uri = URL + "verifyemailcode?username=" + username + "&code=" + code;
        Debug.Log(uri);
        UnityWebRequest www = UnityWebRequest.Get(uri);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string result = www.downloadHandler.text;
            uiManager.onVerifyEmailCodeResult(result == "\"success\"");
        }
        uiManager.showLoading(false);
    }

    public void getOutfit(string username)
    {
        StartCoroutine(GetCurrentOutfit(username));
        uiManager.showLoading(true);
    }

    IEnumerator GetCurrentOutfit(string username)
    {
        string uri = URL + "getuseroutfit?username=" + username;
        Debug.Log(uri);
        UnityWebRequest www = UnityWebRequest.Get(uri);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            JSONObject json = JSONObject.Parse(www.downloadHandler.text);
            if (json["result"].ToString().CompareTo("\"success\"") == 0)
            {
                Debug.Log(json["user"].ToString());

                string strJsonUser = json["user"].ToString();
                //JSONObject jsonUser = JSONObject.Parse(strJsonUser.Substring(1, strJsonUser.Length - 2));
                JSONObject jsonUser = JSONObject.Parse(strJsonUser);

                ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();

                h.Add("skin", int.Parse(jsonUser["SkinTone"].ToString()));
                h.Add("eyes", int.Parse(jsonUser["Eyes"].ToString()));
                h.Add("hair", int.Parse(jsonUser["Hair"].ToString()));
                h.Add("shirt", int.Parse(jsonUser["Shirt"].ToString()));
                h.Add("pants", int.Parse(jsonUser["Pants"].ToString()));
                h.Add("shoes", int.Parse(jsonUser["Shoes"].ToString()));
                h.Add("board", int.Parse(jsonUser["Board"].ToString()));
                h.Add("hairaccessory", int.Parse(jsonUser["HairAccessory"].ToString()));
                h.Add("handaccessory", int.Parse(jsonUser["HandAccessory"].ToString()));
                h.Add("outfit", int.Parse(jsonUser["Outfit"].ToString()));
                h.Add("costume", int.Parse(jsonUser["Costume"].ToString()));
                h.Add("gender", int.Parse(json["gender"].ToString()));
                h.Add("isAMod", int.Parse(json["isAMod"].ToString()));

                PhotonNetwork.player.SetCustomProperties(h);

                Debug.Log("customProperties:" + PhotonNetwork.player.CustomProperties);
            }
        }
        uiManager.showLoading(false);
    }

    public void changePasswordPin(string username, string password, string pincode, string newPassword, string newPin)
    {
        StartCoroutine(SendChangePassword(username, password, pincode, newPassword, newPin));
        //uiManager.showLoading(true);
    }

    IEnumerator SendChangePassword(string username, string password, string pincode, string newPassword, string newPin)
    {
        string uri = URL + "changepassword2?username=" + username + "&password=" + password + "&pincode=" + pincode + "&newpassword=" + newPassword + "&newpincode=" + newPin;
        Debug.Log(uri);
        UnityWebRequest www = UnityWebRequest.Get(uri);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string result = www.downloadHandler.text;
            GetComponent<GameUIManager>().onChangePasswordResult(result == "\"success\"");
        }
        //uiManager.showLoading(false);
    }

    public void getChatMode(string username)
    {
        StartCoroutine(GetChatModeRequest(username));
        //uiManager.showLoading(true);
    }

    IEnumerator GetChatModeRequest(string username)
    {
        string uri = URL + "getchatmode?username=" + username;
        Debug.Log(uri);
        UnityWebRequest www = UnityWebRequest.Get(uri);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string result = www.downloadHandler.text;
            GetComponent<GameUIManager>().SetChatMode(int.Parse(result));
        }
        //uiManager.showLoading(false);
    }

    public void saveChatMode(string username, int chatMode)
    {
        StartCoroutine(SaveChatModeRequest(username, chatMode));
        //uiManager.showLoading(true);
    }

    IEnumerator SaveChatModeRequest(string username, int chatMode)
    {
        string uri = URL + "savechatmode?username=" + username + "&chatmode=" + chatMode;
        Debug.Log(uri);
        UnityWebRequest www = UnityWebRequest.Get(uri);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string result = www.downloadHandler.text;
            GetComponent<GameUIManager>().onSaveChatModeResult(result == "\"success\"");
        }
        //uiManager.showLoading(false);
    }

    IEnumerator sendPing()
    {
        while(true)
        {
            string username;
            LobbyManager lobbyManager = GetComponent<LobbyManager>();
            if (lobbyManager != null)
                username = GetComponent<LobbyManager>().username;
            else
                username = PhotonNetwork.player.name;

            if( username.Length > 0 )
            {
                string uri = URL + "sendping?username=" + username;
                //Debug.Log(uri);
                UnityWebRequest www = UnityWebRequest.Get(uri);
                yield return www.SendWebRequest();
            }

            yield return new WaitForSeconds(pingDelay);
        }
    }

    public void checkRegisterEnabled()
    {
        StartCoroutine(isRegisterEnabled());
        uiManager.showLoading(true);
    }

    IEnumerator isRegisterEnabled()
    {
        string uri = URL + "isregisterenabled";
        Debug.Log(uri);
        UnityWebRequest www = UnityWebRequest.Get(uri);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //JSONObject json = JSONObject.Parse(www.downloadHandler.text);
            //string result = json["result"].ToString();
            string result = www.downloadHandler.text;
            uiManager.onCheckRegisterEnabled(result.ToLower() == "\"true\"");
        }
        uiManager.showLoading(false);
    }
}
