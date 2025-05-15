using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner_House2 : MonoBehaviour
{
    public GameObject House2;

    public Transform stoneMushesParent;
    public Transform woodMushesParent;
    public Transform bobersParent;

    [Header("Spawn Settings")]
    public float initialSpawnDelay = 300f;
    public float spawnInterval = 120f;
    public int maxActive = 5;

    private List<GameObject> allAnimals = new List<GameObject>();
    private List<GameObject> stoneAnimals = new List<GameObject>();
    private List<GameObject> woodAnimals = new List<GameObject>();
    private List<GameObject> boberAnimals = new List<GameObject>();

    private List<List<GameObject>> firstRoundLists = new List<List<GameObject>>();
    private int firstRoundIndex = 0;
    private bool firstRoundDone = false;

    void Start()
    {
        foreach (Transform child in stoneMushesParent)
        {
            allAnimals.Add(child.gameObject);
            stoneAnimals.Add(child.gameObject);
        }
        foreach (Transform child in woodMushesParent)
        {
            allAnimals.Add(child.gameObject);
            woodAnimals.Add(child.gameObject);
        }
        foreach (Transform child in bobersParent)
        {
            allAnimals.Add(child.gameObject);
            boberAnimals.Add(child.gameObject);
        }

        foreach (var obj in allAnimals)
            obj.SetActive(false);

        firstRoundLists.Add(stoneAnimals);
        firstRoundLists.Add(woodAnimals);
        firstRoundLists.Add(boberAnimals);

        Shuffle(firstRoundLists);

        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(initialSpawnDelay);

        while (true)
        {
            if (House2 == null || !House2.activeInHierarchy)
            {
                yield return null;
                continue;
            }

            int activeCount = 0;
            foreach (var obj in allAnimals)
                if (obj.activeSelf) activeCount++;

            if (activeCount >= maxActive)
            {
                yield return new WaitForSeconds(spawnInterval);
                continue;
            }

            if (!firstRoundDone)
            {
                if (firstRoundIndex < firstRoundLists.Count)
                {
                    var list = firstRoundLists[firstRoundIndex];
                    GameObject toSpawn = GetRandomInactive(list);
                    if (toSpawn != null)
                    {
                        toSpawn.SetActive(true);
                        firstRoundIndex++;
                        yield return new WaitForSeconds(spawnInterval);
                        continue;
                    }
                    else
                    {
                        firstRoundIndex++;
                        continue;
                    }
                }
                else
                {
                    firstRoundDone = true;
                }
            }

            List<GameObject> inactiveAnimals = new List<GameObject>();
            foreach (var obj in allAnimals)
                if (!obj.activeSelf) inactiveAnimals.Add(obj);

            if (inactiveAnimals.Count > 0)
            {
                int randIndex = Random.Range(0, inactiveAnimals.Count);
                inactiveAnimals[randIndex].SetActive(true);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    GameObject GetRandomInactive(List<GameObject> list)
    {
        List<GameObject> inactive = new List<GameObject>();
        foreach (var obj in list)
            if (!obj.activeSelf) inactive.Add(obj);

        if (inactive.Count == 0) return null;
        return inactive[Random.Range(0, inactive.Count)];
    }
    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}