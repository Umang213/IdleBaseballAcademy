using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BallStore : MonoBehaviour
{
    public List<Transform> points;
    public Ball prefab;
    public Image fillImage;
    public bool isPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (isPlayer) return;
            isPlayer = true;
            DOTween.To(() => fillImage.fillAmount, x => fillImage.fillAmount = x, 0, 1.5f).SetId(fillImage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isPlayer) return;
            isPlayer = false;
            DOTween.Kill(fillImage);
            DOTween.To(() => fillImage.fillAmount, x => fillImage.fillAmount = x, 1, 0.5f);
        }
    }
}