using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonController : MonoBehaviour
{
    public AudioClipGroup clickUIButton;
    public AudioClipGroup hoverUIButton;

    public AudioClip hoverButtonSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = transform.GetComponent<AudioSource>();
    }

    public void PlayOnClick()
    {
        audioSource.PlayOneShot(clickUIButton.clips[0]);
    }

    public void PlayOnHover()
    {
        audioSource.PlayOneShot(hoverUIButton.clips[1]);
    }
}
