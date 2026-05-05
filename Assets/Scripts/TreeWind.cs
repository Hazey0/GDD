using UnityEngine;

public class TreeWind : MonoBehaviour
{
    [Header("Wind Settings")]
    public float windStrength = 0.3f;
    public float windSpeed = 1.5f;
    public float windRandomness = 0.5f;

    private Vector3 originalRotation;
    private float randomOffset;

    private void Start()
    {
        originalRotation = transform.eulerAngles;
        // Random offset so all trees dont sway at same time
        randomOffset = Random.Range(0f, 100f);
    }

    private void Update()
    {
        float time = Time.time * windSpeed + randomOffset;

        // Create swaying motion using sin wave
        float swayX = Mathf.Sin(time) * windStrength;
        float swayZ = Mathf.Sin(time * 0.7f) * windStrength * 0.5f;

        transform.eulerAngles = new Vector3(
            originalRotation.x + swayX,
            originalRotation.y,
            originalRotation.z + swayZ
        );
    }
}