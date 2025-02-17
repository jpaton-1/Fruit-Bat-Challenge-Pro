using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    [Header("Score Settings")]
    public float maxPointsPerBanana = 1000f;  // Maximum points for hitting a banana instantly
    public float minPointsPerBanana = 100f;   // Minimum points for hitting a banana
    public float timeToMinPoints = 10f;       // Time in seconds until points decay to minimum
    
    [Header("UI")]
    public TextMeshProUGUI scoreText;         // Reference to the UI text component
    public TextMeshProUGUI pointsPopupPrefab; // Prefab for points popup
    public Transform canvasTransform;         // Reference to the UI canvas
    
    private int totalScore = 0;
    private static ScoreManager instance;

    public static ScoreManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        UpdateScoreDisplay();
    }

    public void BananaHit(Vector3 hitPosition, float lifetime)
    {
        // Calculate points based on how quickly the banana was hit
        float timeRatio = Mathf.Clamp01(lifetime / timeToMinPoints);
        float points = Mathf.Round(Mathf.Lerp(maxPointsPerBanana, minPointsPerBanana, timeRatio));
        
        totalScore += (int)points;
        UpdateScoreDisplay();
        
        // Show points popup
        if (pointsPopupPrefab != null && canvasTransform != null)
        {
            // Convert world position to screen position
            Vector3 screenPos = Camera.main.WorldToScreenPoint(hitPosition);
            
            // Only show popup if banana is in front of the camera
            if (screenPos.z > 0)
            {
                TextMeshProUGUI popup = Instantiate(pointsPopupPrefab, screenPos, Quaternion.identity, canvasTransform);
                popup.text = $"+{points}";
                
                // Animate and destroy the popup
                StartCoroutine(AnimatePointsPopup(popup.gameObject));
            }
        }
    }

    private IEnumerator AnimatePointsPopup(GameObject popup)
    {
        float duration = 1f;
        float elapsed = 0f;
        Vector3 startPos = popup.transform.position;
        Vector3 endPos = startPos + Vector3.up * 100f;
        CanvasGroup canvasGroup = popup.GetComponent<CanvasGroup>();
        
        // Ensure we have a CanvasGroup
        if (canvasGroup == null)
        {
            canvasGroup = popup.AddComponent<CanvasGroup>();
        }
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;
            
            // Move upward and fade out
            popup.transform.position = Vector3.Lerp(startPos, endPos, progress);
            canvasGroup.alpha = 1 - progress;
            
            yield return null;
        }
        
        Destroy(popup);
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {totalScore:N0}";  // Format with comma separators
        }
    }
}