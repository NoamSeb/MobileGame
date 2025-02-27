using System;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public Grid PlayGrid {  get; private set; }
    public Slider Slider { get; private set; }
    public PlayerGridMovement Movement { get; private set; }
    public PlayerMirrorMovement MirrorMovement {  get; private set; }

    private void Awake()
    {
        PlayGrid = GetComponentInChildren<Grid>(true);
        Slider = GetComponentInChildren<Slider>(true);
        Movement = GetComponentInChildren<PlayerGridMovement>(true);
        MirrorMovement = GetComponentInChildren<PlayerMirrorMovement>(true);
    }

    public static event Action<Level> OnLevelLoad; 
    private void OnEnable()
    {
        OnLevelLoad?.Invoke(this);
    }
}
