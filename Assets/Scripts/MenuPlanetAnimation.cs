using System;
using UnityEngine;
using NaughtyAttributes;

public class MenuPlanetAnimation : MonoBehaviour
{ 
    [SerializeField] private float rotationSpeed;

    [Foldout("Planets")] [SerializeField] private Transform planet1;
    [Foldout("Planets")] [SerializeField] private Transform planet2;
    [Foldout("Planets")] [SerializeField] private Transform planet3;
    
    [SerializeField] private GameObject _pauseUi;

    private void Start()
    {
        _pauseUi.SetActive(false);
    }
    

    private void FixedUpdate()
    {
            Debug.Log(planet1.localRotation.eulerAngles);
            planet1.Rotate(0,0, rotationSpeed * 1.2f * Time.deltaTime) ;
            planet2.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
            planet3.Rotate(0, 0, rotationSpeed * 0.8f * Time.deltaTime);
    }
}