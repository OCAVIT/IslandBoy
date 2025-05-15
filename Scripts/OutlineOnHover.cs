using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class OutlineOnHover : MonoBehaviour
{
    public Color outlineColor = Color.yellow;
    public float outlineWidth = 0.03f;

    private Material[] originalMaterials;
    private Material outlineMaterial;
    private Renderer rend;
    private bool outlined = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalMaterials = rend.sharedMaterials;
    }

    void OnMouseEnter()
    {
        if (outlined) return;

        Material[] mats = new Material[originalMaterials.Length + 1];
        mats[0] = new Material(Shader.Find("Custom/URP_Outline"));
        mats[0].SetColor("_OutlineColor", outlineColor);
        mats[0].SetFloat("_OutlineWidth", outlineWidth);

        for (int i = 0; i < originalMaterials.Length; i++)
            mats[i + 1] = originalMaterials[i];

        rend.materials = mats;
        outlined = true;
    }

    void OnMouseExit()
    {
        if (!outlined) return;
        rend.materials = originalMaterials;
        outlined = false;
    }
}