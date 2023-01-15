using System;
using System.Collections;
using System.Collections.Generic;
using Turns;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FadeCanvasGroup : MonoBehaviour
{
    private bool _isFaded;
    public float duration = 1f;
    public CanvasGroup canvasGroup;
    public Button button;
    public static FadeCanvasGroup Instance;

    private void Awake()
    {
        Instance = this;
        button.onClick.AddListener(FadeOut);
        button.enabled = false;
    }

    public void ActivateStartScreenWithFade()
    {
        canvasGroup.gameObject.SetActive(true);
        Fade();
    }

    private void Fade()
    {
        button.enabled = !button.enabled;
        if (_isFaded)
        {
            
        }
        DoFade(canvasGroup.alpha, _isFaded ? 1 : 0);
        _isFaded = !_isFaded;
    }

    private void DoFade(float start, float end)
    {
        float counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, counter / duration);
        }
    }

    private void FadeOut()
    {
        Fade();
        canvasGroup.gameObject.SetActive(false);
        TurnManager.Instance.TriggerEndState();
    }
}
