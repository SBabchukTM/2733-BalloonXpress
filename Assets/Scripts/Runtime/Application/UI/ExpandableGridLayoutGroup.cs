using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class FlexibleGridLayout : LayoutGroup
{
    public enum FitType { Uniform, FixedColumns, FixedRows }
    public FitType fitType = FitType.Uniform;
    public int constraintCount = 2;

    public enum HorizontalDirection { LeftToRight, RightToLeft }
    public HorizontalDirection horizontalDirection = HorizontalDirection.LeftToRight;

    public enum VerticalDirection { TopToBottom, BottomToTop }
    public VerticalDirection verticalDirection = VerticalDirection.TopToBottom;

    public enum AlignmentMode { Start, Center, End }
    [SerializeField] private AlignmentMode incompleteAlignment = AlignmentMode.Center;

    [Range(0f, 1f)] public float spacingPercentX = 0.05f;
    [Range(0f, 1f)] public float spacingPercentY = 0.05f;
    [Range(0f, 0.5f)] public float leftPaddingPercent = 0.05f;
    [Range(0f, 0.5f)] public float rightPaddingPercent = 0.05f;
    [Range(0f, 0.5f)] public float topPaddingPercent = 0.05f;
    [Range(0f, 0.5f)] public float bottomPaddingPercent = 0.05f;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        Arrange();
    }

    public override void CalculateLayoutInputVertical()
    {
        Arrange();
    }

    public override void SetLayoutHorizontal() { }
    public override void SetLayoutVertical() { }

    void Arrange()
    {
        int childCount = rectChildren.Count;
        if (childCount == 0) return;

        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        float leftPadding = width * leftPaddingPercent;
        float rightPadding = width * rightPaddingPercent;
        float topPadding = height * topPaddingPercent;
        float bottomPadding = height * bottomPaddingPercent;

        float spacingX = width * spacingPercentX;
        float spacingY = height * spacingPercentY;

        int cols = 1, rows = 1;
        if (fitType == FitType.Uniform)
        {
            float sqrRt = Mathf.Sqrt(childCount);
            cols = Mathf.CeilToInt(sqrRt);
            rows = Mathf.CeilToInt(sqrRt);
        }
        else if (fitType == FitType.FixedColumns)
        {
            cols = constraintCount;
            rows = Mathf.CeilToInt((float)childCount / cols);
        }
        else if (fitType == FitType.FixedRows)
        {
            rows = constraintCount;
            cols = Mathf.CeilToInt((float)childCount / rows);
        }

        float totalSpacingX = spacingX * (cols - 1);
        float totalSpacingY = spacingY * (rows - 1);
        float contentWidth = width - leftPadding - rightPadding - totalSpacingX;
        float contentHeight = height - topPadding - bottomPadding - totalSpacingY;

        float itemWidth = contentWidth / cols;
        float itemHeight = contentHeight / rows;


        RectTransform parentRT = transform.parent as RectTransform;
        bool verticalScroll = parentRT != null && parentRT.GetComponent<ScrollRect>()?.vertical == true;
        bool horizontalScroll = parentRT != null && parentRT.GetComponent<ScrollRect>()?.horizontal == true;

        float requiredWidth = leftPadding + rightPadding + itemWidth * cols + spacingX * (cols - 1);
        float requiredHeight = topPadding + bottomPadding + itemHeight * rows + spacingY * (rows - 1);

        if (verticalScroll && !horizontalScroll)
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, requiredHeight);
        }
        else if (horizontalScroll && !verticalScroll)
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, requiredWidth);
        }

        for (int i = 0; i < childCount; i++)
        {
            int row, col;
            if (fitType == FitType.FixedColumns)
            {
                row = i / cols;
                col = i % cols;
            }
            else 
            {
                col = i / rows;
                row = i % rows;
            }

            int itemsInLine = fitType == FitType.FixedColumns
                ? (row == rows - 1 ? Mathf.Min(childCount - row * cols, cols) : cols)
                : (col == cols - 1 ? Mathf.Min(childCount - col * rows, rows) : rows);

            float lineOffset = GetAlignmentOffset(
                fitType == FitType.FixedColumns ? cols : rows,
                itemsInLine,
                (fitType == FitType.FixedColumns ? itemWidth + spacingX : itemHeight + spacingY),
                incompleteAlignment);

            if (horizontalDirection == HorizontalDirection.RightToLeft)
                col = cols - 1 - col;

            if (verticalDirection == VerticalDirection.BottomToTop)
                row = rows - 1 - row;

            float xPos = leftPadding + col * (itemWidth + spacingX);
            float yPos = -topPadding - row * (itemHeight + spacingY);

            if (fitType == FitType.FixedColumns)
                xPos += lineOffset;
            else
                yPos -= lineOffset;

            SetChildAlongAxis(rectChildren[i], 0, xPos, itemWidth);
            SetChildAlongAxis(rectChildren[i], 1, yPos, itemHeight);
        }
    }

    float GetAlignmentOffset(int fullCount, int actualCount, float cellWithSpacing, AlignmentMode mode)
    {
        int missing = fullCount - actualCount;
        switch (mode)
        {
            case AlignmentMode.Center: return missing * cellWithSpacing / 2f;
            case AlignmentMode.End: return missing * cellWithSpacing;
            default: return 0f;
        }
    }
}
