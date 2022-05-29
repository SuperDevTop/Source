using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject loginScreen;
    public GameObject registerScreen;
    public GameObject forgotPasswordScreen;
    public GameObject serverSelection;
    public GameObject pinVerifyPopup;
    public GameObject invalidUsernamePassword;
    public GameObject invalidPin;
    public GameObject errorPopup;
    public GameObject verifyAccount;
    public GameObject loadingAfterCreate;
    public GameObject activationSuccess;
    public GameObject activationFailure;
    public GameObject emailVerification;
    public GameObject invalidUsernameEmail;
    public GameObject emailCodeVerification;
    public GameObject invalidUsernameCode;
    public GameObject changePasswordPin;
    public GameObject resetSuccessPopup;
    public GameObject registerDisabledPopup;

    [Header("Login:")]
    public InputField username;
    public InputField password;

    [Header("Error popup:")]
    public Text errorText;

    [Header("Verify Account:")]
    public InputField usernameText;
    public InputField securityCode;

    [Header("Pin Verification:")]
    public InputField pinverifyUsername;
    public InputField pinverifyPassword;
    public InputField pinverifyPincode;

    [Header("Email Verification:")]
    public InputField emailVerifyUsername;
    public InputField emailVerifyEmail;

    [Header("Email Code Verification:")]
    public InputField emailCodeVerify_Username;
    public InputField emailCodeVerify_Code;

    [Header("Change Password Pin:")]
    public InputField changePassword_Password;
    public InputField changePassword_ConfirmPassword;
    public InputField changePassword_Pincode;
    public InputField changePassword_ConfirmPin;

    public GameObject loadingScreen;


    private NetworkManager networkManager;

    void Start()
    {
        networkManager = GetComponent<NetworkManager>();
        showLoading(true);
    }

    public void onLogin()
    {
        if (username.text.Trim().Length == 0)
        {
            setErrorMessage("Please input your username.");
            return;
        }

        if (password.text.Trim().Length == 0)
        {
            setErrorMessage("Please input your password.");
            return;
        }

        networkManager.tryLogin(username.text, password.text);
    }

    public void showInvalidUsernamePassword()
    {
        invalidUsernamePassword.SetActive(true);
    }

    public void showPinVerify()
    {
        pinVerifyPopup.SetActive(true);
    }

    public void showServerSelection()
    {
        loginScreen.SetActive(false);
        serverSelection.SetActive(true);
    }

    public void setErrorMessage(string errorMessage)
    {
        errorText.text = "Whoops! Looks like there is an issue with your account!\n\n" + errorMessage;
        errorPopup.SetActive(true);
    }

    public void showVerifyAccountScreen()
    {
        serverSelection.SetActive(false);
        verifyAccount.SetActive(true);
    }

    public void onActivate()
    {
        RegisterScript register = GetComponent<RegisterScript>();

        if( usernameText.text != register.username.text )
        {
            setErrorMessage("Please input a correct username.");
            return;
        }

        if (securityCode.text.Trim().Length == 0)
        {
            setErrorMessage("Please input the security code.");
            return;
        }

        string strBirthday = register.birthday.text;
        string birthday = strBirthday.Substring(strBirthday.Length - 4) + "/" + strBirthday.Substring(0, strBirthday.Length - 5);

        networkManager.verifyAccount(register.username.text, register.password.text, register.pincode.text, 
                                                register.emailAddress.text, birthday, register.gender, register.skin, securityCode.text);
    }

    public void showLoadingAfterCreate(bool bShow)
    {
        loadingAfterCreate.SetActive(bShow);
    }

    public void showActivationSuccess()
    {
        GetComponent<LobbyManager>().username = GetComponent<RegisterScript>().username.text;
        activationSuccess.SetActive(true);
    }

    public void showActivationFailure()
    {
        activationFailure.SetActive(true);
    }

    public void onPinVerify()
    {
        networkManager.pinVerification(pinverifyUsername.text, pinverifyPassword.text, pinverifyPincode.text);
    }

    public void onPinVerificationResult(bool bSuccess)
    {
        if( bSuccess )
        {
            pinVerifyPopup.SetActive(false);
            serverSelection.SetActive(true);
            loginScreen.SetActive(false);

            GetComponent<LobbyManager>().username = pinverifyUsername.text;
        }
        else
        {
            invalidPin.SetActive(true);
        }
    }

    public void onVerifyEmail()
    {
        if (emailVerifyUsername.text.Trim().Length == 0)
        {
            setErrorMessage("Please input your username.");
            return;
        }

        if (emailVerifyEmail.text.Trim().Length == 0)
        {
            setErrorMessage("Please input your email address.");
            return;
        }

        if (!ValidationManager.validateEmail(emailVerifyEmail.text))
        {
            setErrorMessage("Please input a valid email address.");
            return;
        }
        networkManager.emailVerification(emailVerifyUsername.text, emailVerifyEmail.text);
    }

    public void onEmailVerificationResult(bool bSuccess)
    {
        if (bSuccess)
        {
            emailCodeVerification.SetActive(true);
            emailVerification.SetActive(false);
        }
        else
            invalidUsernameEmail.SetActive(true);
    }

    public void onEmailCodeVerification()
    {
        if (emailCodeVerify_Username.text.Trim().Length == 0)
        {
            setErrorMessage("Please input your username.");
            return;
        }

        //if (emailVerifyUsername.text != emailCodeVerify_Username.text )
        //{
        //    setErrorMessage("Please input a correct username.");
        //    return;
        //}

        if (emailCodeVerify_Code.text.Trim().Length != 10)
        {
            setErrorMessage("Please input the 10-digit code.");
            return;
        }

        networkManager.verifyEmailCode(emailCodeVerify_Username.text, emailCodeVerify_Code.text);
    }

    public void onVerifyEmailCodeResult(bool bSuccess)
    {
        if (bSuccess)
        {
            changePasswordPin.SetActive(true);
            emailCodeVerification.SetActive(false);
        }
        else
        {
            invalidUsernameCode.SetActive(true);
        }
    }

    public void onChangePasswordPin()
    {
        if (!ValidationManager.validatePassword(changePassword_Password.text))
        {
            setErrorMessage("Your password does not meet our requirements, please select a different one.");
            return;
        }

        if (changePassword_Password.text != changePassword_ConfirmPassword.text )
        {
            setErrorMessage("Please check your password and pin combination.");
            return;
        }

        if (!ValidationManager.validatePincode(changePassword_Pincode.text))
        {
            setErrorMessage("Your pin does not meet our requirements. It may not be consecutive.");
            return;
        }

        if (changePassword_Pincode.text != changePassword_ConfirmPin.text)
        {
            setErrorMessage("Please check your password and pin combination.");
            return;
        }

        networkManager.changePasswordPin(emailVerifyUsername.text, changePassword_Password.text, changePassword_Pincode.text);
    }

    public void onChangePasswordResult(bool bSuccess)
    {
        if (bSuccess)
        {
            //emailVerification.SetActive(false);
            changePasswordPin.SetActive(false);
            resetSuccessPopup.SetActive(true);

            GetComponent<LobbyManager>().username = emailVerifyUsername.text;
        }
        else
            setErrorMessage("Something went wrong! Please try again later.");
    }

    public void showLoading(bool bShow)
    {
        loadingScreen.SetActive(bShow);
    }

    public void onRegister()
    {
        networkManager.checkRegisterEnabled();
    }

    public void onCheckRegisterEnabled(bool bEnabled)
    {
        if (bEnabled)
        {
            loginScreen.SetActive(false);
            registerScreen.SetActive(true);
        }
        else
        {
            registerDisabledPopup.SetActive(true);
        }
    }
}
