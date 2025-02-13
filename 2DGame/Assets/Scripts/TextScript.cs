﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextScript : MonoBehaviour
{
    public GameObject gameController;
    public GameControllerScript gameControllerObject;
    public Vector2 interactablePos;
    public TextAsset rawTexts;
    Dictionary<string, string> texts = new Dictionary<string, string>();
    public string currentTextObjectName;
    public TextMeshProUGUI displayText;
    public GameObject optionBox;
    public OptionScript optionObject;
    public bool hasNextPage = false;
    public bool hasNextOption = false;
    public bool notOption = true;
    public bool virtualActivation = false;
    public int currentPage = 0;
    public List<string> options = new List<string>();
    public string optionTree = "";

    // Start is called before the first frame update
    void Start()
    {
        gameControllerObject = gameController.GetComponent<GameControllerScript>();
        optionObject = optionBox.GetComponent<OptionScript>();
        gameObject.SetActive(false);
        //Get raw text into a dictionary 
        texts = rawTexts.text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(p => p.Trim().Replace(@"\\n", "\n"))
                            .Where(p => !string.IsNullOrWhiteSpace(p))
                            .ToDictionary(line => line.Split('@')[0], line => line.Split('@')[1]);
        displayText = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    //DISPLAY DIALOG
    public void DisplayDialog()
    {
        gameObject.SetActive(true);
        //Debug.Log("DisplayDialog");
        if (!notOption)
        {
            optionObject.gameObject.SetActive(true);
        }
        else
        {
            //Check if text is a normal text
            //Debug.Log(texts.ContainsKey(currentTextObjectName + optionTree + (currentPage + 1).ToString()));
            if (texts.ContainsKey(currentTextObjectName + optionTree + (currentPage + 1).ToString()))
            {
                hasNextPage = true;
                hasNextOption = false;
                currentPage += 1;
            }
            //Check if text if a option
            else if (texts.ContainsKey(currentTextObjectName + optionTree + (currentPage + 1).ToString() + "O"))
            {
                hasNextPage = true;
                hasNextOption = true;
                notOption = false;
                currentPage += 1;
            }
            //No text
            else
            {
                currentTextObjectName = "";
                optionTree = "";
                hasNextPage = false;
                hasNextOption = false;
                virtualActivation = false;
                currentPage = 0;
                gameObject.SetActive(false);
            }
        }

        //if it's a option
        if (hasNextOption && hasNextPage)
        {
            notOption = false;
            options = texts[currentTextObjectName + optionTree + (currentPage).ToString() + "O"].Split('|').ToList();
            optionObject.DisplayOption(true, options);
        }
        //if it's not a option, a normal text
        else if (hasNextPage)
        {
            //Debug.Log(texts[currentTextObjectName + optionTree + (currentPage).ToString()]);
            gameControllerObject.currentText = texts[currentTextObjectName + optionTree + (currentPage).ToString()];
            displayText.SetText(texts[currentTextObjectName + optionTree + (currentPage).ToString()]);
            //Debug.Log(gameObject.activeInHierarchy);
        }
    }

    public void changeTextByKey(string key, string newstring)
    {
        texts[key] = newstring;
    }

    public void removeTextsByKeyOnDialogue(string[] keys, string dialogue)
    {
        if (gameControllerObject.currentText == dialogue)
        {
            foreach (var key in keys)
            {
                texts.Remove(key);
            }
        }
    }

    public void virtualActivationFuntion(string name, Vector2 position)
    {
        //Debug.Log("Virtual Activation: " + name + "  " + position);
        interactablePos = position;
        currentTextObjectName = name;
        virtualActivation = true;
        DisplayDialog();
    }
}
