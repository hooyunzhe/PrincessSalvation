﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayInventory : MonoBehaviour
{
    public GameObject player;
    RubyController playerObject;
    GameObject currentGO;
    Text currentGOText;
    public int numOfSpace = 3;
    public List<string> availableSpaces = new List<string>();
    public GameObject ImagePrefab;
    Image imageComponent;
    //Image offset
    private int x = 1242;
    private int y = 550;
    //Selector offset
    int selectorX = 1237;
    int selectorY = 549;

    GameObject SelectorObject;
    //Array of keycodes of numbers for testing number keys clicked
    private KeyCode[] keyCodes = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
     };

    // Start is called before the first frame update
    void Start()
    {
        SelectorObject = gameObject.transform.GetChild(0).gameObject;
        playerObject = player.GetComponent<RubyController>();
        //fill list of string for storing which collum of inventory is open
        Debug.Log("STARTT");
        for (int i = 0; i < numOfSpace; i++)
        {
            availableSpaces.Add("empty");
        }
    }

    void Update()
    {
        //Check for number keys pressed for selector
        for (int i = 0; i < numOfSpace; i++)
        {
            //check if any of the number keys are pressed
            if (Input.GetKeyDown(keyCodes[i]))
            {
                //Check if the collum selected is empty or not
                //if it's fill, the item selected will be that
                Debug.Log(availableSpaces[i]);
                Debug.Log(i);
                if (availableSpaces[i] != "empty")
                {
                    playerObject.currentSelectedItem = availableSpaces[i];
                    //Get selector image
                    GameObject SelectorObject = gameObject.transform.GetChild(0).gameObject;
                    //Move selector image
                    SelectorObject.transform.position = new Vector2(selectorX, selectorY - (i * 100));
                }
                //if it's empty, the item selected remain empty
                else
                {
                    playerObject.currentSelectedItem = "";
                    SelectorObject.transform.position = new Vector2(selectorX, selectorY - (i * 100));
                }
            }
        }
    }

    //If function called, update inventory display
    public void InventoryUpdate()
    {
        //go through the dictionary of pickable objects
        foreach (KeyValuePair<string, GameObject> elements in playerObject.pickableGameObjects)
        {
            //check if the current object exist already as a child
            //if yes, it will be a positive number
            //if no, it will be -1
            int haveChild = ContainsChild(elements.Key);

            //if current object exist as child
            if (haveChild != -1)
            {
                if (playerObject.inventoryAmount[elements.Key] == 0)
                {
                    availableSpaces[availableSpaces.IndexOf(elements.Key)] = "empty";
                    Destroy(gameObject.transform.GetChild(haveChild).gameObject);
                    continue;
                }
                //get number next to the object and update it
                currentGOText = gameObject.transform.GetChild(haveChild).GetChild(0).GetComponent<Text>();
                currentGOText.text = Convert.ToString(playerObject.inventoryAmount[elements.Key]);
                //if number equals zero, delete object on display and set it's current space to "empty"
                if (currentGOText.text == "0")
                {
                    Debug.Log("IT SHOULD NOT COME UP BUT YES HERE");
                    availableSpaces[haveChild - 1] = "empty";
                    Destroy(gameObject.transform.GetChild(haveChild).gameObject);
                }
            }
            //else if object does not exist as child
            else
            {
                if (playerObject.inventoryAmount[elements.Key] == 0)
                {
                    Debug.Log("if it's zero");
                    continue;
                }
                //go through all collums
                for (int b = 0; b < numOfSpace; b++)
                {
                    //check for the first one that is empty
                    if (availableSpaces[b] == "empty")
                    {
                        Debug.Log(b);
                        //create current game object
                        currentGO = Instantiate(ImagePrefab, new Vector2(x, y - (b * 100)), Quaternion.identity, gameObject.transform);
                        currentGO.name = elements.Value.name;
                        imageComponent = currentGO.GetComponent<Image>();
                        imageComponent.sprite = elements.Value.GetComponent<SpriteRenderer>().sprite;
                        availableSpaces[b] = currentGO.name;
                        Debug.Log(availableSpaces[b]);
                        break;
                    }
                } 
            }
        }
    }

    //function to check if object exist as child
    public int ContainsChild(string name)
    {
        int answer = -1;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).name == name)
            {
                answer = i;
                break;
            }
        }
        return answer;
    }
}
