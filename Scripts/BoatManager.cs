using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class BoatManager : MonoBehaviour
{
    [Header("Cameras")]
    public Camera CameraIsl1;
    public Camera CameraIsl2;
    public Camera CameraIsl3;

    [Header("Islands")]
    public GameObject Island2;
    public GameObject Island3;

    [Header("UI")]
    public CanvasGroup BlackPanel;
    public TMP_Text IslandText;

    [Header("Boats")]
    public GameObject Boat1n;
    public GameObject Boat2n;
    public GameObject Boat2b;
    public GameObject Boat3b;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip errorClip;
    [Range(0, 1)] public float errorVolume = 1f;
    public AudioClip moveBoatsClip;
    [Range(0, 1)] public float moveBoatsVolume = 1f;

    private void Start()
    {
        AddBoatHandler(Boat1n, BoatType.Boat1n);
        AddBoatHandler(Boat2n, BoatType.Boat2n);
        AddBoatHandler(Boat2b, BoatType.Boat2b);
        AddBoatHandler(Boat3b, BoatType.Boat3b);

        UpdateIslandText();
    }

    private void AddBoatHandler(GameObject boat, BoatType type)
    {
        if (boat.GetComponent<BoatClickHandler>() == null)
            boat.AddComponent<BoatClickHandler>().Init(this, type);
    }

    public void OnBoatClicked(BoatType type)
    {
        switch (type)
        {
            case BoatType.Boat1n:
                if (Island2 != null && !Island2.activeSelf)
                {
                    PlayError();
                    return;
                }
                PlayMoveBoats();
                StartCoroutine(SwitchCameraRoutine(CameraIsl1, CameraIsl2));
                break;
            case BoatType.Boat2b:
                PlayMoveBoats();
                StartCoroutine(SwitchCameraRoutine(CameraIsl2, CameraIsl1));
                break;
            case BoatType.Boat2n:
                if (Island3 != null && !Island3.activeSelf)
                {
                    PlayError();
                    return;
                }
                PlayMoveBoats();
                StartCoroutine(SwitchCameraRoutine(CameraIsl2, CameraIsl3));
                break;
            case BoatType.Boat3b:
                PlayMoveBoats();
                StartCoroutine(SwitchCameraRoutine(CameraIsl3, CameraIsl2));
                break;
        }
    }

    IEnumerator SwitchCameraRoutine(Camera from, Camera to)
    {
        yield return StartCoroutine(FadeCanvasGroup(BlackPanel, 0, 1, 1f));

        from.gameObject.SetActive(false);
        to.gameObject.SetActive(true);

        UpdateIslandText();

        yield return StartCoroutine(FadeCanvasGroup(BlackPanel, 1, 0, 1f));
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float from, float to, float duration)
    {
        float t = 0;
        cg.alpha = from;
        cg.blocksRaycasts = true;
        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        cg.alpha = to;
        cg.blocksRaycasts = to > 0.01f;
    }

    void UpdateIslandText()
    {
        if (CameraIsl1.gameObject.activeSelf)
            IslandText.text = "Остров 1";
        else if (CameraIsl2.gameObject.activeSelf)
            IslandText.text = "Остров 2";
        else if (CameraIsl3.gameObject.activeSelf)
            IslandText.text = "Остров 3";
    }

    void PlayError()
    {
        if (audioSource && errorClip)
            audioSource.PlayOneShot(errorClip, errorVolume);
    }

    void PlayMoveBoats()
    {
        if (audioSource && moveBoatsClip)
            audioSource.PlayOneShot(moveBoatsClip, moveBoatsVolume);
    }
}

public enum BoatType { Boat1n, Boat2n, Boat2b, Boat3b }

public class BoatClickHandler : MonoBehaviour
{
    BoatManager manager;
    BoatType type;

    public void Init(BoatManager m, BoatType t)
    {
        manager = m;
        type = t;
    }

    void OnMouseDown()
    {
        manager.OnBoatClicked(type);
    }
}