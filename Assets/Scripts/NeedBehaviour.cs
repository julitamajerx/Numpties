using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NeedBehaviour : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private float decayDurationSeconds = 7200f;

    private const float BOOST_AMOUNT = 25f;
    private Coroutine decayCoroutine;

    void Start()
    {
        if (healthSlider == null)
        {
            enabled = false;
            return;
        }

        healthSlider.value = healthSlider.maxValue;
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

    private IEnumerator DecayOverTime()
    {
        float startValue = healthSlider.value;
        float elapsedTime = 0f;
        float remainingDuration = decayDurationSeconds * (healthSlider.value / healthSlider.maxValue);

        while (elapsedTime < remainingDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / remainingDuration);
            healthSlider.value = Mathf.Lerp(startValue, 0f, t);
            yield return null;
        }

        healthSlider.value = 0f;
    }

    public void BoostSliderValue()
    {
        healthSlider.value = Mathf.Min(healthSlider.value + BOOST_AMOUNT, healthSlider.maxValue);
        StartDecay();
    }
}
