using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour
{
    private static ScreenShake instance;
    private Vector3 originalPosition;
    
    void Awake()
    {
        instance = this;
        originalPosition = transform.localPosition;
    }

    public static void Shake(float duration = 0.15f, float magnitude = 0.3f)
    {
        if (instance != null)
        {
            instance.StopAllCoroutines();
            instance.StartCoroutine(instance.DoShake(duration, magnitude));
        }
    }

    private IEnumerator DoShake(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}