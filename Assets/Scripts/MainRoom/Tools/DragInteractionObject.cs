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

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponentInChildren<Image>();
        initialPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isAnyObjectActive) return;
        rectTransform.anchoredPosition += eventData.delta / rectTransform.localScale.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isAnyObjectActive)
        {
            rectTransform.anchoredPosition = initialPosition;
            return;
        }

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0f;
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, petLayer);

        if (hit.collider != null && hit.collider.CompareTag(petTag))
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
        else
        {
            rectTransform.anchoredPosition = initialPosition;
        }
    }

    private IEnumerator FulfillNeed()
    {
        petAnimator.SetBool(animationName, true);
        yield return new WaitForSeconds(animationDuration);
        petAnimator.SetBool(animationName, false);
        needBehaviour?.ResumeDecay();
        rectTransform.anchoredPosition = initialPosition;
        image.enabled = true;
        isAnyObjectActive = false;
    }

    public void StopSleep()
    {
        petAnimator.SetBool(sleepAnimationName, false);
        needBehaviour?.StopSleeping();
        isAnyObjectActive = false;
        rectTransform.anchoredPosition = initialPosition;
        image.enabled = true;
    }
}