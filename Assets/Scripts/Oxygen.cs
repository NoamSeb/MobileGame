using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using UnityEditor;
using System;
using TMPro;
using System.Collections;

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

    void Start()
    {
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
        string text = (_currentOxygen * 10).ToString() + "%";
        _oxygenLabel.text = text;

        if (_oxygenSlider != null) _oxygenSlider.value = Mathf.Lerp(_oxygenSlider.value, _currentOxygen, Time.fixedDeltaTime * _lerpSpeed);
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
        Die();
    }

    public void GainOxygen(int amount)
    {
        _currentOxygen += amount;

        if (_currentOxygen < 5)
        {
            OnUnderOxygenThreshold?.Invoke();
        }
        else
        {
            OnOverOxygenThreshold?.Invoke();
        }
    }

    public static event Action OnDeath;
    private void Die()
    {
        StartCoroutine(DeathCoroutine());
    }

    IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(0.1f);

        if (_currentOxygen <= 0)
        {
            Debug.Log("Le joueur est mort, il ne peut plus bouger !");

            GameManager.Instance.PlayerScript.StopMovement();
            if (GameManager.Instance.MirrorScript != null)
            {
                GameManager.Instance.MirrorScript.StopMovement();
            }

            GameManager.Instance.PlayerScript.DisableActions();
            if (GameManager.Instance.MirrorScript != null)
            {
                GameManager.Instance.MirrorScript.DisableActions();
            }

            OnDeath?.Invoke();
        }
    }


    public void StopPlayer()
    {
        SetOxygenToZero();
        GameManager.Instance.PlayerScript.StopMovement();
        Debug.Log("Le joueur est touché par un laser !");
    }
}