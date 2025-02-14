using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using UnityEngine.Serialization;
using UnityEditor;
using System;

public class Oxygen : MonoBehaviour
{
    [ValidateInput(nameof(IsGreaterThanZero), "The value must be greater than 0."), SerializeField, BoxGroup("Oxygen Values")]
    int _maxOxygen, _lossOxygen;
    [SerializeField, BoxGroup("Oxygen Values")] int _gainOxygen;
    bool IsGreaterThanZero(int n) => n > 0;

    [ProgressBar("Oxygen", nameof(_maxOxygen), EColor.Green)]
    [SerializeField]
    int _currentOxygen;

    private Slider _oxygenSlider;

    [BoxGroup("Smooth speed")]
    [SerializeField] float _lerpSpeed = 5f;

    public static Oxygen Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _oxygenSlider = GameManager.Instance.OxygenSlider;
        _currentOxygen = _maxOxygen;

        if (_oxygenSlider != null)
        {
            _oxygenSlider.maxValue = _maxOxygen;
            _oxygenSlider.value = _currentOxygen;
        }

        GridOxygenBottle.OnOxygenBottleRefill += GainOxygen;
    }

    void FixedUpdate()
    {
        if (_oxygenSlider != null) _oxygenSlider.value = Mathf.Lerp(_oxygenSlider.value, _currentOxygen, Time.fixedDeltaTime * _lerpSpeed);

        if (_currentOxygen == 0) Die();
    }

    [Button("Loss Oxygen")]
    void LossOxygen()
    {
        LossOxygen(_lossOxygen);
    }
    private void LossOxygen(int _value)
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

    public bool IsDead()
    {
        return _currentOxygen <= 0;
    }

    public void LoseOxygen()
    {
        _currentOxygen -= _lossOxygen;

        if (_currentOxygen <= 0)
        {
            _currentOxygen = 0;
            Die();
        }
    }

    public void SetOxygenToZero()
    {
        _currentOxygen = 0;
    }

    void GainOxygen(int amount)
    {
        if (amount <= 0) { throw new ArgumentException("The value should be a strict positive"); }
        _currentOxygen = Mathf.Clamp(_currentOxygen + amount, _currentOxygen, _maxOxygen);
    }

    private void Die()
    {
        Debug.Log("You are dead ! Loser !");
    }

    public void StopPlayer()
    {
        SetOxygenToZero();
        IsDead();
        PlayerGridMovement.Instance.StopMovement();
        Debug.Log("Le joueur est touché par un laser !");
    }
}