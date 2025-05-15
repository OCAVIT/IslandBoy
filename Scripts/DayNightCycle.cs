using UnityEngine;
using TMPro;
using System.Collections;

[System.Serializable]
public class DayPhase
{
    public string name;
    public Color sunColor = Color.white;
    public float sunIntensity = 1f;
    public float duration = 10f;
}

public class DayNightCycle : MonoBehaviour
{
    public Light sun;

    [Tooltip("Если включено, игра начинается с дня, иначе — с ночи")]
    public bool startWithDay = true;

    public TMP_Text dayText;

    public DayPhase[] phases = new DayPhase[5]
    {
        new DayPhase { name = "Night", sunColor = new Color(0.1f,0.1f,0.3f), sunIntensity = 0f, duration = 10f },
        new DayPhase { name = "Dawn", sunColor = new Color(1f,0.5f,0.2f), sunIntensity = 0.2f, duration = 5f },
        new DayPhase { name = "Day", sunColor = Color.white, sunIntensity = 1f, duration = 20f },
        new DayPhase { name = "Dusk", sunColor = new Color(1f,0.5f,0.2f), sunIntensity = 0.2f, duration = 5f },
        new DayPhase { name = "Night", sunColor = new Color(0.1f,0.1f,0.3f), sunIntensity = 0f, duration = 10f }
    };

    private float timeInCycle = 0f;
    private float totalCycleDuration;
    private int currentPhaseIndex = 0;
    private int dayCount = 1;
    private bool firstDayShown = false;

    void Start()
    {
        totalCycleDuration = 0f;
        foreach (var phase in phases)
            totalCycleDuration += phase.duration;

        if (startWithDay)
        {
            timeInCycle = 0f;
            for (int i = 0; i < phases.Length; i++)
            {
                if (phases[i].name == "Day")
                {
                    currentPhaseIndex = i;
                    break;
                }
                timeInCycle += phases[i].duration;
            }
        }
        else
        {
            timeInCycle = 0f;
            for (int i = 0; i < phases.Length; i++)
            {
                if (phases[i].name == "Night")
                {
                    currentPhaseIndex = i;
                    break;
                }
                timeInCycle += phases[i].duration;
            }
        }

        if (dayText != null)
        {
            var c = dayText.color;
            c.a = 0f;
            dayText.color = c;
        }

        if (startWithDay && phases[currentPhaseIndex].name == "Day")
        {
            ShowDayText();
            firstDayShown = true;
        }
    }

    void Update()
    {
        timeInCycle += Time.deltaTime;
        if (timeInCycle > totalCycleDuration)
            timeInCycle -= totalCycleDuration;

        float t = timeInCycle;
        int current = 0;
        while (t > phases[current].duration)
        {
            t -= phases[current].duration;
            current = (current + 1) % phases.Length;
        }
        int next = (current + 1) % phases.Length;
        float phaseT = t / phases[current].duration;

        Color sunCol = Color.Lerp(phases[current].sunColor, phases[next].sunColor, phaseT);
        float sunInt = Mathf.Lerp(phases[current].sunIntensity, phases[next].sunIntensity, phaseT);

        float cycleProgress = (timeInCycle / totalCycleDuration);
        sun.transform.localRotation = Quaternion.Euler((cycleProgress * 360f) - 90f, 170f, 0);

        sun.color = sunCol;
        sun.intensity = sunInt;
        RenderSettings.ambientIntensity = sunInt * 0.5f;

        if (current != currentPhaseIndex)
        {
            if (phases[current].name == "Day" && phases[currentPhaseIndex].name != "Day")
            {
                if (firstDayShown)
                    dayCount++;
                else
                    firstDayShown = true;

                ShowDayText();
            }
            currentPhaseIndex = current;
        }
    }

    void ShowDayText()
    {
        if (dayText != null)
        {
            dayText.text = "День " + dayCount;
            StopAllCoroutines();
            StartCoroutine(FadeTextRoutine());
        }
    }

    IEnumerator FadeTextRoutine()
    {
        float fadeInTime = 1f;
        float visibleTime = 3f;
        float fadeOutTime = 1f;

        float t = 0f;
        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            SetTextAlpha(Mathf.Lerp(0f, 1f, t / fadeInTime));
            yield return null;
        }
        SetTextAlpha(1f);

        yield return new WaitForSeconds(visibleTime);

        t = 0f;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            SetTextAlpha(Mathf.Lerp(1f, 0f, t / fadeOutTime));
            yield return null;
        }
        SetTextAlpha(0f);
    }

    void SetTextAlpha(float a)
    {
        if (dayText != null)
        {
            Color c = dayText.color;
            c.a = a;
            dayText.color = c;
        }
    }
}