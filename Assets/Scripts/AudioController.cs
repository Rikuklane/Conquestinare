using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [Header("UI Sounds")]
    [Space]
    public AudioClipGroup clickUIButton;
    public AudioClipGroup hoverUIButton;

    [Header("Game Sounds")]
    [Space]
    public AudioClipGroup cannon;
    public AudioClipGroup coin;
    public AudioClipGroup warCry;
    public AudioClipGroup battleHit;
    public AudioClipGroup startTurn;

    [Header("Card Sounds")]
    [Space]
    public AudioClipGroup cardChoiceSelect;
    public AudioClipGroup cardHit;
    public AudioClipGroup place;
    public AudioClipGroup sparkle;
    public AudioClipGroup cardHover;

    public static AudioController Instance;

    private void Awake()
    {
        Instance = this;
    }
}
