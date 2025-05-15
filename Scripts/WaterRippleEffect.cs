using UnityEngine;

public class WaterRippleEffect : MonoBehaviour
{
    public Material waterMaterial;
    public float rippleRadius = 0.3f;
    public float rippleThickness = 0.03f;
    public float rippleDuration = 1.0f;

    private float rippleTime = 0f;
    private bool rippleActive = false;
    private Vector2 rippleCenter;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Water"))
                {
                    rippleCenter = hit.textureCoord;
                    rippleTime = 0f;
                    rippleActive = true;

                    waterMaterial.SetVector("_RippleCenter", new Vector4(rippleCenter.x, rippleCenter.y, 0, 0));
                    waterMaterial.SetFloat("_RippleRadius", rippleRadius);
                    waterMaterial.SetFloat("_RippleThickness", rippleThickness);
                    waterMaterial.SetFloat("_RippleStrength", 1f);
                }
            }
        }

        if (rippleActive)
        {
            rippleTime += Time.deltaTime / rippleDuration;
            waterMaterial.SetFloat("_RippleTime", rippleTime);

            if (rippleTime > 1f)
            {
                rippleActive = false;
                waterMaterial.SetFloat("_RippleStrength", 0f);
            }
        }
    }
}