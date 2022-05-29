using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    [HideInInspector]public CharacterMovementController owner;

    [Space]
    public float yOffset;
    public Text speech;
    public float ttl = 5.0f;

    float height;
    bool bInitialized;

    void Start()
    {
    }

    public void initSpeechBubble(string text, CharacterMovementController cc)
    {
        speech.text = text;

        Canvas.ForceUpdateCanvases();
        GetComponent<VerticalLayoutGroup>().enabled = false;
        GetComponent<VerticalLayoutGroup>().enabled = true;

        

        owner = cc;

        bInitialized = true;

       
        if( cc == null )
            Debug.Log("owner equals null");

        StartCoroutine(DestroyBubble());
    }

    IEnumerator DestroyBubble()
    {
        yield return new WaitForSeconds(ttl);
        Destroy(gameObject);
    }

    void Update()
    {
        if (!bInitialized)
            return;

        if (owner)
        {
            Image image = GetComponent<Image>();

            if (owner.isVisibleObject())
            {
                speech.color = Color.black;
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.8f);
            }
            else
            {
                speech.color = Color.clear;
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
            }
        }
        else
        {
            Destroy(gameObject); // Destroy this if the owner doesn't exist anymore.
        }
    }

    void LateUpdate()
    {
        if (!bInitialized)
            return;

        if (owner)
        {
            height = GetComponent<RectTransform>().sizeDelta.y;
            Debug.Log("bubble has been initialized, height=" + height);
            // Positioning:
            transform.position = Camera.main.WorldToScreenPoint(owner.transform.position + Vector3.up * yOffset) + new Vector3(0, height / 2, 0);
        }
    }
}
