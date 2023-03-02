using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class FillObject : MonoBehaviour
{
    bool _isPLayer;
    public Image fillImage;
    public List<GameObject> allFillObject;
    public UnityEvent playerTriggerEnter;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_isPLayer) return;
            _isPLayer = true;
            /*DOTween.To(() => fillImage.fillAmount, x => fillImage.fillAmount = x, 0, 1.5f)
                .OnComplete(() => StartCoroutine(StartFilling()))
                .SetId(fillImage);*/
            StartCoroutine(StartFilling());
            playerTriggerEnter?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_isPLayer) return;
        _isPLayer = false;
        DOTween.Kill(fillImage);
        DOTween.To(() => fillImage.fillAmount, x => fillImage.fillAmount = x, 1, 0.5f);
        StopCoroutine(StartFilling());
    }

    IEnumerator StartFilling()
    {
        var temp = allFillObject.FindAll(x => x.activeSelf == false);
        for (byte i = 0; i < allFillObject.Count; i++)
        {
            if (_isPLayer)
            {
                var i1 = i;
                DOTween.To(() => fillImage.fillAmount, x => fillImage.fillAmount = x, 0, 1f).From(1)
                    .OnComplete(
                        () =>
                        {
                            temp[i1].Show();
                            temp[i1].transform.DOScale(Vector3.one, 0.75f).SetEase(Ease.OutBack).From(Vector3.zero);
                        })
                    .SetId(fillImage);
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield break;
            }

            if (i == allFillObject.Count - 1)
            {
                DOTween.Kill(fillImage);
                DOTween.To(() => fillImage.fillAmount, x => fillImage.fillAmount = x, 1, 0.5f);
            }
        }
    }
}