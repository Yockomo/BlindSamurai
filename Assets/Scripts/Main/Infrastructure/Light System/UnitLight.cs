using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class UnitLight : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector3 scaleMaxValue;
    [SerializeField] private float fightingTimeToScale;
    [SerializeField] private float inactiveTimeToScale;

    [Header("Time to scale when change to fighting state")]
    [SerializeField] private float fastScaleTime;

    public event Action<UnitLight> OnTurnOffAction; 
    
    private Sequence sequence;
    private Vector3 startScaleValue;

    private void Start()
    {
        if(scaleMaxValue.x < 1)
            scaleMaxValue.x += 1;
        
        if (scaleMaxValue.y < 1)
            scaleMaxValue.y += 1;

        if (scaleMaxValue.z < 1)
            scaleMaxValue.z += 1;
    }

    private void Awake()
    {
        startScaleValue = transform.localScale;
    }

    public void TurnOn()
    {
        gameObject.SetActive(true); 

        ScaleOverTime(startScaleValue, inactiveTimeToScale);
    }

    public void TurnOff()
    {
        ClearSequence();
        transform.localScale = startScaleValue;
        OnTurnOffAction?.Invoke(this);
        gameObject.SetActive(false);
    }

    public void ScaleUp()
    {
        ClearSequence();
        var maxScale = new Vector3((startScaleValue.x * scaleMaxValue.x), (startScaleValue.y * scaleMaxValue.y), (startScaleValue.z * scaleMaxValue.z)); 
        transform.DOScale(maxScale, fastScaleTime);
        
        ScaleOverTime(maxScale, fightingTimeToScale);
    }

    public void ScaleDown()
    {   
        ClearSequence();
        transform.DOScale(startScaleValue, fastScaleTime);
        ScaleOverTime(startScaleValue, inactiveTimeToScale);
    }

    private void ClearSequence()
    {
        sequence.Kill();
    }

    private void ScaleOverTime(Vector3 currentScale, float time)
    {
        var maxScale = GetMaxScaleValue(currentScale);

        sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(maxScale, time));
        sequence.Append(transform.DOScale(currentScale, time));
        sequence.SetLoops(-1, LoopType.Yoyo);
    }    
    
    private Vector3 GetMaxScaleValue(Vector3 startScale)
    {
        return new Vector3((startScale.x * scaleMaxValue.x), (startScale.y * scaleMaxValue.y), (startScale.z * scaleMaxValue.z));
    }
}