using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class ValidationManager : MonoBehaviour
{
    public static bool validateUsername(string username)
    {
        return Regex.IsMatch(username, "^[a-zA-Z0-9_]*$") && (username.Length >= 4 && username.Length <= 12);
    }

    public static bool validateEmail(string email)
    {
        bool validEmail = false;
        if (email.Length > 4 && email.Substring(email.Length - 4).ToLower() == ".com")
        {
            if (Regex.Matches(email.Substring(0, email.Length - 4), "@").Count == 1)
                validEmail = true;
        }

        return validEmail;
    }

    public static bool validatePassword(string password)
    {
        return password.Length >= 8 && !password.Contains(" ") && Regex.IsMatch(password, @"\d+");
    }

    public static bool validateDate(string date)
    {
        return Regex.IsMatch(date, @"\d?\d/\d?\d/\d\d\d\d");
        //return Regex.IsMatch(date, @"\d\d/\d\d/\d\d\d\d");
    }

    public static bool validatAge(string date)
    {
        int birthYear = int.Parse(date.Substring(date.Length - 4));
        int curYear = System.DateTime.Now.Year;
        if (birthYear + 13 > curYear)
            return false;

        return true;
    }

    public static bool validatePincode(string pincode)
    {
        if (pincode.Length < 4)
            return false;

        bool validpin = false;

        int firstnum = int.Parse(pincode.Substring(0, 1));
        int prevnum = firstnum;
        for (int i = 1; i < pincode.Length; i++)
        {
            int curnum = int.Parse(pincode.Substring(i, 1));
            if (prevnum + 1 != curnum)
            {
                validpin = true;
                break;
            }
            prevnum = curnum;
        }

        if (!validpin)
            return false;

        validpin = false;
        prevnum = firstnum;
        for (int i = 1; i < pincode.Length; i++)
        {
            int curnum = int.Parse(pincode.Substring(i, 1));
            if (prevnum - 1 != curnum)
            {
                validpin = true;
                break;
            }
            prevnum = curnum;
        }
        return validpin;
    }

    public static bool validateChatMessage(string message, bool bAllowNumber)
    {
        string pattern = "^[a-zA-Z?! ]*$";
        if (bAllowNumber)
            pattern = "^[a-zA-Z0-9?!. ]*$";

        return Regex.IsMatch(message, pattern);
    }
}
