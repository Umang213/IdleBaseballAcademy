using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseballShop : MonoBehaviour
{
    public Transform stadingPoint;
    public List<Customer> allWaitingCustomer;
    public FillObject fillObject;
    BaseBallController _ballController;
    Customer _storedCustomer;

    private void Start()
    {
        _ballController = BaseBallController.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Customer tcustomer))
        {
            if (tcustomer == allWaitingCustomer[0])
            {
                _storedCustomer = tcustomer;
                PickUpBaseball();
                /*var temp = fillObject.allFillObject.Find(x => x.activeSelf == true);
                if (temp != null)
                {
                    temp.Hide();
                    allWaitingCustomer.Remove(tcustomer);
                    tcustomer.baseball.Show();
                    tcustomer.SetTarget(tcustomer.taskController.gamePlayPoint.position);
                    ArrangePosition();
                }*/
            }
        }
    }

    public void ArrangePosition()
    {
        for (byte i = 0; i < allWaitingCustomer.Count; i++)
        {
            var pos = stadingPoint.position;
            if (!i.Equals(0))
            {
                pos.z -= (i * 2);
            }

            var i1 = i;
            allWaitingCustomer[i]
                .SetTarget(pos, (() => allWaitingCustomer[i1].transform.rotation = stadingPoint.rotation));
        }
    }

    public void PickUpBaseball()
    {
        StartCoroutine(Baseball());
    }

    IEnumerator Baseball()
    {
        yield return new WaitForSeconds(1.5f);
        var temp = fillObject.allFillObject.Find(x => x.activeSelf == true);
        if (temp != null && _storedCustomer != null)
        {
            temp.Hide();
            allWaitingCustomer.Remove(_storedCustomer);
            _storedCustomer.baseball.Show();
            _storedCustomer.SetTarget(_storedCustomer.taskController.gamePlayPoint.position);
            ArrangePosition();
        }
    }
}