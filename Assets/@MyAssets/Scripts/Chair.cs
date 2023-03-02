using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    public Transform standingPoint;
    public Transform sitingPoint;
    public Customer storedCustomer;

    public void SittingCustomer()
    {
        storedCustomer.StopAgent();
        var transform1 = storedCustomer.transform;
        transform1.position = sitingPoint.position;
        transform1.rotation = sitingPoint.rotation;
        storedCustomer.SetAnimation("Sit", true);
        storedCustomer.isSiting = true;
    }
}