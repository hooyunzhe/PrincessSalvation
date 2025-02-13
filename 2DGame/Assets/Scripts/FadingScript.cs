﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadingScript : MonoBehaviour
{
    Image fadeOutImage;
    float alpha = 1f;
    int fadeDir = -1;
    public float fadeDura = 1;
    public bool continueMusic = false;

    void Start()
    {
        //OnSceneLoaded will be called when the scene is loaded (name is self declared)
        SceneManager.sceneLoaded += OnSceneLoaded;
        fadeOutImage = gameObject.GetComponent<Image>();
    }

    void Update()
    {       
        AudioSource BackgroundMusic = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
        if (fadeDir == 1)
        {
            if (!continueMusic)
                BackgroundMusic.volume -= 0.0075f / fadeDura;
        }
        else if (BackgroundMusic.volume <= 0.5)
        {
            if (BackgroundMusic.volume == 0f)
            {
                BackgroundMusic.UnPause();
            }
            BackgroundMusic.volume += 0.015f;
        }

        if (BackgroundMusic.volume <= 0f)
        {
            BackgroundMusic.Pause();
        }

        // Change the alpha of the fade screen over the duration
        if (Mathf.Clamp01(alpha) != Mathf.Clamp01(fadeDir))
        {
            alpha += fadeDir * (1 / fadeDura) * Time.deltaTime;
            Color alphaChanged = fadeOutImage.color;
            alphaChanged.a = Mathf.Clamp01(alpha);
            fadeOutImage.color = alphaChanged;
        }
    }

    public void BeginFade(int direction, float duration)
    {
        fadeDir = direction;
        fadeDura = duration;
    }

    //Fade in once scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BeginFade(-1, fadeDura);
    }
}
