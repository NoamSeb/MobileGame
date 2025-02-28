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

        CheckForCorrectSceneSetup();

        PlayGrid = GameObject.FindGameObjectWithTag("Playzone").GetComponent<Grid>();
        OxygenSlider = GameObject.FindGameObjectWithTag("Oxygen").GetComponent<Slider>();
        PlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerGridMovement>();
        MirrorScript = FindFirstObjectByType<PlayerMirrorMovement>();
    }

    void CheckForCorrectSceneSetup()
    {
        if (!GameObject.FindGameObjectWithTag("Playzone"))
        {
            throw new MissingComponentException("Missing Grid in scene");
        }

        if (!GameObject.FindGameObjectWithTag("Oxygen"))
        {
            throw new MissingComponentException("Missing Oxygen bar in scene");
        }

        if (!GameObject.FindGameObjectWithTag("Player"))
        {
            throw new MissingComponentException("Missing Player in scene");
        }
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
    public void SetCurrentBiome(BiomeManager biome)
    {
        PlayerPrefs.SetString("CurrentBiome", biome.name); // sauvegarde le nom du biome
        PlayerPrefs.Save();
    }
}