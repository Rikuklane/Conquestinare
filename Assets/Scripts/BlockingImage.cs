using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlockingImage : MonoBehaviour
{
    public Image transparentBlockingImage;
    public static BlockingImage Instance;


    void Awake()
    {
        Instance = this;
    }

    public void ActivateBlockingImage(bool active)
    {
        var imageObject = transparentBlockingImage.gameObject;
        imageObject.SetActive(active);
    }
}
