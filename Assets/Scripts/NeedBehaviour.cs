using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class NeedBehaviour : MonoBehaviour
{
    [SerializeField] public Slider needSlider;
    [SerializeField] private float decayDurationSeconds = 7200f;
    [SerializeField] private float boostAmount = 25f;
    [SerializeField] private float sleepDuration = 10f;

    private Coroutine decayCoroutine;
    private Coroutine sleepCoroutine;
    public event Action OnSleepEnded;

    public bool isSleeping = false;

    void Start()
    {
        if (needSlider == null)
        {
            enabled = false;
            return;
        }

        needSlider.value = needSlider.maxValue;
        StartDecay();
    }

    public void StartDecay()
    {
        if (decayCoroutine != null)
        {
            StopCoroutine(decayCoroutine);
        }
        decayCoroutine = StartCoroutine(DecayOverTime());
    }

    public void PauseDecay()
    {
        if (decayCoroutine != null)
        {
            StopCoroutine(decayCoroutine);
            decayCoroutine = null;
        }
    }

    public void ResumeDecay()
    {
        StartDecay();
    }

    private IEnumerator DecayOverTime()
    {
        float startValue = needSlider.value;
        float elapsedTime = 0f;
        float remainingDuration = decayDurationSeconds * (needSlider.value / needSlider.maxValue);

        while (elapsedTime < remainingDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / remainingDuration);
            needSlider.value = Mathf.Lerp(startValue, 0f, t);
            yield return null;
        }

        needSlider.value = 0f;
    }

    public void BoostSliderValue()
    {
        if (needSlider == null) return;
        needSlider.value = Mathf.Min(needSlider.value + boostAmount, needSlider.maxValue);
    }

    public void StartSleeping()
    {
        if (isSleeping) return;
        isSleeping = true;
        PauseDecay();
        if (sleepCoroutine != null) StopCoroutine(sleepCoroutine);
        sleepCoroutine = StartCoroutine(SleepRoutine());
    }

    public void StopSleeping()
    {
        if (!isSleeping) return;
        isSleeping = false;
        if (sleepCoroutine != null)
        {
            StopCoroutine(sleepCoroutine);
            sleepCoroutine = null;
        }
        ResumeDecay();
    }

    private IEnumerator SleepRoutine()
    {
        float targetValue = needSlider.maxValue * 0.995f;

        while (isSleeping && needSlider.value < targetValue)
        {
            needSlider.value += (needSlider.maxValue / sleepDuration) * Time.deltaTime;
            yield return null;
        }

        needSlider.value = targetValue;
        isSleeping = false;
        ResumeDecay();
        OnSleepEnded?.Invoke();
    }



}
