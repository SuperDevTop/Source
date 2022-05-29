using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RegisterScript : MonoBehaviour
{
    public CharacterCustomization[] avatars;
    public Button createAccountButton;

    public InputField username;
    public InputField password;
    public InputField confirmPassword;
    public InputField pincode;
    public InputField confirmPincode;
    public InputField emailAddress;
    public InputField birthday;

    [HideInInspector] public int skin = 0;
    [HideInInspector] public int gender = 0; //0: male, 1: female

    UIManager uiManager;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onSkinChanged(int value)
    {
        skin = value;
        //avatarImages[0].sprite = maleAvatars[value];
        //avatarImages[1].sprite = femaleAvatars[value];
        if( avatars!= null )
        {
            for( int i = 0; i < avatars.Length; i ++ )
                avatars[i].changeSkin(value);
        }
    }

    public void onGenderChanged(int value)
    {
        gender = value;
    }

    public void onAcceptTerms(bool value)
    {
        createAccountButton.interactable = value;
    }

    public void onCreateAccount()
    {
        if ( !ValidationManager.validateUsername(username.text) )
        {
            uiManager.setErrorMessage("Your username does not meet our requirements, please select a different one.");
            return;
        }

        if (!ValidationManager.validatePassword(password.text))
        {
            uiManager.setErrorMessage("Your password does not meet our requirements, please select a different one.");
            return;
        }

        if ( password.text != confirmPassword.text )
        {
            uiManager.setErrorMessage("Your passwords do not match, please try again.");
            return;
        }

        if (!ValidationManager.validatePincode(pincode.text))
        {
            uiManager.setErrorMessage("Your pin does not meet our requirements. It may not be consecutive.");
            return;
        }

        if (pincode.text != confirmPincode.text)
        {
            uiManager.setErrorMessage("Your pins do not match, please try again.");
            return;
        }

        if (emailAddress.text.Trim().Length == 0)
        {
            uiManager.setErrorMessage("Please input your email address.");
            return;
        }

       if (!ValidationManager.validateEmail(emailAddress.text))
        {
            uiManager.setErrorMessage("Please input a valid email address.");
            return;
        }

        if (birthday.text.Trim().Length == 0)
        {
            uiManager.setErrorMessage("Please input your birthday.");
            return;
        }

        if( !ValidationManager.validateDate(birthday.text) )
        {
            uiManager.setErrorMessage("Your birthday should be in the format MM/DD/YYYY. Please try again");
            return;
        }

        if (!ValidationManager.validatAge(birthday.text))
        {
            uiManager.setErrorMessage("You must be at least 13 years old to join.");
            return;
        }

        GetComponent<NetworkManager>().createAccount(username.text, emailAddress.text);
    }

}
