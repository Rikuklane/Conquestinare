using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClipGroup clickUIButton;
    public AudioClipGroup hoverUIButton;
    public AudioClipGroup cannon;
    public AudioClipGroup cardChoiceSelect;
    public AudioClipGroup coin;
    public AudioClipGroup warCry;
    public AudioClipGroup cardHit;
    public AudioClipGroup cardHover;
    public static AudioController Instance;
    
    private void Awake()
    {
        Instance = this;
    }
}
