using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TaskController : MonoBehaviour
{
    public int maxStackCount;
    public Transform stackPoint;
    public List<Ball> allBalls;
    public Transform gamePlayPoint;
    public Customer storedCustomer;
    public Transform ballShotPoint;
    public Transform ballShotPoint1;
    public Transform[] points;
    public List<Ball> usedBall;
    int _ballCount;
    bool _isPlayer;
    bool _isTaskStart;
    private PoolManager _poolManager;
    public GameObject _ballNotification;

    private void Start()
    {
        _poolManager = PoolManager.instance;
        BaseBallController.instance.allTaskControllers.Add(this);
        _ballNotification = stackPoint.GetComponentInChildren<Image>(true).gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Customer customer))
        {
            if (storedCustomer == customer && _isTaskStart == false)
            {
                _isTaskStart = true;
                var transform1 = customer.transform;
                transform1.position = gamePlayPoint.position;
                transform1.rotation = gamePlayPoint.rotation;
                customer.SetAnimation("Bat", true);
                StartTask();
            }
        }

        if (other.TryGetComponent(out PlayerController player))
        {
            if (_isPlayer) return;
            _isPlayer = true;
            if (player.allStackItems.Count.Equals(maxStackCount))
            {
                TutorialControler.Instance.targetPoint = null;
                PlayerPrefs.SetInt(PlayerPrefsKey.TutorialCount, 1);
            }

            StartCoroutine(RemoveBall(player));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_isPlayer) return;
            _isPlayer = false;
        }
    }

    IEnumerator RemoveBall(PlayerController player)
    {
        for (byte i = 0; i < maxStackCount; i++)
        {
            if (allBalls.Count < maxStackCount && _isPlayer)
            {
                var ball = player.RemoveFromLast(_poolManager.ballPrefab, stackPoint);
                if (ball != null) allBalls.Add(ball);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void StartTask()
    {
        _ballCount = 0;
        StartCoroutine(PlayTask());
    }

    IEnumerator PlayTask()
    {
        if (allBalls.Count.Equals(0))
        {
            if (PlayerPrefs.GetInt(PlayerPrefsKey.TutorialCount, 0).Equals(0))
            {
                TutorialControler.Instance.targetPoint = TutorialControler.Instance.ballStorage;
            }

            _ballNotification.Show();
        }

        yield return new WaitUntil(() => allBalls.Count > 0);
        _ballNotification.Hide();
        yield return new WaitForSeconds(1);
        var ball = allBalls[0];
        ball.transform.SetParent(null);
        allBalls.Remove(ball);
        ball.transform.position = ballShotPoint.position;
        ball.transform.DOMove(ballShotPoint1.position, 2).OnComplete(() =>
        {
            ball.rb.isKinematic = false;
            var pos = -transform.forward;
            var count = Random.Range(0, 3);
            if (count.Equals(0))
            {
                pos = (points[0].position - transform.position).normalized;
            }
            else if (count.Equals(1))
            {
                pos = (points[1].position - transform.position).normalized;
            }

            ball.rb.AddForce(pos * Random.Range(400, 500));
        });
        yield return new WaitForSeconds(0.85f);
        storedCustomer.SetAnimation("Shot");
        usedBall.Add(ball);
        yield return new WaitForSeconds(5);
        _ballCount += 1;
        if (_ballCount.Equals(6))
        {
            storedCustomer.SetAnimation("Bat", false);
            storedCustomer.ExitCustomer();
            _isTaskStart = false;
            storedCustomer = null;
            CustomerManager.instance.ticketController.CheckWaitingCustomer();
            StartCoroutine(CleanBall());
            yield break;
        }

        //add force
        StartCoroutine(PlayTask());
    }

    IEnumerator CleanBall()
    {
        yield return new WaitForSeconds(10);
        //var temp = usedBall;
        for (var i = usedBall.Count - 1; i >= 0; i--)
        {
            _poolManager.PoolBall(usedBall[i]);
            usedBall.Remove(usedBall[i]);
        }
    }

    public void ChangeStackPoint(Transform point)
    {
        stackPoint = point;

        for (var i = 0; i < allBalls.Count; i++)
        {
            allBalls[i].transform.SetParent(stackPoint);
        }

        _ballNotification = point.GetComponentInChildren<Image>(true).gameObject;
        if (allBalls.Count.Equals(0) && _isTaskStart)
        {
            _ballNotification.Show();
        }
    }
}