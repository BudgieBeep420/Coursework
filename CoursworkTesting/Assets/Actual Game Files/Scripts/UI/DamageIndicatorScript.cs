using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicatorScript : MonoBehaviour
{
    [SerializeField] private Image bloodImage;

    private const float Time = 0.5f;
    
    private void OnEnable()
    {
        StartCoroutine(WaitToDisable(Time));
        bloodImage.canvasRenderer.SetAlpha(1.0f);
        bloodImage.CrossFadeAlpha(0.0f, Time, false);
    }

    private IEnumerator WaitToDisable(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
