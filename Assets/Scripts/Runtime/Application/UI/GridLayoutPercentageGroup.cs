using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;

[ExecuteAlways]
public class GridLayoutWeightedGroup : LayoutGroup
{
    public enum ConstraintMode
    {
        FixedColumns,
        FixedRows
    }

    public enum AlignmentMode
    {
        Start,  // Left or Top
        Center,
        End     // Right or Bottom
    }

    [Header("Grid Constraint")]
    [SerializeField] private ConstraintMode _constraintMode = ConstraintMode.FixedColumns;
    [SerializeField, Min(1)] private int _fixedCount = 2;

    [Header("Spacing")]
    [SerializeField, Range(0f, 0.2f)] private float _spacingPercent = 0.02f;

    [Header("Padding")]
    [SerializeField, Range(0f, 0.5f)] private float _leftOffsetPercent = 0.05f;
    [SerializeField, Range(0f, 0.5f)] private float _rightOffsetPercent = 0.05f;
    [SerializeField, Range(0f, 0.5f)] private float _topOffsetPercent = 0.05f;
    [SerializeField, Range(0f, 0.5f)] private float _bottomOffsetPercent = 0.05f;

    [Header("Alignment for Incomplete Rows/Columns")]
    [SerializeField] private AlignmentMode _incompleteAlignment = AlignmentMode.Center;

    private bool _isFixedColumns => _constraintMode == ConstraintMode.FixedColumns;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        ArrangeElements();
    }

    public override void CalculateLayoutInputVertical()
    {
        ArrangeElements();
    }

    public override void SetLayoutHorizontal()
    {
        ArrangeElements();
    }

    public override void SetLayoutVertical()
    {
        ArrangeElements();
    }

    private void ArrangeElements()
    {
        int childCount = rectChildren.Count;
        if (childCount == 0) return;

        ConfigureRectTransform();

        int columns = CalculateColumns(childCount);
        int rows = CalculateRows(childCount, columns);

        float spacing = rectTransform.rect.width * _spacingPercent;

        float[] columnWidths = CalculateColumnWidths(columns, spacing);
        float[] rowHeights = CalculateRowHeights(rows, spacing);

        PositionChildren(columns, rows, columnWidths, rowHeights, spacing);
    }

    private void ConfigureRectTransform()
    {
        rectTransform.pivot = new Vector2(0.5f, 1f);
        rectTransform.anchorMin = new Vector2(0f, 0f);
        rectTransform.anchorMax = new Vector2(1f, 1f);
    }

    private int CalculateColumns(int childCount)
    {
        return _isFixedColumns ? _fixedCount : Mathf.CeilToInt((float)childCount / _fixedCount);
    }

    private int CalculateRows(int childCount, int columns)
    {
        return _isFixedColumns ? Mathf.CeilToInt((float)childCount / columns) : _fixedCount;
    }

    private float[] CalculateColumnWidths(int columns, float spacing)
    {
        float parentWidth = rectTransform.rect.width;
        float totalHorizontalPadding = parentWidth * (_leftOffsetPercent + _rightOffsetPercent);
        float availableWidth = parentWidth - totalHorizontalPadding - (columns - 1) * spacing;

        float columnWidth = availableWidth / columns;
        return Enumerable.Repeat(columnWidth, columns).ToArray();
    }

    private float[] CalculateRowHeights(int rows, float spacing)
    {
        float parentHeight = rectTransform.rect.height;
        float totalVerticalPadding = parentHeight * (_topOffsetPercent + _bottomOffsetPercent);
        float availableHeight = parentHeight - totalVerticalPadding - (rows - 1) * spacing;

        float rowHeight = availableHeight / rows;
        return Enumerable.Repeat(rowHeight, rows).ToArray();
    }

    private void PositionChildren(int columns, int rows, float[] columnWidths, float[] rowHeights, float spacing)
    {
        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float xStart = padding.left + parentWidth * _leftOffsetPercent;
        float yStart = -padding.top - parentHeight * _topOffsetPercent;

        if (_isFixedColumns)
        {
            for (int row = 0; row < rows; row++)
            {
                int itemsInThisRow = (row == rows - 1) ? Mathf.Min(rectChildren.Count - row * columns, columns) : columns;

                float rowWidth = columnWidths.Take(itemsInThisRow).Sum() + spacing * (itemsInThisRow - 1);
                float alignmentOffset = GetAlignmentOffset(columns, itemsInThisRow, columnWidths[0] + spacing, _incompleteAlignment);

                for (int col = 0; col < itemsInThisRow; col++)
                {
                    int index = row * columns + col;
                    if (index >= rectChildren.Count) break;

                    RectTransform child = rectChildren[index];

                    float xPos = xStart + alignmentOffset + columnWidths.Take(col).Sum() + col * spacing;
                    float yPos = yStart - row * (rowHeights[row] + spacing);

                    SetChildAlongAxis(child, 0, xPos, columnWidths[col]);
                    SetChildAlongAxis(child, 1, -yPos, rowHeights[row]);
                }
            }
        }
        else
        {
            for (int col = 0; col < columns; col++)
            {
                int itemsInThisCol = (col == columns - 1) ? Mathf.Min(rectChildren.Count - col * rows, rows) : rows;

                float colHeight = rowHeights.Take(itemsInThisCol).Sum() + spacing * (itemsInThisCol - 1);
                float alignmentOffset = GetAlignmentOffset(rows, itemsInThisCol, rowHeights[0] + spacing, _incompleteAlignment);

                for (int row = 0; row < itemsInThisCol; row++)
                {
                    int index = col * rows + row;
                    if (index >= rectChildren.Count) break;

                    RectTransform child = rectChildren[index];

                    float xPos = xStart + col * (columnWidths[col] + spacing);
                    float yPos = yStart - alignmentOffset - row * (rowHeights[row] + spacing);

                    SetChildAlongAxis(child, 0, xPos, columnWidths[col]);
                    SetChildAlongAxis(child, 1, -yPos, rowHeights[row]);
                }
            }
        }
    }

    private float GetAlignmentOffset(int fullCount, int actualCount, float cellWithSpacing, AlignmentMode mode)
    {
        int missing = fullCount - actualCount;

        switch (mode)
        {
            case AlignmentMode.Center:
                return missing * cellWithSpacing / 2f;
            case AlignmentMode.End:
                return missing * cellWithSpacing;
            case AlignmentMode.Start:
            default:
                return 0f;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (rectTransform == null) return;

        Vector3[] outerCorners = new Vector3[4];
        rectTransform.GetWorldCorners(outerCorners);

        Handles.color = Color.green;
        for (int i = 0; i < 4; i++)
        {
            Handles.DrawLine(outerCorners[i], outerCorners[(i + 1) % 4]);
        }

        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        float padLeft = width * _leftOffsetPercent;
        float padRight = width * _rightOffsetPercent;
        float padTop = height * _topOffsetPercent;
        float padBottom = height * _bottomOffsetPercent;

        Vector3 worldPos = rectTransform.position;
        Vector3 localCenter = rectTransform.rect.center;
        Quaternion rotation = rectTransform.rotation;

        Vector3 innerMin = rectTransform.TransformPoint(new Vector3(
            rectTransform.rect.xMin + padLeft,
            rectTransform.rect.yMin + padBottom,
            0f
        ));
        Vector3 innerMax = rectTransform.TransformPoint(new Vector3(
            rectTransform.rect.xMax - padRight,
            rectTransform.rect.yMax - padTop,
            0f
        ));

        Handles.color = Color.yellow;
        Vector3 bl = new Vector3(innerMin.x, innerMin.y, 0);
        Vector3 br = new Vector3(innerMax.x, innerMin.y, 0);
        Vector3 tr = new Vector3(innerMax.x, innerMax.y, 0);
        Vector3 tl = new Vector3(innerMin.x, innerMax.y, 0);

        Handles.DrawLine(bl, br);
        Handles.DrawLine(br, tr);
        Handles.DrawLine(tr, tl);
        Handles.DrawLine(tl, bl);

        Handles.color = new Color(0.2f, 0.8f, 1f);
        foreach (RectTransform child in rectChildren)
        {
            if (child == null) continue;

            Vector3[] childCorners = new Vector3[4];
            child.GetWorldCorners(childCorners);

            for (int i = 0; i < 4; i++)
                Handles.DrawLine(childCorners[i], childCorners[(i + 1) % 4]);

            float childWidth = child.rect.width;
            float childHeight = child.rect.height;


            Vector3 labelPos = (childCorners[1] + childCorners[2]) / 2f - Vector3.up * 10f;

            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.normal.textColor = Color.cyan;
            style.alignment = TextAnchor.MiddleCenter;
            Handles.Label(labelPos, $"{childWidth:0} x {childHeight:0}", style);
        }
    }
#endif

}