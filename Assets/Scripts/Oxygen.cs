using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using UnityEngine.Serialization;
using UnityEditor;
using System;
using TMPro;

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

    public TextMeshProUGUI _oxygenLabel;

    public static Oxygen Instance;

    void Start()
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
        _oxygenLabel = _oxygenSlider.GetComponentInChildren<TextMeshProUGUI>();
        _currentOxygen = _maxOxygen;

        if (_oxygenSlider != null)
        {
            _oxygenSlider.maxValue = _maxOxygen;
            _oxygenSlider.value = _currentOxygen;
        }

        GridBatteryModifier.OnBatteryModification += GainOxygen;
    }

    void FixedUpdate()
    {
        string text = _currentOxygen == 10 ? _currentOxygen.ToString() : "0" + _currentOxygen.ToString();
        _oxygenLabel.text = text;

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

    public static event Action OnUnderOxygenThreshold;
    public static event Action OnOverOxygenThreshold;
    public void LoseOxygen()
    {
        _currentOxygen -= _lossOxygen;

        if (_currentOxygen < 5)
        {
            OnUnderOxygenThreshold?.Invoke();
        }
        else
        {
            OnOverOxygenThreshold?.Invoke();
        } 

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

    public void GainOxygen(int amount)
    {
        _currentOxygen += amount;
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