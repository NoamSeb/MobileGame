using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    //L'objet GameManager est un singleton présent dans chaque scène
    public static GameManager Instance;
    [SerializeField] public Grid PlayGrid { get; private set; }

    private void Awake()
    {
        if (Instance == null || Instance != this)
        {
            Instance = this;
        }

        CheckForCorrectSceneSetup();

        PlayGrid = GameObject.FindGameObjectWithTag("Playzone").GetComponent<Grid>();
    }

    void CheckForCorrectSceneSetup()
    {
        if (!GameObject.FindGameObjectWithTag("Playzone"))
        {
            throw new MissingComponentException("Missing Grid in scene");
        }
    }
}
