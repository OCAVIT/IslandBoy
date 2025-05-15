using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TMPTextFader : MonoBehaviour
{
    public float fadeDuration = 1.0f;
    public float minAlpha = 0.0f;
    public float maxAlpha = 1.0f;

    private TMP_Text tmpText;
    private float timer = 0f;

    void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = Mathf.PingPong(timer / fadeDuration, 1f);
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, t);

        Color color = tmpText.color;
        color.a = alpha;
        tmpText.color = color;
    }
}