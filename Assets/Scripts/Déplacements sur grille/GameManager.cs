using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //L'objet GameManager est un singleton pr�sent dans chaque sc�ne
    public static GameManager Instance;
    public static int CurrentLevelID { get; set; }

    public Grid PlayGrid { get; private set; }
    public Slider OxygenSlider { get; private set; }
    public PlayerGridMovement PlayerScript { get; private set; }
    public PlayerMirrorMovement MirrorScript { get; private set; }

    private void Awake()
    {
        if (Instance == null || Instance != this)
        {
            Instance = this;
        }

        Level.OnLevelLoad += FindNeededObjects;
    }

    void FindNeededObjects(Level level)
    {
        PlayGrid = level.PlayGrid;
        OxygenSlider = level.Slider;
        PlayerScript = level.Movement;
        MirrorScript = level.MirrorMovement;
    }

    public static void OnSave(int currentScore)
    {
        PlayerData playerData = SaveSystem.LoadPlayer();

        if (playerData != null)
        {
            var dataElement = playerData.data.FirstOrDefault(x => x.idLevel == CurrentLevelID);

            if (!dataElement.Equals(default))
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
                playerData.data.Add(new PlayerData.DataElement(CurrentLevelID, currentScore));
            }
        }

        SaveSystem.SavePlayer(playerData);
    }
    
}