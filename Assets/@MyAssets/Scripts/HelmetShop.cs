using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetShop : MonoBehaviour
{
    public Transform stadingPoint;
    public List<Customer> allWaitingCustomer;
    public FillObject fillObject;

    Customer _storedCustomer;
    BaseBallController _ballController;

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
                PickUpHelmet();
                /*var temp = fillObject.allFillObject.Find(x => x.activeSelf == true);
                if (temp != null)
                {
                    temp.SetActive(false);
                    allWaitingCustomer.Remove(tcustomer);
                    tcustomer.helmet.Show();
                    _ballController.baseballShop.allWaitingCustomer.Add(tcustomer);
                    _ballController.baseballShop.ArrangePosition();
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

    public void PickUpHelmet()
    {
        StartCoroutine(Helmet());
    }

    IEnumerator Helmet()
    {
        yield return new WaitForSeconds(1.5f);
        var temp = fillObject.allFillObject.Find(x => x.activeSelf == true);
        if (temp == null)
        {
            if (PlayerPrefs.GetInt(PlayerPrefsKey.TutorialCount, 0).Equals(0))
            {
                TutorialControler.Instance.targetPoint = TutorialControler.Instance.helmetPoint;
            }
        }

        if (temp != null && _storedCustomer != null)
        {
            temp.SetActive(false);
            if (_storedCustomer.isGirl)
            {
                _storedCustomer.hair.Hide();
            }

            _storedCustomer.helmet.Show();
            allWaitingCustomer.Remove(_storedCustomer);
            _ballController.baseballShop.allWaitingCustomer.Add(_storedCustomer);
            _ballController.baseballShop.ArrangePosition();
            _storedCustomer = null;
            ArrangePosition();
        }
    }
}