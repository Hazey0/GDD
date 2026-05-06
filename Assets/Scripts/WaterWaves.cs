using UnityEngine;

public class WaterWaves : MonoBehaviour
{
    public float waveHeight = 0.5f;
    public float waveSpeed = 1f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * waveSpeed) * waveHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}