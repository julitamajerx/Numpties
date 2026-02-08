using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class DragInteractionObject : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public static bool isAnyObjectActive = false;

    [SerializeField] private NeedBehaviour needBehaviour;
    [SerializeField] private Animator petAnimator;
    [SerializeField] private LayerMask petLayer;
    [SerializeField] private string petTag;
    [SerializeField] private string animationName = "IsEating";
    [SerializeField] private string sleepAnimationName = "IsSleeping";
    [SerializeField] private float animationDuration = 5f;

    private RectTransform rectTransform;
    private Image image;
    private Vector2 initialPosition;
    private Canvas canvas;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponentInChildren<Image>();
        initialPosition = rectTransform.anchoredPosition;
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isAnyObjectActive) return;

        // Na Androidzie bezpieczniej jest dzieliæ delta przez scaleFactor canvasu
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isAnyObjectActive)
        {
            ResetPosition();
            return;
        }

        // Pobieramy kamerê, która renderuje UI lub g³ówn¹ kamerê
        Camera cam = eventData.pressEventCamera != null ? eventData.pressEventCamera : Camera.main;

        if (cam != null)
        {
            // Konwersja pozycji dotyku/myszki na œwiat 2D
            Vector3 worldPos = cam.ScreenToWorldPoint(eventData.position);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, petLayer);

            if (hit.collider != null && hit.collider.CompareTag(petTag))
            {
                StartInteraction();
                return;
            }
        }

        ResetPosition();
    }

    private void StartInteraction()
    {
        isAnyObjectActive = true;
        image.enabled = false;

        if (animationName == sleepAnimationName)
        {
            needBehaviour?.StartSleeping();
            petAnimator.SetBool(sleepAnimationName, true);
        }
        else
        {
            needBehaviour?.PauseDecay();
            needBehaviour?.BoostSliderValue();
            StartCoroutine(FulfillNeed());
        }
    }

    private void ResetPosition()
    {
        rectTransform.anchoredPosition = initialPosition;
    }

    private IEnumerator FulfillNeed()
    {
        petAnimator.SetBool(animationName, true);
        yield return new WaitForSeconds(animationDuration);
        petAnimator.SetBool(animationName, false);

        needBehaviour?.ResumeDecay();
        ResetPosition();
        image.enabled = true;
        isAnyObjectActive = false;
    }

    public void StopSleep()
    {
        petAnimator.SetBool(sleepAnimationName, false);
        needBehaviour?.StopSleeping();
        isAnyObjectActive = false;
        ResetPosition();
        image.enabled = true;
    }
}