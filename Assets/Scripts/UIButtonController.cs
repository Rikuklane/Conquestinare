using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonController : MonoBehaviour
{
    public AudioClipGroup clickUIButton;
    public AudioClipGroup hoverUIButton;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(PlayClickUIButton);
    }

    private void PlayHoverUIButton()
    {
        hoverUIButton.Play();
    }
    
    private void PlayClickUIButton()
    {
        clickUIButton.Play();
    }

    private void OnMouseEnter()
    {
        PlayHoverUIButton();
    }
}
