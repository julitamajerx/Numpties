using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PetTouchMovement : MonoBehaviour
{
    [SerializeField] private MiniGameMoneyManager moneyManager;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Animator petAnimator;
    [SerializeField] private string animationName = "IsEating";
    [SerializeField] private float eatDuration = 1f;

    private Camera mainCam;
    private float minX, maxX;
    private float petHalfWidth;
    private Coroutine animationCoroutine;

    void Start()
    {
        mainCam = Camera.main;
        float screenAspect = (float)Screen.width / Screen.height;
        float cameraHeight = mainCam.orthographicSize * 2;
        float cameraWidth = cameraHeight * screenAspect;
        petHalfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
        minX = mainCam.transform.position.x - (cameraWidth / 2) + petHalfWidth;
        maxX = mainCam.transform.position.x + (cameraWidth / 2) - petHalfWidth;
    }

    void Update()
    {
        bool isTouching = false;
        Vector2 screenPos = Vector2.zero;

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            isTouching = true;
            screenPos = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            isTouching = true;
            screenPos = Mouse.current.position.ReadValue();
        }

        if (isTouching)
        {
            if (DragInteractionObject.isAnyObjectActive) return;
            MovePet(screenPos);
        }
    }

    void MovePet(Vector2 screenPos)
    {
        Vector3 worldPos = mainCam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, mainCam.nearClipPlane));
        float clampedX = Mathf.Clamp(worldPos.x, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            moneyManager.UpdateCoinsUI();
            Destroy(other.gameObject);

            if (animationCoroutine != null) StopCoroutine(animationCoroutine);
            animationCoroutine = StartCoroutine(PlayEatingAnimation());
        }

        if (other.CompareTag("GameOverObj"))
        {
            gameOverPanel.SetActive(true);
            Destroy(other.gameObject);
            Time.timeScale = 0f;
        }
    }

    private IEnumerator PlayEatingAnimation()
    {
        petAnimator.SetBool(animationName, true);
        yield return new WaitForSeconds(eatDuration);
        petAnimator.SetBool(animationName, false);
    }
}