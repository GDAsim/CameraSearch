using UnityEngine;

/// <summary>
/// RectEncapsulate
/// </summary>
public static partial class RectExtensions
{
    public static Rect Encapsulate(this Rect rect, Vector2[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            var point = points[i];
            var xmin = Mathf.Min(rect.min.x, rect.max.x, point.x);
            var xmax = Mathf.Max(rect.min.x, rect.max.x, point.x);
            var ymin = Mathf.Min(rect.min.y, rect.max.y, point.y);
            var ymax = Mathf.Max(rect.min.y, rect.max.y, point.y);
            rect = Rect.MinMaxRect(xmin, ymin, xmax, ymax);
        }
        
        return rect;
    }

    public static Rect Encapsulate(this Rect rect, Rect other)
    {
        var xmin = Mathf.Min(rect.min.x, rect.max.x, other.min.x, other.max.x);
        var xmax = Mathf.Max(rect.min.x, rect.max.x, other.min.x, other.max.x);
        var ymin = Mathf.Min(rect.min.y, rect.max.y, other.min.y, other.max.y);
        var ymax = Mathf.Max(rect.min.y, rect.max.y, other.min.y, other.max.y);
        return Rect.MinMaxRect(xmin, ymin, xmax, ymax);
    }
}