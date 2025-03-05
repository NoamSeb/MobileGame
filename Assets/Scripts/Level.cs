using NaughtyAttributes;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    [SerializeField, Range(2f, 5f)] float _neededCameraSize;

    public float CameraSize { get { return _neededCameraSize; } }
    public Grid PlayGrid { get; private set; }
    public Slider Slider { get; private set; }
    public PlayerGridMovement Movement { get; private set; }
    public Oxygen Oxygen { get; private set; }
    public PlayerMirrorMovement MirrorMovement { get; private set; }

    private GameObject _initialStateBackup;
    private bool _isFirstTimeBackingUp;

    private void Awake()
    {
        GetNeededComponents();

        _initialStateBackup = new("Backup");

        
    }

    void GetNeededComponents()
    {
        PlayGrid = GetComponentInChildren<Grid>(true);
        Slider = GetComponentInChildren<Slider>(true);
        Movement = GetComponentInChildren<PlayerGridMovement>(true);
        Oxygen = GetComponentInChildren<Oxygen>(true);
        MirrorMovement = GetComponentInChildren<PlayerMirrorMovement>(true);
    }

    public static event Action<Level> OnLevelLoad;
    private void OnEnable()
    {
        GetNeededComponents();

        if (GameManager.Instance != null && !GameManager.Instance.IsAwake && _initialStateBackup.transform.childCount == 0)
        {
            foreach (Transform obj in transform)
            {
                Instantiate(obj.gameObject, _initialStateBackup.transform);
            }

            _initialStateBackup.SetActive(false);
        }

        OnLevelLoad?.Invoke(this);
    }

    private void OnDisable()
    {
        if (!GameManager.Instance.IsAwake)
        {
            if (_initialStateBackup != null) 
            { 
                _initialStateBackup.SetActive(true);
                foreach (Transform obj in transform)
                {
                    Destroy(obj.gameObject);
                }
                foreach (Transform obj in _initialStateBackup.transform)
                {
                    Instantiate(obj.gameObject, transform);
                }
                if (PlayGrid == null) { throw new Exception(); }
                _initialStateBackup.SetActive(false);
            }
        }
    }
}

