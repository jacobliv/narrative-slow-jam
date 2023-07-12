using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PhoneManager : MonoBehaviour
{
    bool phoneVisible;
    [SerializeField] GameObject phoneScreen, phoneToggle, homePage, musicPage, settingsPage;
    [SerializeField] TMP_Text clock;

    void Start()
    {
        phoneVisible = false;
        musicPage.SetActive(false);
        homePage.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) & phoneVisible)
        {
            phoneVisible = false;
            phoneToggle.SetActive(true);
            phoneScreen.SetActive(false);
        }
    }

    public void HandleButtonPress()
    {
        if (phoneVisible)
        {
            phoneVisible = false;
            gameObject.SetActive(false);
        }

        else if (!phoneVisible)
        {
            phoneVisible = true;
            phoneToggle.SetActive(false);
            phoneScreen.SetActive(true);
        }
    }

    public void handleHomeButton()
    {
        homePage.SetActive(true);
        musicPage.SetActive(false);
        settingsPage.SetActive(false);
    }

    public void handleMusicButton()
    {
        homePage.SetActive(false);
        musicPage.SetActive(true);
        settingsPage.SetActive(false);
    }

    public void handleSettingsButton()
    {
        homePage.SetActive(false);
        musicPage.SetActive(false);
        settingsPage.SetActive(true);
    }

}
