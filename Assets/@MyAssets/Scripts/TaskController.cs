using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

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
    List<Ball> _usedBall = new List<Ball>();
    int _ballCount;
    bool _isPlayer;
    bool _isTaskStart;
    PoolManager _poolManager;

    private void Start()
    {
        _poolManager = PoolManager.instance;
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

            yield return new WaitForSeconds(1);
        }
    }

    public void StartTask()
    {
        _ballCount = 0;
        StartCoroutine(PlayTask());
    }

    IEnumerator PlayTask()
    {
        yield return new WaitUntil(() => allBalls.Count > 0);
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
            //_poolManager.PoolBall(ball);
        });
        yield return new WaitForSeconds(0.85f);
        storedCustomer.SetAnimation("Shot");
        _usedBall.Add(ball);
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
        for (byte i = 0; i < _usedBall.Count; i++)
        {
            _usedBall[i].rb.isKinematic = true;
            _poolManager.PoolBall(_usedBall[i]);
        }
    }
}