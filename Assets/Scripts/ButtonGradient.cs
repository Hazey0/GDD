using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Button Gradient")]
public class ButtonGradient : BaseMeshEffect
{
    [Header("Gradient Colors")]
    public Color topColor = new Color(0.2f, 0.8f, 0.4f, 1f);
    public Color bottomColor = new Color(0.0f, 0.3f, 0.1f, 1f);

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive()) return;

        var verts = new System.Collections.Generic.List<UIVertex>();
        vh.GetUIVertexStream(verts);

        float min = float.MaxValue;
        float max = float.MinValue;

        for (int i = 0; i < verts.Count; i++)
        {
            if (verts[i].position.y < min) min = verts[i].position.y;
            if (verts[i].position.y > max) max = verts[i].position.y;
        }

        float height = max - min;

        for (int i = 0; i < verts.Count; i++)
        {
            UIVertex v = verts[i];
            float t = (v.position.y - min) / height;
            v.color = Color.Lerp(bottomColor, topColor, t);
            verts[i] = v;
        }

        vh.Clear();
        vh.AddUIVertexTriangleStream(verts);
    }
}