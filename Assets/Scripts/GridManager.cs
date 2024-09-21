using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public float cooldown;
    public Sprite sprite;
    public int[,] Grid;
    float Vertical, Horizontal;
    int Columns, Rows;
    float spriteSizeX, spriteSizeY;
    private bool canClickGlobal = true; // Control global para los clics

    void Start()
    {
        // Calcula el tamaño de la cámara
        Vertical = Camera.main.orthographicSize;
        Horizontal = Vertical * Screen.width / Screen.height;

        // Obtén el tamaño del sprite
        spriteSizeX = sprite.bounds.size.x;
        spriteSizeY = sprite.bounds.size.y;

        // Calcula el número de columnas y filas usando CeilToInt
        Columns = Mathf.CeilToInt(Horizontal * 2 / spriteSizeX) - 1;
        Rows = Mathf.CeilToInt(Vertical * 2 / spriteSizeY) - 1;

        // Inicializa la grilla
        Grid = new int[Columns, Rows];

        // Calcular el desplazamiento inicial
        float offsetX = (Columns * spriteSizeX) / 2 - spriteSizeX / 2;
        float offsetY = (Rows * spriteSizeY) / 2 - spriteSizeY / 2;

        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                Grid[i, j] = Random.Range(0, 10);
                SpawnTile(i, j, Grid[i, j], offsetX, offsetY);
            }
        }
    }

    private void SpawnTile(int x, int y, int value, float offsetX, float offsetY)
    {
        GameObject g = new GameObject("Tile_" + x + "_" + y);
        g.transform.position = new Vector3(x * spriteSizeX - offsetX, y * spriteSizeY - offsetY, 0);

        var s = g.AddComponent<SpriteRenderer>();
        s.sprite = sprite;

        // Añade el TileClickHandler y pasa la referencia a GridManager
        TileClickHandler clickHandler = g.AddComponent<TileClickHandler>();
        clickHandler.SetGridManager(this);
        clickHandler.SetTileCoordinates(x, y, value);
        
        // Añade un BoxCollider2D para detectar clics
        g.AddComponent<BoxCollider2D>();
    }

    public void NotifyTileClicked(TileClickHandler tileClickHandler)
    {
        if (canClickGlobal)
        {
            print(DetermineMajorAxisDirection(tileClickHandler, 6, 3));
            //print(CalculateTileDistance(tileClickHandler, 3, 6));
            tileClickHandler.ChangeColor(); // Cambia el color del tile clickeado
            StartCoroutine(WaitBeforeNextClick());
        }
    }


    private IEnumerator WaitBeforeNextClick()
    {
        canClickGlobal = false; // Desactiva los clics
        yield return new WaitForSeconds(cooldown); // Espera 2 segundos
        canClickGlobal = true; // Reactiva los clics
    }

    public int CalculateTileDistance(TileClickHandler clickedTile, int targetX, int targetY)
    {
        // Obtiene las coordenadas del tile clickeado
        int clickedX = clickedTile.GetTileX();
        int clickedY = clickedTile.GetTileY();

        // Calcula la distancia de Manhattan
        int distance = Mathf.Abs(targetX - clickedX) + Mathf.Abs(targetY - clickedY);

        //print($"Distancia entre tile ({clickedX}, {clickedY}) y tile ({targetX}, {targetY}): {distance} tiles");
        return distance;
    }

    public string DetermineMajorAxisDirection(TileClickHandler clickedTile, int targetX, int targetY)
    {
        int clickedX = clickedTile.GetTileX();
        int clickedY = clickedTile.GetTileY();

        // Calcula las diferencias en los ejes
        int diffX = targetX - clickedX;
        int diffY = targetY - clickedY;

        // Si ya está en la posición objetivo
        if (diffX == 0 && diffY == 0)
        {
            return "Llegaste";
        }

        // Generar direcciones basadas en las diferencias
        string horizontalDirection = diffX > 0 ? "a la derecha" : (diffX < 0 ? "a la izquierda" : "");
        string verticalDirection = diffY > 0 ? "arriba" : (diffY < 0 ? "abajo" : "");

        // Si hay diferencias en ambos ejes, combinar las direcciones
        if (horizontalDirection != "" && verticalDirection != "")
        {
            return $"más {verticalDirection} y {horizontalDirection}";
        }
        // Si solo hay diferencia en uno de los ejes
        else if (horizontalDirection != "")
        {
            return $"más {horizontalDirection}";
        }
        else
        {
            return $"más {verticalDirection}";
        }
    }

}
