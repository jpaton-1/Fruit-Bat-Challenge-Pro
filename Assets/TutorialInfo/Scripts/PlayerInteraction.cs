using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f;
    public float hitForce = 10f;
    
    private Camera playerCamera;
    
    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
    }
    
    void Update()
    {
        // Hitting objects with left click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, interactionRange))
            {
                HittableObject hittable = hit.collider.GetComponent<HittableObject>();
                if (hittable != null)
                {
                    hittable.Hit(ray.direction * hitForce);
                }
            }
        }
        
        // Trigger floating with 'F' key
        if (Input.GetKeyDown(KeyCode.F))
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, interactionRange))
            {
                FloatingObject floating = hit.collider.GetComponent<FloatingObject>();
                if (floating != null)
                {
                    floating.ToggleFloat();
                }
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        GrowingObject growing = hit.gameObject.GetComponent<GrowingObject>();
        if (growing != null)
        {
            growing.Grow();
        }
    }
}
