using UnityEngine;

public class FluidShaderAccel : MonoBehaviour
{
    private Material material;
    public Rigidbody rb;

    [Range(0f, 1f)] public float fillLevel = 0.5f;

    [Tooltip("Subtract gravity so fluid stays flat in free-fall")]
    public bool ignoreGravity = true;

    [Tooltip("Smoothing factor for acceleration (0 = no smoothing, 1 = full smoothing)")]
    [Range(0f, 1f)] public float smoothing = 0.1f;

    private Vector3 lastVelocity;
    private Vector3 previousAccel;
    private Vector3 rawAccel;

    void Start()
    {
        // Try to get renderer on this object
        MeshRenderer targetRenderer = transform.Find("PotionInterior").GetComponent<MeshRenderer>();
        if (targetRenderer != null)
        {
            material = targetRenderer.material; // unique instance
        }
        else
        {
            Debug.LogError("FluidShaderAccel: No Renderer found on " + gameObject.name);
        }

        if (rb == null)
        {
            Debug.LogError("FluidShaderAccel: Rigidbody is not assigned.");
        }
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        Vector3 currentVelocity = rb.velocity;
        rawAccel = (currentVelocity - lastVelocity) / Time.fixedDeltaTime;
        lastVelocity = currentVelocity;
    }

    void Update()
    {
        if (material == null || rb == null) return;

        // Smooth acceleration over time
        Vector3 smoothedAccel = Vector3.Lerp(previousAccel, rawAccel, 1f - Mathf.Pow(1f - smoothing, 60f * Time.deltaTime));
        previousAccel = smoothedAccel;

        Vector3 finalAccel = ignoreGravity ? smoothedAccel - Physics.gravity : smoothedAccel;

        Debug.Log("Accel: " + finalAccel + " Fill: " + fillLevel);

        material.SetVector("_Accel", finalAccel);
        material.SetFloat("_Fill", fillLevel);
    }
}
