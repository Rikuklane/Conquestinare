using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Serialization;

public class UIButtonController : MonoBehaviour
{
    public int numberOfPlayers = 1;
    public int numberOfNpc = 1;
    [FormerlySerializedAs("input")] public TMP_InputField inputPlayer;
    public TMP_InputField inputNpc;

    private void Awake()
    {
        string validCharactersPlayer = "123456";
        string validCharactersNpc = "012345";
        if (inputPlayer == null && inputNpc == null)
        {
            return;
        }
        inputPlayer.onValidateInput = (text, index, addedChar) => OnValidateInput(
            index, addedChar, validCharactersPlayer, inputNpc);
        inputNpc.onValidateInput = (text, index, addedChar) => OnValidateInput(
            index, addedChar, validCharactersNpc, inputPlayer);
    }

    private char OnValidateInput(int charindex, char addedchar, string validChars, TMP_InputField otherValue)
    {
        if (charindex > 0 || validChars.IndexOf(addedchar) == -1)
        {
            return '\0';
        }

        if (otherValue.text.Length != 0)
        {
            Int32.TryParse(addedchar.ToString(), out int i);
            Int32.TryParse(otherValue.text, out int j);
            if (i + j > 6)
            {
                otherValue.text = (6 - i).ToString();
            }
            else if (i + j < 2)
            {
                otherValue.text = (2 - i).ToString();
            }
        }

        return addedchar;
    }

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
        if(Int32.TryParse(inputPlayer.text, out int i))
        {
            numberOfPlayers = i;
        }
        if(Int32.TryParse(inputNpc.text, out int j))
        {
            numberOfNpc = j;
        }
        PlayerPrefs.SetInt("playerNumber", numberOfPlayers);
        PlayerPrefs.SetFloat("volumeSlider", AudioController.Instance.volumeSliderValue);
        PlayerPrefs.SetInt("npcNumber", numberOfNpc);
        SceneManager.LoadScene(1);
    }
    
    public void BackToMenu()
    {
        PlayerPrefs.SetFloat("volumeSlider", AudioController.Instance.volumeSliderValue);
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
