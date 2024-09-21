using UnityEngine;

public class TileClickHandler : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private GridManager gridManager;
    private int tileX;
    private int tileY;
    private int value;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetGridManager(GridManager manager)
    {
        gridManager = manager;
    }

    public void SetTileCoordinates(int x, int y, int value)
    {
        tileX = x;
        tileY = y;

    }

    public int GetTileX() {
        return tileX;
    }

    public int GetTileY() {
        return tileY;
    }

    public int GetTileValue() {
        return value;
    }

    private void OnMouseDown()
    {
        if (gridManager != null)
        {
            gridManager.NotifyTileClicked(this); // Pasa la referencia del tile clickeado
        }
    }

    public void ChangeColor()
    {
        // Cambiar el color a rojo
        spriteRenderer.color = Color.red;
    }
}
