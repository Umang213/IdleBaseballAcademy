using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    public List<Money> allHideMoney;
    public List<GameObject> allHideMoneyImage;
    MoneyManager _moneyManager;

    protected void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        _moneyManager = MoneyManager.instance;
        for (byte i = 0; i < 20; i++)
        {
            var m = Instantiate(_moneyManager.moneyPrefab, transform).GetComponent<Money>();
            var mi = Instantiate(_moneyManager.moneyImage, transform);
            PoolMoney(m);
            PoolMoneyImage(mi);
        }
    }

    public Money GetMoney()
    {
        if (allHideMoney.Count > 0)
        {
            var m = allHideMoney[0];
            allHideMoney.Remove(allHideMoney[0]);
            m.Show();
            return m;
        }
        else
        {
            var m = Instantiate(_moneyManager.moneyPrefab, transform);
            return m.GetComponent<Money>();
        }
    }

    public GameObject GetMoneyImage()
    {
        if (allHideMoneyImage.Count > 0)
        {
            var m = allHideMoneyImage[0];
            allHideMoneyImage.Remove(allHideMoneyImage[0]);
            m.Show();
            return m;
        }
        else
        {
            var m = Instantiate(_moneyManager.moneyImage, transform);
            return m;
        }
    }

    public void PoolMoney(Money money)
    {
        if (allHideMoney.Count > 25)
        {
            Destroy(money.gameObject);
        }
        else
        {
            money.Hide();
            money.transform.SetParent(transform);
            allHideMoney.Add(money);
        }
    }

    public void PoolMoneyImage(GameObject moneyimage)
    {
        if (allHideMoneyImage.Count > 25)
        {
            Destroy(moneyimage.gameObject);
        }
        else
        {
            moneyimage.Hide();
            moneyimage.transform.SetParent(transform);
            allHideMoneyImage.Add(moneyimage);
        }
    }
}