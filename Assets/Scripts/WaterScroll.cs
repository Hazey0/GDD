using UnityEngine;

public class WaterScroll : MonoBehaviour
{
    public float scrollSpeedX = 0.5f;
    public float scrollSpeedY = 0.3f;

    private Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        float x = Time.time * scrollSpeedX;
        float y = Time.time * scrollSpeedY;
        mat.SetTextureOffset(
        "_SampleTexture2D_d9bb9fb820d548b59476e842a1e7abd6_Texture_1_Texture2D",
        new Vector2(x, y));
    }
}