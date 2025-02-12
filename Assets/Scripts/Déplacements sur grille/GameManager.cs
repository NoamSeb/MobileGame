using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //L'objet GameManager est un singleton pr�sent dans chaque sc�ne
    public static GameManager Instance;
    public Grid PlayGrid { get; private set; }
    public Slider OxygenSlider { get; private set; }
    public PlayerGridMovement PlayerScript { get; private set; }

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
}
