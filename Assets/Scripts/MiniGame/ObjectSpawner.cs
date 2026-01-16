using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Ustawienia Obiektów")]
    [SerializeField] private GameObject frequentObject;
    [SerializeField] private GameObject rareObject;

    [Header("Ustawienia Grawitacji")]
    [SerializeField] private float startGravity = 0.5f;
    [SerializeField] private float endGravity = 0.9f;

    [Header("Ustawienia Szansy")]
    [Range(0, 100)][SerializeField] private int startFrequentChance = 80;
    [Range(0, 100)][SerializeField] private int endFrequentChance = 35;

    [Header("Ustawienia Trudnoœci")]
    [SerializeField] private float startSpawnInterval = 2f;
    [SerializeField] private float minSpawnInterval = 0.2f;
    [SerializeField] private float speedUpFactor = 0.03f;

    private float currentInterval;
    private Camera mainCam;
    private float minX, maxX;

    void Start()
    {
        mainCam = Camera.main;
        currentInterval = startSpawnInterval;
        CalculateBounds();
        StartCoroutine(SpawnRoutine());
    }

    void CalculateBounds()
    {
        float screenAspect = (float)Screen.width / Screen.height;
        float cameraHeight = mainCam.orthographicSize * 2;
        float cameraWidth = cameraHeight * screenAspect;

        minX = mainCam.transform.position.x - (cameraWidth / 2) + 0.5f;
        maxX = mainCam.transform.position.x + (cameraWidth / 2) - 0.5f;
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentInterval);
            SpawnObject();

            if (currentInterval > minSpawnInterval)
            {
                currentInterval -= speedUpFactor;
                if (currentInterval < minSpawnInterval) currentInterval = minSpawnInterval;
            }
        }
    }

    void SpawnObject()
    {
        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPos = new Vector3(randomX, mainCam.orthographicSize + 1f, 0);

        float t = Mathf.InverseLerp(startSpawnInterval, minSpawnInterval, currentInterval);

        float currentChance = Mathf.Lerp(startFrequentChance, endFrequentChance, t);
        float currentGravity = Mathf.Lerp(startGravity, endGravity, t);

        GameObject prefabToSpawn;
        int roll = Random.Range(0, 101);

        if (roll <= currentChance) prefabToSpawn = frequentObject;
        else prefabToSpawn = rareObject;

        GameObject spawnedObj = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

        Rigidbody2D rb = spawnedObj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = currentGravity;
        }
    }
}