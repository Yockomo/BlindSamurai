using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class UnitLight : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private float minScaleValue;
    [SerializeField] private float maxScaleValue;
    [SerializeField] private float timeToScaleUp;
    [SerializeField] private float timeToScaleDown;
    [SerializeField] private float baseScaleTime;
    [SerializeField] private float fightingTimeToScale;
    [SerializeField] private float inactiveTimeToScale;

    [Header("Time to scale when change to fighting state")]
    [SerializeField] private float fastScaleTime;

    public event Action<UnitLight> OnTurnOffAction; 
    
    private Sequence sequence;
    private Vector3 startScaleValue;

    private void Awake()
    {
        startScaleValue = transform.localScale;
    }

    public void TurnOn()
    {
        gameObject.SetActive(true); 
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(maxScaleValue, inactiveTimeToScale));
        sequence.Append(transform.DOScale(startScaleValue, inactiveTimeToScale));
        sequence.SetLoops(-1, LoopType.Yoyo);
    }

    public void TurnOff()
    {
        ClearSequence();
        transform.localScale = startScaleValue;
        OnTurnOffAction?.Invoke(this);
        gameObject.SetActive(false);
    }

    public void Scale(float value)
    {
        ClearSequence();
        transform.DOScale(value, baseScaleTime);
    }
    
    public void ScaleUp()
    {
        ScaleLightUp(timeToScaleUp);
    }

    public void ScaleUp(float time)
    {
        ScaleLightUp(time);
    }

    public void ScaleDown()
    {   
        ScaleLightDown(timeToScaleDown);
    }

    public void ScaleDown(float time)
    {
        ScaleLightDown(time);
    }
    
    private void ScaleLightUp(float time)
    {
        ClearSequence();
        transform.DOScale(maxScaleValue, time);
    }

    private void ScaleLightDown(float time)
    {
        ClearSequence();
        transform.DOScale(minScaleValue, time);
    }

    private void ClearSequence()
    {
        sequence.Kill();
    }
}