using UnityEngine;

public class HittableObject : MonoBehaviour
{
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    public void Hit(Vector3 force)
    {
        rb.isKinematic = false; // Enable physics
        rb.AddForce(force, ForceMode.Impulse);
    }
}
