using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingBar : MonoBehaviour
{
    public CharacterMovementController owner;

    [Space]
    public float yOffset;
    public Text playerNameText;

    void Start()
    {
        if (owner)
        {

            // Set text of name text to owner's name:
            playerNameText.text = owner.GetOwner().NickName;

            // Set name text color:
            playerNameText.color = Color.black;
        }
    }

    void Update()
    {
        if (owner)
        {

            if (owner.isVisibleObject())
            {
                playerNameText.color = Color.black;
            }
            else
                playerNameText.color = Color.clear;
        }
        else
        {
            Destroy(gameObject); // Destroy this if the owner doesn't exist anymore.
        }
    }

    void LateUpdate()
    {
        if (owner)
        {
            // Positioning:
            transform.position = Camera.main.WorldToScreenPoint(owner.transform.position + Vector3.up * yOffset);
        }
    }
}
