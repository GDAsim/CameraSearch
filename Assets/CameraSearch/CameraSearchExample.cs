using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraSearchExample : MonoBehaviour
{
    [SerializeField] Camera cam;

    Vector3 SearchPos;
    float SearchSize;

    Vector3 SearchRectPos;
    Vector3 SearchRectSize;

    List<Vector3[]> searchFustrumLines = new List<Vector3[]>();

    void Update()
    {
        (SearchPos, SearchSize) = cam.CalculateSearchSize(cam.farClipPlane);

        (SearchRectPos, SearchRectSize) = cam.CalculateSearchRect(cam.farClipPlane);  

        searchFustrumLines = cam.CalculateSearchFustrumField(cam.farClipPlane);
    }

    public bool ShowRadius = true;
    public bool ShowSquare = true;
    public bool ShowRect = true;
    public bool ShowPoly = true;
    void OnDrawGizmos()
    {
        if(ShowRadius)
        {
            Gizmos.color = Color.red;
            var radius = (SearchRectSize/2).magnitude;
            Gizmos.DrawWireSphere(SearchPos, radius);
        }
        if(ShowSquare)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(SearchPos, new Vector3(SearchSize, 0, SearchSize));
            Gizmos.DrawSphere(SearchPos, 1f);
        }
        if (ShowRect)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(SearchRectPos, SearchRectSize);
            Gizmos.DrawSphere(SearchRectPos, 1f);
        }
        if (ShowPoly)
        {
            Gizmos.color = Color.black;
            foreach (var line in searchFustrumLines)
            {
                Gizmos.DrawLine(line[0], line[1]);
            }
        }
    }
}
