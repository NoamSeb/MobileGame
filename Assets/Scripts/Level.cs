using NaughtyAttributes;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
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
    public Vector3 LevelCenter { get; private set; }

    private GameObject _initialStateBackup;

    private void Awake()
    {
        GetNeededComponents();

        _initialStateBackup = new("Backup");

        foreach(Transform transf in PlayGrid.transform)
        {
            transf.gameObject.TryGetComponent(out Tilemap tempMap);
            tempMap.CompressBounds();
        }

        Tilemap map = PlayGrid.transform.childCount switch
        {
            1 => PlayGrid.GetComponentInChildren<Tilemap>(),

            2 => PlayGrid.transform.GetChild(0).GetComponent<Tilemap>(),

            _ => throw new ArgumentException($"{gameObject.name} has no tilemap or too much tilemaps")
        };

        BoundsInt bounds = map.cellBounds;
        Vector3 correctionX = Vector3.zero, correctionX2 = Vector3.zero, correctionY = Vector3.zero;
        if (bounds.size.y % 2 != 0) { correctionY = Vector3.up / 2; }

        if (PlayGrid.transform.childCount == 1)
        {
            Vector3Int point = new(Mathf.FloorToInt(bounds.center.x), Mathf.FloorToInt(bounds.center.y), 0);
            Vector3 realpoint = map.CellToWorld(point);
            if (bounds.size.x % 2 != 0) { correctionX = Vector3.right / 2; }

            float mapCenterXCorrection = bounds.size.x / 4.666f;
            correctionX2 = new(mapCenterXCorrection, 0, 0);

            LevelCenter = realpoint + correctionX + correctionX2 + correctionY;
        }
        else
        {
            Vector3Int point = new(Mathf.FloorToInt(bounds.max.x), Mathf.FloorToInt(bounds.center.y), 0);
            Vector3 realpoint = map.CellToWorld(point);

            float mapCenterXCorrection = bounds.size.x / 3f;
            correctionX2 = new(mapCenterXCorrection, 0, 0);

            LevelCenter = realpoint + correctionX + correctionX2 +correctionY;
        }
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

