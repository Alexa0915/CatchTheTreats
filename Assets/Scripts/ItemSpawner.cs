using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] goodItems;
    public GameObject[] badItems;

    [Header("Початок гри")]
    public float startInterval = 1.2f;
    [Range(0f, 1f)] public float startBadChance = 0.25f;

    [Header("Максимальна складність")]
    public float minInterval = 0.5f;
    [Range(0f, 1f)] public float maxBadChance = 0.4f;
    public float maxSpeedMultiplier = 2.2f;

    [Header("За скільки секунд гра стає найважчою")]
    public float timeToMax = 90f;

    float elapsed;
    float timer;

    void Update()
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / timeToMax);

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Spawn(t);
            timer = Mathf.Lerp(startInterval, minInterval, t);
        }
    }

    void Spawn(float t)
    {
        float badChance = Mathf.Lerp(startBadChance, maxBadChance, t);
        GameObject[] pool = (Random.value < badChance && badItems.Length > 0) ? badItems : goodItems;
        if (pool.Length == 0) return;

        GameObject prefab = pool[Random.Range(0, pool.Length)];
        Camera cam = Camera.main;
        float halfWidth = cam.orthographicSize * cam.aspect - 0.5f;
        float x = Random.Range(-halfWidth, halfWidth);
        float y = cam.orthographicSize + 1f;

        GameObject item = Instantiate(prefab, new Vector3(x, y, 0f), Quaternion.identity);

        FallingItem fi = item.GetComponent<FallingItem>();
        if (fi != null)
            fi.speed *= Mathf.Lerp(1f, maxSpeedMultiplier, t);
    }
}