using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIButtonController : MonoBehaviour
{
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
        SceneManager.LoadScene(1);
    }
}
