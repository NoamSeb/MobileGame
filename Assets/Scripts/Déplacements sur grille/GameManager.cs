using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    //L'objet GameManager est un singleton présent dans chaque scène
    public static GameManager Instance;

    /// <summary>
    /// Pour remplacer la classe Bounds native qui ne fonctionnait pas
    /// </summary>
    public struct NotBounds
    {
        public Vector3 min { get; private set; }
        public Vector3 max { get; private set; }

        public NotBounds(Vector3 min, Vector3 max)
        {
            this.min = min;
            this.max = max;
        }

        public bool Contains(Vector3 point)
        {
            if (point.x >= min.x && point.y >= min.y 
                && point.x <= max.x && point.y <= max.y) { return true; }

            return false;
        }
    }
    public NotBounds PlayZone { get; private set; }

    private void Awake()
    {
        if (Instance == null || Instance != this)
        {
            Instance = this;
        }

        //Récupération de l'extension maximale de la tilemap, et donc de la zone de jeu
        Tilemap playZoneMap = GameObject.FindGameObjectWithTag("Playzone").GetComponent<Tilemap>();
        Vector3 min = playZoneMap.cellBounds.min;
        Vector3 max = playZoneMap.cellBounds.max;

        PlayZone = new NotBounds(min, max);
    }
}
