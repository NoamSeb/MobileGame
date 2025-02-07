using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TilemapCollider2D PlayZoneCollider { get; private set; }

    private void Awake()
    {
        if (Instance == null || Instance != this)
        {
            Instance = this;
        }

        PlayZoneCollider = GameObject.FindGameObjectWithTag("Playzone").GetComponent<TilemapCollider2D>();

        SwipeMovement.OnSwipeEnd += DebugSwipe;
    }

    void DebugSwipe(SwipeMovement.SwipeDirection direction)
    {
        Debug.Log("aezeazeaeazaez");
    }
}
