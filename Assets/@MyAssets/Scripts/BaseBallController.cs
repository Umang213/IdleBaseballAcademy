using System.Collections.Generic;
using UnityEngine;

public class BaseBallController : MonoBehaviour
{
    public static BaseBallController instance;
    public List<GameObject> allHelmet;
    public List<GameObject> allBaseball;
    public HelmetShop helmetShop;
    public BaseballShop baseballShop;
    public List<TaskController> allTaskControllers;

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

    /*public void CollectHelmet(Customer customer)
    {
        customer.SetTarget(helmetPoint.position, (() =>
        {
            customer.helmet.Show();
            CollectBaseball(customer);
        }));
    }

    public void CollectBaseball(Customer customer)
    {
        customer.SetTarget(baseballPoint.position, () =>
        {
            customer.baseball.Show();
            customer.SetTarget(customer.taskController.gamePlayPoint.position,
                (() => customer.taskController.PlayTask()));
        });
    }*/
}