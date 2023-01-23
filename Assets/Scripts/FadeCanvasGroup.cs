using System;
using System.Collections;
using System.Collections.Generic;
using Turns;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TMPro;

public class FadeCanvasGroup : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public Button button;
    public TextMeshProUGUI startText;

    private void Awake()
    {
        button.onClick.AddListener(FadeOut);
        button.enabled = false;
    }

    public void ActivateStartScreenWithFade()
    {
        Player currentPlayer = Events.RequestPlayer();
        startText.text = String.Format("{0}'s turn", currentPlayer.name);
        canvasGroup.gameObject.SetActive(true);
        StartCoroutine(DoFade(true, 1f));
    }

    IEnumerator DoFade(bool fadeIn, float duration)
    {
        float counter = 0f;
        float start = fadeIn ? 0 : 1;
        float end = fadeIn ? 1 : 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, counter / duration);
            yield return null;
        }

        button.enabled = fadeIn;
        if(!fadeIn)
        {
            canvasGroup.gameObject.SetActive(false);
            TurnManager.Instance.TriggerTurnEndState();
        }
    }

    public void FadeOut()
    {
        StartCoroutine(DoFade(false, 0.3f));
    }
}
