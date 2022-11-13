using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonController : MonoBehaviour
{
    public AudioClipGroup clickUIButton;
    public AudioClipGroup hoverUIButton;

    private void OnMouseEnter()
    {
        Debug.Log("Mouse entered!");
        hoverUIButton.Play();
    }

    public void PlayOnClick()
    {
        Debug.Log("Button clicked");
        clickUIButton.Play();
    }
}
