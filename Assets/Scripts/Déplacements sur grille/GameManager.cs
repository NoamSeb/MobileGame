using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    //L'objet GameManager est un singleton pr�sent dans chaque sc�ne
    public static GameManager Instance;
    public static int CurrentLevelID { get; set; }
    public Grid PlayGrid { get; private set; }
    public Slider OxygenSlider { get; private set; }
    public PlayerGridMovement PlayerScript { get; private set; }
    public Oxygen PlayerOxygen { get; private set; }
    public PlayerMirrorMovement MirrorScript { get; private set; }
    public bool IsAwake { get; private set; }

    private float _cameraSize;
    private Vector3 _cameraPos;
    private float _inactivityThreshold = 300f; // 5 minutes
    private float _inactivityTimer = 0f;

    [SerializeField] GameObject _victoryCanvas;
    [SerializeField] GameObject _defeatCanvas;
    [SerializeField] GameObject _tabletteLoadGame;
    [SerializeField] GameObject _tabletteUnloadGame;

    public GameObject VictoryCanvas { get { return _victoryCanvas; } }
    public GameObject DefeatCanvas { get { return _defeatCanvas; } }
    public GameObject GameLoadTablette { get { return _tabletteLoadGame; } }
    public GameObject GameUnloadTablette { get { return _tabletteUnloadGame; } }
    
    private void Awake()
    {
        if (Instance == null || Instance != this)
        {
            Instance = this;
        }

        IsAwake = true;
        Level.OnLevelLoad += SetUp;
    }


    private void Start()
    {
        IsAwake = false;
    }

    void SetUp(Level level)
    {
        PlayGrid = level.PlayGrid;
        OxygenSlider = level.Slider;
        PlayerScript = level.Movement;
        PlayerOxygen = level.Oxygen;
        MirrorScript = level.MirrorMovement;

        _cameraSize = level.CameraSize;
        _cameraPos = level.LevelCenter;
    }

    private void Update()
    {
        if (CurrentLevelID != 0)
        {
            Camera.main.orthographicSize = _cameraSize;
            Camera.main.transform.position = new(_cameraPos.x, _cameraPos.y, -10);
        }
        else
        {
            Camera.main.orthographicSize = 5;
            Camera.main.transform.position = new(0, 0, -10);
        }

        CheckPlayerActivity();
    }

    public static void OnSave(int currentScore)
    {
        PlayerData playerData = SaveSystem.LoadPlayer();

        if (playerData != null)
        {
            var dataElement = playerData.data.FirstOrDefault(x => x.idLevel == CurrentLevelID);

            if (dataElement.idLevel != 0)
            {
                if (dataElement.score > currentScore)
                    return;
                else
                {
                    var idElement = playerData.data.IndexOf(dataElement);
                    dataElement.score = currentScore;
                    playerData.data[idElement] = dataElement;
                }
            }
            else
            {
                BiomeManager bm = FindFirstObjectByType<BiomeManager>();

                if (bm != null) // Ensure bm is found before using it
                {
                    BiomeManager.BiomeStructure currentBiome = bm.Biomes.Find(x => x.idBiome == bm._currentBiomeID); // Corrected predicate logic

                    if (currentBiome.biome != null) // Ensure currentBiome is valid
                    {
                        playerData.data.Add(new PlayerData.DataElement(CurrentLevelID, currentScore, currentBiome.idBiome)); // Use currentBiome.biome
                    }
                }
                else
                {
                    Debug.LogError("BiomeManager not found in the scene!");
                }
            }
        }

        SaveSystem.SavePlayer(playerData);
    }

    void CheckPlayerActivity()
    {
        if ((Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed) 
            || (Mouse.current != null && Mouse.current.leftButton.isPressed))
        {
            _inactivityTimer = 0f; 
        }
        else
        {
            _inactivityTimer += Time.deltaTime;
        }
        
        if (_inactivityTimer >= _inactivityThreshold)
        {
            GooglePlayManager.UnlockAchievement((GPGSlds.achievement_whos_the_craziest));
        }
    }
}