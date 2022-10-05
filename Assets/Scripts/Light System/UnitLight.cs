using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class UnitLight : MonoBehaviour
{
    [SerializeField] private Vector3 scaleMaxValue;
    [SerializeField] private float timeToScale;
    
    [Header("If looped animation")]
    [SerializeField] private bool doScaleOverTime;

    public event Action<UnitLight> OnTurnOffAction; 
    
    private Sequence sequence;
    private Vector3 startScaleValue;

    public void TurnOn()
    {
        gameObject.SetActive(true);
        startScaleValue = transform.localScale;
        if (doScaleOverTime)
        {
            ScaleOverTime();
        }
    }

    public void TurnOff()
    {
        gameObject.SetActive(false);
        transform.parent = null;
        sequence.Kill();
        transform.localScale = startScaleValue;
        OnTurnOffAction?.Invoke(this);
    }

    public void ScaleUp()
    {
        transform.DOScale(scaleMaxValue, timeToScale);
    }

    public void ScaleDown()
    {
        transform.DOScale(startScaleValue, timeToScale);
    }
    
    private void ScaleOverTime()
    {
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(scaleMaxValue, timeToScale));
        sequence.SetLoops(-1, LoopType.Restart);
    }
}