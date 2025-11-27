using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class DragInteractionObject : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private NeedBehaviour gameManager;
    [SerializeField] private Animator petAnimator;

    [SerializeField] private LayerMask petLayer;
    [SerializeField] private string petTag;

    private RectTransform rectTransform;
    private Image image;
    private Vector2 initialPosition;

    private const string IS_EATING = "IsEating";
    private const float EAT_DURATION = 5f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponentInChildren<Image>();
        initialPosition = rectTransform.anchoredPosition;

        if (image == null)
            Debug.LogError("Image component wasn't found.");
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / rectTransform.localScale.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0;

        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, petLayer);

        if (hit.collider != null && hit.collider.CompareTag(petTag))
        {
            gameManager?.BoostSliderValue();
            StartCoroutine(EatRoutine());
            image.enabled = false;
        }
        else
        {
            rectTransform.anchoredPosition = initialPosition;
        }
    }

    private IEnumerator EatRoutine()
    {
        petAnimator.SetBool(IS_EATING, true);

        yield return new WaitForSeconds(EAT_DURATION);

        petAnimator.SetBool(IS_EATING, false);

        rectTransform.anchoredPosition = initialPosition;
        image.enabled = true;
    }
}
