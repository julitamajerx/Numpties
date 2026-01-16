using UnityEngine;

public class DestroyOffScreen : MonoBehaviour
{
    private float screenBottomLimit;

    void Start()
    {
        screenBottomLimit = -Camera.main.orthographicSize - 2f;
    }

    void Update()
    {
        if (transform.position.y < screenBottomLimit)
        {
            Destroy(gameObject);
        }
    }
}