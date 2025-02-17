using UnityEngine;

public class DirectionalArrow : MonoBehaviour
{
    public float edgeBuffer = 100f; // Distance from screen edge
    public float arrowScale = 50f;  // Size of the arrow
    public Color arrowColor = Color.yellow;
    
    [Tooltip("Base rotation offset to align arrow properly")]
    public float baseRotationOffset = 90f; // Adjust this if arrow points up/down by default

    private Camera mainCamera;
    private RectTransform arrowRectTransform;
    private CanvasGroup canvasGroup;
    private Transform targetBanana;

    void Start()
    {
        mainCamera = Camera.main;
        arrowRectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        
        // Set initial scale
        arrowRectTransform.sizeDelta = new Vector2(arrowScale, arrowScale);
    }

    void Update()
    {
        if (targetBanana == null)
        {
            canvasGroup.alpha = 0f;
            return;
        }

        Vector3 screenPoint = mainCamera.WorldToScreenPoint(targetBanana.position);
        bool isBehind = screenPoint.z < 0;
        screenPoint.z = 0;

        // Check if banana is off screen horizontally
        bool isOffScreenX = screenPoint.x <= 0 || screenPoint.x >= Screen.width;
        
        if (isOffScreenX || isBehind)
        {
            canvasGroup.alpha = 1f;

            // Calculate arrow position
            float xPosition;
            float rotation;
            
            // If behind camera or far left, show arrow on left
            if (isBehind || screenPoint.x <= 0)
            {
                xPosition = edgeBuffer;
                rotation = baseRotationOffset - 90f; // Point right
            }
            // If far right, show arrow on right
            else
            {
                xPosition = Screen.width - edgeBuffer;
                rotation = baseRotationOffset + 90f; // Point left
            }

            // Set position and rotation
            arrowRectTransform.position = new Vector3(xPosition, Screen.height / 2f, 0f);
            arrowRectTransform.rotation = Quaternion.Euler(0, 0, rotation);
        }
        else
        {
            canvasGroup.alpha = 0f;
        }
    }

    public void SetTarget(Transform banana)
    {
        targetBanana = banana;
    }
}