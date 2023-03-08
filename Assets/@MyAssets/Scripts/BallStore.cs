using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BallStore : MonoBehaviour
{
    public List<Transform> points;
    public Image fillImage;
    public bool isPlayer;
    PoolManager _poolManager;

    private void Start()
    {
        _poolManager = PoolManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (isPlayer) return;
            isPlayer = true;
            DOTween.To(() => fillImage.fillAmount, x => fillImage.fillAmount = x, 0, 1.5f).SetId(fillImage)
                .OnComplete(() => StartCoroutine(CollectBall(player)));
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

    IEnumerator CollectBall(PlayerController player)
    {
        for (byte i = 0; i < 10; i++)
        {
            if (!player.IsStackFull() && isPlayer)
            {
                var point = points[Helper.RandomInt(0, points.Count)].transform;
                var ball = _poolManager.GetBall();
                ball.transform.position = point.position;
                point.Hide();
                player.AddToStack(ball);
                point.DOScale(Vector3.one, 0.2f).SetDelay(0.5f).From(Vector3.zero).OnStart((() => point.Show()));
            }

            if (player.IsStackFull())
            {
                if (PlayerPrefs.GetInt(PlayerPrefsKey.TutorialCount, 0).Equals(0))
                {
                    TutorialControler.Instance.targetPoint = TutorialControler.Instance.ballMachinePoint;
                }
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}