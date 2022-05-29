using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntranceTip : MonoBehaviour
{
    public Text nameText;

    public void setName(string name)
    {
        nameText.text = name;
    }
}
