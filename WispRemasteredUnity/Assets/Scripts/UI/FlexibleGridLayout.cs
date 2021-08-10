using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
    [SerializeField] private int rows, columns;
    private Vector2 cellSize;
    public Vector2 spacing;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        float sqrt = Mathf.Sqrt(transform.childCount);
        rows = Mathf.CeilToInt(sqrt);
        columns = Mathf.CeilToInt(sqrt);

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = (parentWidth / (float) columns) - ((spacing.x / (float) columns) * (columns - 1));
        float cellHeight = (parentHeight / (float) rows) - ((spacing.y / (float) rows) * (rows - 1));

        cellSize.x = cellWidth;
        cellSize.y = cellHeight;

        int columnCount = 0;
        int rowCount = 0;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / columns;
            columnCount = i % columns;

            var item = rectChildren[i];
            
            var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount);
            var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount);

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }
    }

    public override void SetLayoutHorizontal()
    {
        
    }

    public override void SetLayoutVertical()
    {
        
    }

    public override void CalculateLayoutInputVertical()
    {
       
    }
}
