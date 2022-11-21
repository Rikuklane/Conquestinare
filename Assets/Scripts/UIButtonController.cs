using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIButtonController : MonoBehaviour
{
    public int numberOfPlayers = 2;
    public TMP_InputField input;
    public void PlayOnClick()
    {
        AudioController.Instance.clickUIButton.Play();
    }

    public void PlayOnHover()
    {
        AudioController.Instance.hoverUIButton.Play();
    }

    public void StartGame()
    {
        if(Int32.TryParse(input.text, out int j))
        {
            if(2 <= j && j <= 4)
            {
                numberOfPlayers = j;
            }
        }
        PlayerPrefs.SetInt("playerNumber", numberOfPlayers);
        SceneManager.LoadScene(1);
    }
}
