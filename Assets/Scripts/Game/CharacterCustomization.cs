using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCustomization : MonoBehaviour
{
    public GameObject[] skins;
    public GameObject[] eyes;
    public GameObject[] hair;
    public GameObject[] shirts;
    public GameObject[] pants;
    public GameObject[] shoes;
    public GameObject[] hoverboards;
    public GameObject[] costumes;
    public GameObject[] outfits;
    public GameObject[] hairAccessories;
    public GameObject[] handAccessories;

    int orderInLayerOffset = 280;
    //public Text usernameText;

    public void changeSkin(int skin)
    {
        if( skins != null )
        {
            for( int i = 0; i < skins.Length; i ++ )
            {
                skins[i].SetActive(i == skin);
            }
        }
    }



    public void customize(int skinIndex, int eyesIndex, int hairIndex, int shirtIndex, int pantsIndex, int shoesIndex, int boardIndex, 
                    int hairAccessoryIndex, int handAccessoryIndex, int outfitIndex, int costumeIndex)
    {
        if (skins != null)
        {
            for (int i = 0; i < skins.Length; i++)
            {
                skins[i].SetActive(i == skinIndex);
            }
        }

        if (eyes != null)
        {
            for (int i = 0; i < eyes.Length; i++)
            {
                eyes[i].SetActive(i == eyesIndex);
            }
        }

        if (hair != null)
        {
            for (int i = 0; i < hair.Length; i++)
            {
                hair[i].SetActive(i == hairIndex && costumeIndex == -1);
            }
        }

        if (shirts != null)
        {
            for (int i = 0; i < shirts.Length; i++)
            {
                shirts[i].SetActive(i == shirtIndex && costumeIndex == -1 && outfitIndex == -1);
            }
        }

        if (pants != null)
        {
            for (int i = 0; i < pants.Length; i++)
            {
                pants[i].SetActive(i == pantsIndex && costumeIndex == -1 && outfitIndex == -1);
            }
        }

        if (shoes != null)
        {
            for (int i = 0; i < shoes.Length; i++)
            {
                shoes[i].SetActive(i == shoesIndex && costumeIndex == -1);
            }
        }

        if (hoverboards != null)
        {
            for (int i = 0; i < hoverboards.Length; i++)
            {
                hoverboards[i].SetActive(i == boardIndex);
            }
        }

        if (costumes != null)
        {
            for (int i = 0; i < costumes.Length; i++)
            {
                costumes[i].SetActive(i == costumeIndex);
            }
        }

        if (outfits != null)
        {
            for (int i = 0; i < outfits.Length; i++)
            {
                outfits[i].SetActive(i == outfitIndex && costumeIndex == -1);
            }
        }

        if (hairAccessories != null)
        {
            for (int i = 0; i < hairAccessories.Length; i++)
            {
                hairAccessories[i].SetActive(i == hairAccessoryIndex);
            }
        }

        if (handAccessories != null)
        {
            for (int i = 0; i < handAccessories.Length; i++)
            {
                handAccessories[i].SetActive(i == handAccessoryIndex);
            }
        }
    }

    void Start()
    {
        if (GetComponent<CharacterMovementController>() == null)
            return;

        //if (!GetComponent<CharacterMovementController>().isMyPlayer())
        {
            PhotonPlayer player = GetComponent<CharacterMovementController>().GetOwner();
            int skin = (int)player.CustomProperties["skin"];
            int eyes = (int)player.CustomProperties["eyes"];
            int hair = (int)player.CustomProperties["hair"];
            int shirt = (int)player.CustomProperties["shirt"];
            int pants = (int)player.CustomProperties["pants"];
            int shoes = (int)player.CustomProperties["shoes"];
            int board = (int)player.CustomProperties["board"];
            int hairaccessory = (int)player.CustomProperties["hairaccessory"];
            int handaccessory = (int)player.CustomProperties["handaccessory"];
            int outfit = (int)player.CustomProperties["outfit"];
            int costume = (int)player.CustomProperties["costume"];

            customize(skin, eyes, hair, shirt, pants, shoes, board, hairaccessory, handaccessory, outfit, costume);
        }

        int maxSortingOrder = -300;

        SpriteRenderer[] sprites = FindObjectsOfType(typeof(SpriteRenderer)) as SpriteRenderer[];
        foreach (SpriteRenderer sprite in sprites)
        {
            if (maxSortingOrder < sprite.sortingOrder && sprite.sortingOrder < 0)
                maxSortingOrder = sprite.sortingOrder;
        }
        Debug.Log("maxSortingOrder:" + maxSortingOrder);
        maxSortingOrder++;

        //maxSortingOrder -= orderInLayerOffset;

        foreach (Transform child in gameObject.transform)
        {
            foreach (Transform grandchild in child)
            {
                if (grandchild.GetComponent<SpriteRenderer>())
                {
                    grandchild.GetComponent<SpriteRenderer>().sortingOrder += maxSortingOrder;
                }
            }
        }
        Debug.Log("Sorting order done");

        int targetLocation = (int)GetComponent<PhotonView>().owner.CustomProperties["location"];
        GetComponent<CharacterMovementController>().showOrHide(targetLocation == GameManager.instance.chosenMap);

        

    }
}
