using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class Bar : MonoBehaviour
{
    Slider slider;

    [SerializeField]
    private float maxValue = 100f;
    [SerializeField]
    private float currentValue = 100f;

    public float CurrentValue
    {
        get { return currentValue; }
    }

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = maxValue;
        slider.value = currentValue;
    }

    public void SetValue(float endValue)
    {
        DOTween.To(() => currentValue, x => currentValue = x, endValue, 0.5f).OnUpdate(() =>
        {
            slider.value = currentValue;
        });
    }
}