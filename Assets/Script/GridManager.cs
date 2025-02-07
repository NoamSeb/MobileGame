using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 5;  // largeur de la grille (modifiable dans l'inspector)
    public int height = 5; // hauteur de la grille (modifiable dans l'inspector)
    public GameObject tilePrefab; 

    private void Start()
    {
        GenerateGrid();
    }                           

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 tilePosition = new Vector3(x, 0, y);
                GameObject newTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                newTile.transform.parent = transform; 
            }
        }
    }                                       
}
