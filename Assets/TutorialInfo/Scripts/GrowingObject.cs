using UnityEngine;

public class GrowingObject : MonoBehaviour
{
    public float growFactor = 1.2f;
    public float maxScale = 3f;
    public float growSpeed = 2f;
    
    private Vector3 originalScale;
    private Vector3 targetScale;
    
    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }
    
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * growSpeed);
    }
    
    public void Grow()
    {
        if (transform.localScale.x < maxScale)
        {
            targetScale *= growFactor;
            targetScale = Vector3.ClampMagnitude(targetScale, maxScale);
        }
    }
}