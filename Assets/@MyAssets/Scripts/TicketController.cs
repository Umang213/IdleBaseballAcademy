using System.Collections.Generic;
using UnityEngine;

public class TicketController : MonoBehaviour
{
    public Collider playerCollider;

    public MoneyStacker moneyStacker;
    public Transform stadingPoint;
    public List<Chair> chairs;

    bool _verify;
    bool _isCustomer;
    bool _isPlayer;

    BaseBallController _baseballController;

    private void Start()
    {
        _baseballController = BaseBallController.instance;
        CodeMonkey.Utils.FunctionTimer.Create(() => { CustomerManager.instance.instanceSpawing(); }, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Customer tcustomer))
        {
            if (tcustomer == CustomerManager.instance.allWaitingCustomers[0])
            {
                _isCustomer = true;
                _verify = true;
                if (_isPlayer)
                {
                    AggryPermission();
                }
            }
        }

        if (other.TryGetComponent(out PlayerController player))
        {
            if (_isPlayer) return;
            if (other.bounds.Intersects(playerCollider.bounds))
            {
                _isPlayer = true;
                if (_verify && _isCustomer)
                {
                    AggryPermission();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (!_isPlayer) return;
            _isPlayer = false;
        }

        if (other.TryGetComponent(out Customer tcustomer))
        {
            if (tcustomer == CustomerManager.instance.allWaitingCustomers[0])
            {
                _isCustomer = false;
                _verify = false;
            }
        }
    }

    public void AggryPermission()
    {
        if ((_isCustomer && _isPlayer && _verify))
        {
            var customer = CustomerManager.instance.allWaitingCustomers[0];
            var task = _baseballController.allTaskControllers.Find(x => x.storedCustomer == null);

            if (task != null && _baseballController.helmetShop.allWaitingCustomer.Count < 3)
            {
                task.storedCustomer = customer;
                task.storedCustomer.taskController = task;

                _baseballController.helmetShop.allWaitingCustomer.Add(customer);
                _baseballController.helmetShop.ArrangePosition();
                //_ballController.CollectHelmet(task.storedCustomer);
                NextCustomer(customer);
            }
            else
            {
                var temp = chairs.Find(x => x.storedCustomer == null);
                if (temp != null)
                {
                    temp.storedCustomer = customer;
                    customer.SetTarget(temp.standingPoint.position, (() => { temp.SittingCustomer(); }));
                    NextCustomer(customer);
                }
            }
        }
    }

    private void NextCustomer(Customer customer)
    {
        moneyStacker.GiveMoney(customer.transform, 5);
        CustomerManager.instance.allWaitingCustomers.Remove(customer);
        _isCustomer = false;
        _verify = false;
        CodeMonkey.Utils.FunctionTimer.Create(() =>
        {
            CustomerManager.instance.instanceSpawing();
            CustomerManager.instance.ArrangePosition();
        }, 2);
    }

    public void CheckWaitingCustomer()
    {
        var waitingCustomer = chairs.Find(x => x.storedCustomer != null);
        if (waitingCustomer != null)
        {
            var customer = waitingCustomer.storedCustomer;
            var task = _baseballController.allTaskControllers.Find(x => x.storedCustomer == null);
            if (task != null && _baseballController.helmetShop.allWaitingCustomer.Count < 3)
            {
                waitingCustomer.storedCustomer = null;
                customer.isSiting = false;
                customer.SetAnimation("Sit", false);
                customer.SetAnimation("Idle");
                task.storedCustomer = customer;
                task.storedCustomer.taskController = task;

                _baseballController.helmetShop.allWaitingCustomer.Add(customer);
                _baseballController.helmetShop.ArrangePosition();
            }
        }
    }
}