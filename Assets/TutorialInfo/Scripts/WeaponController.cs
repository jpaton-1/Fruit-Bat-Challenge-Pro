using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    public GameObject batPrefab;
    public float hitRange = 3f;
    public float swingCooldown = 0.5f;
    public float swingSpeed = 400f;
    
    private GameObject bat;
    private bool isSwinging = false;
    private bool canSwing = true;
    private float nextSwingTime;

    void Start()
    {
        // Instantiate the actual bat prefab
        bat = Instantiate(batPrefab, transform);
        bat.transform.localPosition = new Vector3(1f, -1f, 2f);
        bat.transform.localRotation = Quaternion.Euler(45f, 0, 0);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canSwing && Time.time >= nextSwingTime && !isSwinging)
        {
            StartCoroutine(SwingBat());
        }
    }

    IEnumerator SwingBat()
    {
        isSwinging = true;
        canSwing = false;
        nextSwingTime = Time.time + swingCooldown;

        // Starting rotation
        Quaternion startRotation = bat.transform.localRotation;
        
        // Swing forward
        float elapsedTime = 0f;
        float swingDuration = 0.2f;

        while (elapsedTime < swingDuration)
        {
            elapsedTime += Time.deltaTime;
            float swingProgress = elapsedTime / swingDuration;
            
            float currentAngle = Mathf.Lerp(0, 90, swingProgress);
            bat.transform.localRotation = startRotation * Quaternion.Euler(-currentAngle, 0, 0);

            // Check for hits during the swing
            if (swingProgress > 0.2f && swingProgress < 0.8f)
            {
                CheckForHits();
            }

            yield return null;
        }

        // Swing back
        elapsedTime = 0f;
        Quaternion endRotation = bat.transform.localRotation;

        while (elapsedTime < swingDuration)
        {
            elapsedTime += Time.deltaTime;
            float swingProgress = elapsedTime / swingDuration;
            bat.transform.localRotation = Quaternion.Lerp(endRotation, startRotation, swingProgress);
            yield return null;
        }

        bat.transform.localRotation = startRotation;
        isSwinging = false;
        canSwing = true;
    }

    void CheckForHits()
    {
        Vector3 batPosition = bat.transform.position;
        Vector3 batForward = bat.transform.forward;
        
        RaycastHit[] hits = Physics.SphereCastAll(batPosition, hitRange, batForward, hitRange);
        
        foreach (RaycastHit hit in hits)
        {
            FruitEnemy fruit = hit.collider.GetComponent<FruitEnemy>();
            if (fruit != null)
            {
                Vector3 hitDirection = (hit.point - batPosition).normalized;
                fruit.TakeDamage(25f, hitDirection);
            }
        }
    }
}