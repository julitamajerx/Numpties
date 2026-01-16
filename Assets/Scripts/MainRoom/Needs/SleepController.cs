using UnityEngine;

public class SleepController : MonoBehaviour
{
    [SerializeField] private NeedBehaviour sleepNeed;
    [SerializeField] private ClickableObject lamp;
    [SerializeField] private Animator petAnimator;
    [SerializeField] private string sleepAnimationName = "IsSleeping";

    void Start()
    {
        sleepNeed.OnSleepEnded += () =>
        {
            petAnimator.SetBool(sleepAnimationName, false);
            DragInteractionObject.isAnyObjectActive = false;
        };
    }

    void Update()
    {
        if (!lamp.IsOn() && !sleepNeed.isSleeping && sleepNeed.needSlider.value < sleepNeed.needSlider.maxValue)
        {
            sleepNeed.StartSleeping();
            petAnimator.SetBool(sleepAnimationName, true);

            DragInteractionObject.isAnyObjectActive = true;
        }

        if ((lamp.IsOn() || sleepNeed.needSlider.value >= sleepNeed.needSlider.maxValue) && sleepNeed.isSleeping)
        {
            sleepNeed.StopSleeping();
            petAnimator.SetBool(sleepAnimationName, false);

            DragInteractionObject.isAnyObjectActive = false;
        }
    }
}