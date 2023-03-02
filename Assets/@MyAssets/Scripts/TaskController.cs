using System;
using UnityEngine;

public class TaskController : MonoBehaviour
{
    public Transform gamePlayPoint;
    public Customer storedCustomer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Customer customer))
        {
            if (storedCustomer == customer)
            {
                var transform1 = customer.transform;
                transform1.position = gamePlayPoint.position;
                transform1.rotation = gamePlayPoint.rotation;
                customer.SetAnimation("Bat", true);
            }
        }
    }

    public void PlayTask()
    {
    }
}