using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using UnityEngine.Serialization;

public class Oxygen : MonoBehaviour
{
    [Header("Oxygen values")]
    [ValidateInput("IsGreaterThanZero", "The value must be greater than 0.")] [SerializeField]
    int _maxOxygen;
    [SerializeField] private int _lossOxygen;
    [SerializeField] int _gainOxygen;
    
    [ProgressBar("Oxygen", nameof(_maxOxygen), EColor.Green)] [SerializeField]
    int _currentOxygen;
    
    [Header("Oxygen Slider")]
    [SerializeField] Slider _oxygenSlider;

    [Header("Smooth speeds")]
    [SerializeField] float _lerpSpeed = 5f;

    void Awake()
    {
        _currentOxygen = _maxOxygen;
        if (_oxygenSlider != null)
        {
            _oxygenSlider.maxValue = _maxOxygen;
            _oxygenSlider.value = _currentOxygen;
        }
    }
    
    void FixedUpdate()
    {
        if (_oxygenSlider != null) _oxygenSlider.value = Mathf.Lerp(_oxygenSlider.value, _currentOxygen, Time.fixedDeltaTime * _lerpSpeed); 
        
        if (_currentOxygen == 0) Die();
    }

    [Button("Loss Oxygen")]
    void LoseOxygen()
    {
        LoseOxygen(_lossOxygen);
    }
    private void LoseOxygen(int _value)
    {
        _currentOxygen -= _value;
    }

    [Button("Get Oxygen")]
    void GetOxygen()
    {
        _currentOxygen += _gainOxygen;
    }

    [Button("Reset Oxygen")]
    private void ResetOxygen()
    {
        _currentOxygen = _maxOxygen;
    }
    
    private void Die()
    {
        Debug.Log("You are dead ! Loser !");
    }
}
