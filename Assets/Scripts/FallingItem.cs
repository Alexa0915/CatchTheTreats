using UnityEngine;

public class FallingItem : MonoBehaviour
{
    public float speed = 3f;
    public bool isGood = true;
    public bool isBomb = false;

    [Range(0.2f, 1f)] public float itemHitbox = 0.6f;
    [Range(0.2f, 1f)] public float playerHitbox = 0.6f;

    SpriteRenderer sr;
    SpriteRenderer playerSr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        PlayerController player = FindAnyObjectByType<PlayerController>();
        playerSr = player.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        Bounds a = playerSr.bounds;
        a.size = a.size * playerHitbox;

        Bounds b = sr.bounds;
        b.size = b.size * itemHitbox;

        bool hit = a.min.x < b.max.x && a.max.x > b.min.x &&
                   a.min.y < b.max.y && a.max.y > b.min.y;

        GameManager gm = GameManager.Instance;

        if (hit)
        {
            if (isGood) gm.CatchFood();
            else if (isBomb) gm.CatchBomb();
            else gm.CatchBad();
            Destroy(gameObject);
            return;
        }

        float bottom = -Camera.main.orthographicSize - 1f;
        if (transform.position.y < bottom)
        {
            if (isGood) gm.MissFood();
            Destroy(gameObject);
        }
    }
}