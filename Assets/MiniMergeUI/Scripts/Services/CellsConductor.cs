using MiniMergeUI.View;
using UnityEngine;

public class CellsConductor 
{
    private Cell[] _cells;
    private readonly GameCanvas _gameCanvas;

    public CellsConductor(GameCanvas gameCanvas)
    {
        _cells = gameCanvas.Cells;
       
        _gameCanvas = gameCanvas;
    }

    public Cell GetNearestCell(Vector2 chipAnchoredPosition)
    {
        Cell best = null;
        float bestSqrDist = float.PositiveInfinity;

        foreach (var cell in _cells)
        {
            Vector3 cellWorldCenter = cell.RectTransform.TransformPoint(cell.RectTransform.rect.center);
            Vector2 cellLocalCenter = (Vector2)_gameCanvas.RectTransform.InverseTransformPoint(cellWorldCenter);

            float sqrDist = (chipAnchoredPosition - cellLocalCenter).sqrMagnitude;

            if (sqrDist < bestSqrDist)
            {
                bestSqrDist = sqrDist;
                best = cell;
            }
        }

        return best;
    }


}
