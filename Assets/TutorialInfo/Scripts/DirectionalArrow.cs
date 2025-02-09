using UnityEngine;

public class DirectionalArrow : MonoBehaviour
{
    public float edgeBuffer = 100f; // Distance from screen edge
    public float arrowScale = 50f;  // Size of the arrow
    public Color arrowColor = Color.yellow;

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

        // Check if banana is off screen
        bool isOffScreen = screenPoint.x <= 0 || screenPoint.x >= Screen.width ||
                          screenPoint.y <= 0 || screenPoint.y >= Screen.height || isBehind;

        if (isOffScreen)
        {
            canvasGroup.alpha = 1f;

            // If behind the camera, flip the point
            if (isBehind)
            {
                screenPoint.x = Screen.width - screenPoint.x;
                screenPoint.y = Screen.height - screenPoint.y;
            }

            // Calculate center of screen
            Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            
            // Get direction from center to banana
            Vector2 direction = (screenPoint - new Vector3(screenCenter.x, screenCenter.y, 0f)).normalized;

            // Calculate position on screen edge
            float angle = Mathf.Atan2(direction.y, direction.x);
            Vector2 arrowPosition = screenCenter + direction * (Mathf.Min(screenCenter.x, screenCenter.y) - edgeBuffer);

            // Set position and rotation
            arrowRectTransform.position = arrowPosition;
            arrowRectTransform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * angle);
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