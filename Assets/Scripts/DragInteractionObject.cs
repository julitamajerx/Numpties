using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class DragInteractionObject : MonoBehaviour, IDragHandler, IEndDragHandler
{
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
        rectTransform.anchoredPosition += eventData.delta / rectTransform.localScale.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0f;

        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, petLayer);

        if (hit.collider != null && hit.collider.CompareTag(petTag))
        {
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

            image.enabled = false;
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
    }

    public void StopSleep()
    {
        needBehaviour?.StopSleeping();
        petAnimator.SetBool(sleepAnimationName, false);
    }
}
