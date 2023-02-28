using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TicketController : MonoBehaviour
{
    public Image fillImage;
    public Collider playerCollider;

    //public MoneyStacker moneyStacker;
    public Transform stadingPoint;
    public List<Chair> chairs;

    bool _verify;
    bool _isCustomer;

    bool _isPlayer;
    //public bool _isWorker;

    private void Start()
    {
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
                /*if (_isWorker)
                {
                    //AggryPermission();
                }
                else */
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
            var temp = chairs.Find(x => x.storedCustomer == null);
            if (temp != null)
            {
                temp.storedCustomer = customer;
                customer.SetTarget(temp.standingPoint.position, (() => { temp.SittingCustomer(); }));
                customer.isCustomerReady = true;
                //CodeMonkey.Utils.FunctionTimer.Create(() => moneyStacker.GiveMoney(customer.transform, 5), 0.5f);
                CustomerManager.instance.allWaitingCustomers.Remove(customer);
                _isCustomer = false;
                _verify = false;
                CodeMonkey.Utils.FunctionTimer.Create(() =>
                {
                    CustomerManager.instance.instanceSpawing();
                    CustomerManager.instance.ArrangePosition();
                }, 2);
            }
        }
    }
}