using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public float floatHeight = 2f;
    public float floatSpeed = 2f;
    
    private bool isFloating = false;
    private Vector3 startPosition;
    private Rigidbody rb;
    
    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if (isFloating)
        {
            Vector3 targetPosition = startPosition + Vector3.up * floatHeight;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * floatSpeed);
        }
        else if (!isFloating && transform.position != startPosition)
        {
            transform.position = Vector3.Lerp(transform.position, startPosition, Time.deltaTime * floatSpeed);
        }
    }
    
    public void ToggleFloat()
    {
        isFloating = !isFloating;
        rb.isKinematic = true; // Disable physics while floating
    }
}
