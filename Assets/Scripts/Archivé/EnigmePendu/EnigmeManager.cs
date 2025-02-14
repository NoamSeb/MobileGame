using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnigmeManager : MonoBehaviour
{
    private string motchoisi;
    [SerializeField] public List<string> motsDisponibles;

    public TextMeshProUGUI listeLettresText;
    private Dictionary<char, int> lettresAssociees = new Dictionary<char, int>(); // associe une lettre à un numéro
    private List<int> numerosCorrects = new List<int>(); // stocke les numéros des bonnes lettres

    void Start()
    {
        ChoisirMot();
        GénérerLettresEtNum();
        AfficherMot();
    }

    void ChoisirMot()
    {
        if (motsDisponibles.Count == 0)
        {
            Debug.Log("aucun mots disponibles");
            return;
        }
        motchoisi = motsDisponibles[Random.Range(0, motsDisponibles.Count)];
        motchoisi = motchoisi.ToUpper();    
        Debug.Log("mot choisi :" + motchoisi);
    }

    void GénérerLettresEtNum()
    {
        listeLettresText.text = "";
        lettresAssociees.Clear();
        numerosCorrects.Clear();

        List<int> numerosDispo = new List<int> { 1, 2, 3, 4, 5 }; 

        foreach (char lettre in motchoisi)
        {
            if (!lettresAssociees.ContainsKey(lettre)) 
            {
                int numeroAttribue = numerosDispo[Random.Range(0, numerosDispo.Count)];
                numerosDispo.Remove(numeroAttribue); 

                lettresAssociees.Add(lettre, numeroAttribue);
                numerosCorrects.Add(numeroAttribue);
            }
        }

        foreach (var entry in lettresAssociees)
        {
            listeLettresText.text += entry.Value + " = " + entry.Key + "\n";
        }

        Debug.Log("lettres et numéros générés : " + string.Join(", ", lettresAssociees));
    }

    void AfficherMot()
    {
        //
    }

    // Update is called once per frame
    void Update()
    {

    }
}
