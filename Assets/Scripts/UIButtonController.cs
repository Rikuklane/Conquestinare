using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
}
