using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Calculate the Area where the camera can see
/// Currently only works on YPlane Interceptions
/// </summary>
public static partial class CameraExtensions
{
    public static(Vector3 pos, float size) CalculateSearchSize(this Camera cam, float searchDistance = 10000)
    {
        Plane interceptPlane = new Plane(Vector3.up, Vector3.zero);
        Vector3 pos = Vector3.zero;
        float size = 0;

        //Four corners of screen (0,0) is bottom left (1,1) is top right
        Vector3 bl = new Vector3(0, 0);
        Vector3 tl = new Vector3(0, Screen.height);
        Vector3 tr = new Vector3(Screen.width, Screen.height);
        Vector3 br = new Vector3(Screen.width, 0);

        //Create Ray, this will match camera fustrum
        Ray bottomLeftRay = cam.ScreenPointToRay(bl);
        Ray topLeftRay = cam.ScreenPointToRay(tl);
        Ray topRightRay = cam.ScreenPointToRay(tr);
        Ray bottomRightRay = cam.ScreenPointToRay(br);

        interceptPlane.Raycast(bottomLeftRay, out float blhit);
        interceptPlane.Raycast(topLeftRay, out float tlhit);
        interceptPlane.Raycast(topRightRay, out float trhit);
        interceptPlane.Raycast(bottomRightRay, out float brhit);

        Vector3 blPoint = bottomLeftRay.GetPoint(blhit);
        Vector3 tlPoint = topLeftRay.GetPoint(tlhit);
        Vector3 trPoint = topRightRay.GetPoint(trhit);
        Vector3 brPoint = bottomRightRay.GetPoint(brhit);

        Plane camPlane = new Plane(-cam.transform.forward, cam.transform.position + cam.transform.forward * searchDistance);
        camPlane.Raycast(bottomLeftRay, out float blsearchDistance);
        camPlane.Raycast(topLeftRay, out float tlsearchDistance);
        camPlane.Raycast(topRightRay, out float trsearchDistance);
        camPlane.Raycast(bottomRightRay, out float brsearchDistance);

        bool istop = tlhit > 0 && tlhit <= tlsearchDistance &&
                     trhit > 0 && trhit <= trsearchDistance;
        bool isbot = blhit > 0 && blhit <= blsearchDistance &&
                     brhit > 0 && brhit <= brsearchDistance;

        if (istop || isbot)
        {
            if (istop != isbot)
            {
                if (isbot)
                {
                    var tlstart = bottomLeftRay.GetPoint(blsearchDistance);
                    var tldir = topLeftRay.GetPoint(tlsearchDistance) - tlstart;
                    topLeftRay = new Ray(tlstart, tldir);
                    interceptPlane.Raycast(topLeftRay, out tlhit);
                    tlPoint = topLeftRay.GetPoint(tlhit);

                    var trstart = bottomRightRay.GetPoint(brsearchDistance);
                    var trdir = topRightRay.GetPoint(trsearchDistance) - trstart;
                    topRightRay = new Ray(trstart, trdir);
                    interceptPlane.Raycast(topLeftRay, out trhit);
                    trPoint = topRightRay.GetPoint(trhit);
                }
                else
                {
                    var blstart = topLeftRay.GetPoint(tlsearchDistance);
                    var bldir = bottomLeftRay.GetPoint(blsearchDistance) - blstart;
                    bottomLeftRay = new Ray(blstart, bldir);
                    interceptPlane.Raycast(bottomLeftRay, out blhit);
                    blPoint = bottomLeftRay.GetPoint(blhit);

                    var brstart = topRightRay.GetPoint(trsearchDistance);
                    var brdir = bottomRightRay.GetPoint(brsearchDistance) - brstart;
                    bottomRightRay = new Ray(brstart, brdir);
                    interceptPlane.Raycast(bottomRightRay, out brhit);
                    brPoint = bottomRightRay.GetPoint(brhit);
                }
            }

            Vector2[] points = new Vector2[4];
            points[0] = new Vector2(tlPoint.x, tlPoint.z);
            points[1] = new Vector2(trPoint.x, trPoint.z);
            points[2] = new Vector2(blPoint.x, blPoint.z);
            points[3] = new Vector2(brPoint.x, brPoint.z);
            Rect r = new Rect(tlPoint, Vector2.zero).Encapsulate(points);

            pos = new Vector3(r.center.x, 0, r.center.y);
            size = Mathf.Max(r.size.x,r.size.y);
        }

        return (pos, size);
    }

    public static(Vector3 pos, Vector3 size) CalculateSearchRect(this Camera cam, float searchDistance = 10000)
    {
        Plane interceptPlane = new Plane(Vector3.up, Vector3.zero);
        Vector3 pos = Vector3.zero;
        Vector3 size = Vector3.zero;

        //Four corners of screen (0,0) is bottom left (1,1) is top right
        Vector3 bl = new Vector3(0, 0);
        Vector3 tl = new Vector3(0, Screen.height);
        Vector3 tr = new Vector3(Screen.width, Screen.height);
        Vector3 br = new Vector3(Screen.width, 0);

        //Create Ray, this will match camera fustrum
        Ray bottomLeftRay = cam.ScreenPointToRay(bl);
        Ray topLeftRay = cam.ScreenPointToRay(tl);
        Ray topRightRay = cam.ScreenPointToRay(tr);
        Ray bottomRightRay = cam.ScreenPointToRay(br);

        interceptPlane.Raycast(bottomLeftRay, out float blhit);
        interceptPlane.Raycast(topLeftRay, out float tlhit);
        interceptPlane.Raycast(topRightRay, out float trhit);
        interceptPlane.Raycast(bottomRightRay, out float brhit);

        Vector3 blPoint = bottomLeftRay.GetPoint(blhit);
        Vector3 tlPoint = topLeftRay.GetPoint(tlhit);
        Vector3 trPoint = topRightRay.GetPoint(trhit);
        Vector3 brPoint = bottomRightRay.GetPoint(brhit);

        Plane camPlane = new Plane(-cam.transform.forward, cam.transform.position + cam.transform.forward * searchDistance);
        camPlane.Raycast(bottomLeftRay, out float blsearchDistance);
        camPlane.Raycast(topLeftRay, out float tlsearchDistance);
        camPlane.Raycast(topRightRay, out float trsearchDistance);
        camPlane.Raycast(bottomRightRay, out float brsearchDistance);

        bool istop = tlhit > 0 && tlhit <= tlsearchDistance &&
                     trhit > 0 && trhit <= trsearchDistance;
        bool isbot = blhit > 0 && blhit <= blsearchDistance &&
                     brhit > 0 && brhit <= brsearchDistance;

        if (istop || isbot)
        {
            if (istop != isbot)
            {
                if (isbot)
                {
                    var tlstart = bottomLeftRay.GetPoint(blsearchDistance);
                    var tldir = topLeftRay.GetPoint(tlsearchDistance) - tlstart;
                    topLeftRay = new Ray(tlstart, tldir);
                    interceptPlane.Raycast(topLeftRay, out tlhit);
                    tlPoint = topLeftRay.GetPoint(tlhit);

                    var trstart = bottomRightRay.GetPoint(brsearchDistance);
                    var trdir = topRightRay.GetPoint(trsearchDistance) - trstart;
                    topRightRay = new Ray(trstart, trdir);
                    interceptPlane.Raycast(topLeftRay, out trhit);
                    trPoint = topRightRay.GetPoint(trhit);
                }
                else
                {
                    var blstart = topLeftRay.GetPoint(tlsearchDistance);
                    var bldir = bottomLeftRay.GetPoint(blsearchDistance) - blstart;
                    bottomLeftRay = new Ray(blstart, bldir);
                    interceptPlane.Raycast(bottomLeftRay, out blhit);
                    blPoint = bottomLeftRay.GetPoint(blhit);

                    var brstart = topRightRay.GetPoint(trsearchDistance);
                    var brdir = bottomRightRay.GetPoint(brsearchDistance) - brstart;
                    bottomRightRay = new Ray(brstart, brdir);
                    interceptPlane.Raycast(bottomRightRay, out brhit);
                    brPoint = bottomRightRay.GetPoint(brhit);
                }
            }

            Vector2[] points = new Vector2[4];
            points[0] = new Vector2(tlPoint.x, tlPoint.z);
            points[1] = new Vector2(trPoint.x, trPoint.z);
            points[2] = new Vector2(blPoint.x, blPoint.z);
            points[3] = new Vector2(brPoint.x, brPoint.z);
            Rect r = new Rect(tlPoint, Vector2.zero).Encapsulate(points);

            pos = new Vector3(r.center.x,0,r.center.y);
            size = new Vector3(r.size.x,0,r.size.y);
        }

        return (pos, size);
    }

    public static List<Vector3[]> CalculateSearchFustrumField(this Camera cam, float searchDistance = 10000)
    {
        Plane interceptPlane = new Plane(Vector3.up, Vector3.zero);
        List<Vector3[]> boundarylines = new List<Vector3[]>();

        //Four corners of screen (0,0) is bottom left (1,1) is top right
        Vector3 bl = new Vector3(0, 0);
        Vector3 tl = new Vector3(0, Screen.height);
        Vector3 tr = new Vector3(Screen.width, Screen.height);
        Vector3 br = new Vector3(Screen.width, 0);

        //Create Ray, this will match camera fustrum
        Ray bottomLeftRay = cam.ScreenPointToRay(bl);
        Ray topLeftRay = cam.ScreenPointToRay(tl);
        Ray topRightRay = cam.ScreenPointToRay(tr);
        Ray bottomRightRay = cam.ScreenPointToRay(br);

        interceptPlane.Raycast(bottomLeftRay, out float blhit);
        interceptPlane.Raycast(topLeftRay, out float tlhit);
        interceptPlane.Raycast(topRightRay, out float trhit);
        interceptPlane.Raycast(bottomRightRay, out float brhit);

        Vector3 blPoint = bottomLeftRay.GetPoint(blhit);
        Vector3 tlPoint = topLeftRay.GetPoint(tlhit);
        Vector3 trPoint = topRightRay.GetPoint(trhit);
        Vector3 brPoint = bottomRightRay.GetPoint(brhit);

        Plane camPlane = new Plane(-cam.transform.forward, cam.transform.position + cam.transform.forward * searchDistance);
        camPlane.Raycast(bottomLeftRay, out float blsearchDistance);
        camPlane.Raycast(topLeftRay, out float tlsearchDistance);
        camPlane.Raycast(topRightRay, out float trsearchDistance);
        camPlane.Raycast(bottomRightRay, out float brsearchDistance);

        bool istop = tlhit > 0 && tlhit <= tlsearchDistance &&
                     trhit > 0 && trhit <= trsearchDistance;
        bool isbot = blhit > 0 && blhit <= blsearchDistance &&
                     brhit > 0 && brhit <= brsearchDistance;

        if (istop || isbot)
        {
            if (istop != isbot)
            {
                if (isbot)
                {
                    var tlstart = bottomLeftRay.GetPoint(blsearchDistance);
                    var tldir = topLeftRay.GetPoint(tlsearchDistance) - tlstart;
                    topLeftRay = new Ray(tlstart, tldir);
                    interceptPlane.Raycast(topLeftRay, out tlhit);
                    tlPoint = topLeftRay.GetPoint(tlhit);

                    var trstart = bottomRightRay.GetPoint(brsearchDistance);
                    var trdir = topRightRay.GetPoint(trsearchDistance) - trstart;
                    topRightRay = new Ray(trstart, trdir);
                    interceptPlane.Raycast(topLeftRay, out trhit);
                    trPoint = topRightRay.GetPoint(trhit);
                }
                else
                {
                    var blstart = topLeftRay.GetPoint(tlsearchDistance);
                    var bldir = bottomLeftRay.GetPoint(blsearchDistance) - blstart;
                    bottomLeftRay = new Ray(blstart, bldir);
                    interceptPlane.Raycast(bottomLeftRay, out blhit);
                    blPoint = bottomLeftRay.GetPoint(blhit);

                    var brstart = topRightRay.GetPoint(trsearchDistance);
                    var brdir = bottomRightRay.GetPoint(brsearchDistance) - brstart;
                    bottomRightRay = new Ray(brstart, brdir);
                    interceptPlane.Raycast(bottomRightRay, out brhit);
                    brPoint = bottomRightRay.GetPoint(brhit);
                }
            }

            Vector3[] left = new Vector3[2]
            {
                blPoint,
                tlPoint
            };
            Vector3[] top = new Vector3[2]
            {
                tlPoint,
                trPoint
            };
            Vector3[] right = new Vector3[2]
            {
                trPoint,
                brPoint
            };
            Vector3[] bot = new Vector3[2]
            {
                brPoint,
                blPoint
            };
            boundarylines.Add(left);
            boundarylines.Add(top);
            boundarylines.Add(right);
            boundarylines.Add(bot);
        }

        return boundarylines;
    }
}
