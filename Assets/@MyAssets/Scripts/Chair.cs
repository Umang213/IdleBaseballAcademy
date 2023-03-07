using UnityEngine;

public class Chair : MonoBehaviour
{
    public Transform standingPoint;
    public Transform sitingPoint;
    public Customer storedCustomer;

    private void Start()
    {
        CustomerManager.instance.ticketController.chairs.Add(this);
    }

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