using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;


public class Helpers
{
    public static float IsAtEndWaypoint(Vector3 currentPosition, Vector3 waypointFromPosition, Vector3 waypointToPosition)
    {
        var a = currentPosition - waypointFromPosition;
        //The vector between the waypoints
        var b = waypointToPosition - waypointFromPosition;

        //Vector projection from https://en.wikipedia.org/wiki/Vector_projection
        //To know if we have passed the upcoming waypoint we need to find out how much of b is a1
        //a1 = (a.b / |b|^2) * b
        //a1 = progress * b -> progress = a1 / b -> progress = (a.b / |b|^2)
        return (a.x * b.x + a.y * b.y + a.z * b.z) / (b.x * b.x + b.y * b.y + b.z * b.z);
    }

    public static bool IsApproximately(float a, float b, float tolerance)
    {
        return Mathf.Abs(a - b) < tolerance;
    }

    public static bool IsVectorApproximately(Vector3 a, Vector3 b, float tolerance)
    {
        if (Mathf.Abs(a.x - b.x) < tolerance
            && Mathf.Abs(a.y - b.y) < tolerance
            && Mathf.Abs(a.z - b.z) < tolerance)
        {
            return true;
        }

        return false;
    }

    public static void GetCircularPoints(float radius, Vector3 center, float angleIntervalInRadians, List<Vector3> points, out List<Vector3> pointsResult)
    {
        pointsResult = points;

        for (var interval = angleIntervalInRadians; interval < 2 * Mathf.PI; interval += angleIntervalInRadians)
        {
            var X = center.x + (radius * Mathf.Cos(interval));
            var Z = center.z + (radius * Mathf.Sin(interval));

            pointsResult.Add(new Vector3(X, 0.25f, Z));
        }
    }

    public static float Map01(float value, float min, float max)
    {
        return (value - min) * 1f / (max - min);
    }

    public static bool IsOdd(int value)
    {
        return value % 2 != 0;
    }


    #region create classes with reflection
    public static I CreateInstance<I>(string namespaceName, string name) where I : class
    {
        var typeClass = System.Type.GetType(namespaceName + name);
        return Activator.CreateInstance(typeClass) as I;
    }

    public static I CreateInstance<I>(string namespaceName, string name, object[] someParams) where I : class
    {
        var typeClass = System.Type.GetType(namespaceName + name);
        return Activator.CreateInstance(typeClass, someParams) as I;
    }

    public static object CreateInstance(string strFullyQualifiedName)
    {
        var t = Type.GetType(strFullyQualifiedName);
        return Activator.CreateInstance(t);
    }

    public static object CreateInstance(string strFullyQualifiedName, object[] someParams)
    {
        var t = Type.GetType(strFullyQualifiedName);
        return Activator.CreateInstance(t, someParams);
    }

    public static bool CheckClassName(ref List<string> classesList, string name)
    {
        for (var i = 0; i < classesList.Count; ++i)
        {
            var result = String.Equals(classesList[i], name, StringComparison.OrdinalIgnoreCase);
            if (result)
            {
                return true;
            }
        }

        return false;
    }

    public static List<string> GetClasses(string nameSpace)
    {
        var asm = Assembly.GetExecutingAssembly();

        var namespacelist = new List<string>();
        var classlist = new List<string>();

        foreach (var type in asm.GetTypes())
        {
            var result = String.Equals(type.Namespace, nameSpace, StringComparison.OrdinalIgnoreCase);

            if (result)
            {
                namespacelist.Add(type.Name);
            }
        }

        foreach (var classname in namespacelist)
            classlist.Add(classname);

        return classlist;
    }
    #endregion



    public static void Shuffle<T>(List<T> list)
    {
        //1000 seems a good number of times to shuffle
        //to get a decent random list
        for (var i = 0; i < 1000; ++i)
        {
            var a = UnityEngine.Random.Range(0, list.Count);
            var b = UnityEngine.Random.Range(0, list.Count);
            var t = list[a];
            list[a] = list[b];
            list[b] = t;
        }
    }




    public static bool CheckStraightVisibility(Vector3 destination, Vector3 startPosition, int mask)
    {
        RaycastHit hit;
        Vector3 directionTmp;

        directionTmp.x = destination.x - startPosition.x;
        directionTmp.y = startPosition.y + 0.5f;
        directionTmp.z = destination.z - startPosition.z;

        if (Physics.SphereCast(new Vector3(startPosition.x, startPosition.y + 0.5f, startPosition.z),
            0.2f,
                directionTmp.normalized,
                out hit,
                directionTmp.magnitude,
                mask))
        {
            return false;
        }


        return true;
    }
    
    public static bool CheckStraightRaycastVisibility(Vector3 destination, Vector3 initPosition, int mask, float radius, float extraMaxDistance, RaycastHit[] hits)
    {
        var directionTmp = Vector3.zero;
        directionTmp.x = destination.x - initPosition.x;
        directionTmp.y = 0.5f;
        directionTmp.z = destination.z - initPosition.z;
        
        //check whether the player is visible, with the double of the close distance
        var totalHits = Physics.SphereCastNonAlloc(
            new Vector3(initPosition.x, initPosition.y + 1f, initPosition.z), //above feet position
            radius,
            directionTmp.normalized,
            hits,
            directionTmp.magnitude + extraMaxDistance, //just a litte bit more of distance, just a bit..
            mask);

        return totalHits == 0 ? true : false;
    }


    public static List<Vector3> GetFilteredPointsAroundPosition(Vector3 ourPosition, float radius, int mask, float offsetDistance = 0f)
    {
        //get points around the destination point (usually the player position)
        //pretty much at 8º degree of distance between points (converted then to radians)
        var tmpPoints = new List<Vector3>();

        GetCircularPoints(radius, ourPosition, Mathf.PI / 7.0f, tmpPoints, out tmpPoints);

        for (var i = tmpPoints.Count - 1; i > -1; --i)
        {
            if (CheckStraightVisibility(tmpPoints[i], ourPosition, mask))
            {
                continue;
            }
            else
            {
                tmpPoints.RemoveAt(i);
            }
        }

        return tmpPoints;
    }


    public static bool IsWithinViewport(Camera camera, Vector3 position)
    {
        var thePos = camera.WorldToViewportPoint(position);

        //left and right side
        //top and down side
        if ((thePos.x > 0f && thePos.x < 1.0f)
            && thePos.y > 0f && thePos.y < 1.0f
            && thePos.z > 0f)
        {
            return true;
        }

        return false;
    }

    public static bool IsInCircle(int x1, int y1, int x2, int y2, float radius)
    {
        return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)) <= radius;
    }
    

    public static Vector3 CalculateNearestTargetPosition(List<Vector3> targets, Vector3 initPosition)
    {
        var positionId = -1;
        var minSqDist = float.MaxValue;

        for (var i = 0; i < targets.Count; ++i)
        {
            var diff = targets[i] - initPosition;
            var sqDiff = Vector3.Dot(diff, diff);
            
            if (sqDiff < minSqDist)
            {
                positionId = i;
                minSqDist = sqDiff;
            }
        }

        return targets[positionId];
    }

    public static int FindClosestDistance(Vector3 p, Vector3[] targets)
    {
        var min = -1;
        var minSqDist = float.MaxValue;

        for (var i = 0; i < targets.Length; ++i) {
            var diff = p - targets[i];
            var sqDiff = Vector3.Dot(diff, diff);
            if (sqDiff < minSqDist)
            {
                min = i;
                minSqDist = sqDiff;
            }
        }
        return min;
    }


    public static void Sort<T>(List<T> list)
    {
        //li.Sort((a, b) => a.CompareTo(b)); // ascending sort
        //li.Sort((a, b) => -1* a.CompareTo(b)); // descending sort

        // Sort the numbers by their first digit.
        // We use ToString on each number.
        // We access the first character of the string and compare that.
        // extremely slow and too garbage created...
        list.Sort(delegate (T q1, T q2) { return -1 * q1.ToString()[0].CompareTo(q2.ToString()[0]); });
    }


    //fast removal from list test code
    // private void RemoveTest4(ref List<string> list)
    // {
    //     List<string> newList = new List<string>();
    //     for (int i = 0; i < list.Count; i++)
    //     {
    //         if (i % 5 == 0)
    //             continue;
    //         else
    //             newList.Add(list[i]);
    //     }

    //     list.RemoveRange(0, list.Count);
    //     list.AddRange(newList);
    // }




    #region Math calculations
    public static float WrapAngle180(float angle)
    {
        var newAngle = angle;
        if (angle > 179)
            newAngle = -180;
        if (angle < -179)
            newAngle = 180;
        return newAngle;
    }

    /// <summary>
    /// Keep angle in -360 to 360 interval.
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static float WrapAngle360(float angle)
    {
        var newAngle = angle;
        if (angle > 359)
            newAngle = -360;
        if (angle < -359)
            newAngle = 360;
        return newAngle;
    }

    /// <summary>
    /// Plane dot coodinate function.
    /// </summary>
    /// <param name="plane"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static float PlaneDotCoordinate(ref Plane plane, ref Vector3 value)
    {
        var num1 = ((value.x * plane.normal.x) + (value.y * plane.normal.y)) +
                   (value.z * plane.normal.z);
        var num2 = num1 + plane.distance;
        return num2;
    }

    /// <summary>
    /// Test intersection sphere with frustum.
    /// </summary>
    /// <param name="planes">plane array</param>
    /// <param name="center">center of sphere</param>
    /// <param name="radius">radius of sphere</param>
    /// <returns>true or false</returns>
    public static bool TestSphereFrustum(Plane[] planes, ref Vector3 center, float radius)
    {
        for (var i = 0; i < 6; ++i)
        {
            var dotCoord = PlaneDotCoordinate(ref planes[i], ref center) + radius;
            if (dotCoord < 0.0f)
                return false;
        }
        return true;
    }

    /// <summary>
    /// Get closest point to line ( duh ).
    /// </summary>
    /// <param name="A">line start</param>
    /// <param name="B">line end</param>
    /// <param name="P">point to test ( reference )</param>
    /// <returns>closest point</returns>
    public static Vector3 GetClosestPoint2Line(Vector3 A, Vector3 B, ref Vector3 P)
    {
        var AP = P - A;       //Vector from A to P   
        var AB = B - A;       //Vector from A to B  

        var magnitudeAB = Vector3.SqrMagnitude(AB);// //Magnitude of AB vector (it's length squared)     
        var ABAPproduct = Vector3.Dot(AP, AB);    //The DOT product of a_to_p and a_to_b     
        var distance = ABAPproduct / magnitudeAB; //The normalized "distance" from a to your closest point  

        if (distance < 0)     //Check if P projection is over vectorAB     
        {
            return A;

        }
        else if (distance > 1)
        {
            return B;
        }
        else
        {
            return A + AB * distance;
        }
    }

    /// <summary>
    /// Check if point is between line start and end.
    /// Point does not have to be on line
    /// </summary>
    /// <param name="lineA">line start</param>
    /// <param name="lineB">line end</param>
    /// <param name="point">point to test ( reference )</param>
    /// <returns>true or false</returns>
    public static bool IsInsideLineSegment(Vector3 lineA, Vector3 lineB, ref Vector3 point)
    {
        var AP = point - lineA;
        var AB = lineB - lineA;

        var magnitudeAB = Vector3.SqrMagnitude(AB);
        var ABAPproduct = Vector3.Dot(AP, AB);
        var distance = ABAPproduct / magnitudeAB;

        if (distance < 0)
        {
            return false;

        }
        else if (distance > 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Calculates closest point to line and 
    /// assignes distances from two edges to that closest point.
    /// /// </summary>
    /// <param name="A">first point of line</param>
    /// <param name="B">second point of line</param>
    /// <param name="P">point in space to test</param>
    /// <param name="distFromA">assignes distance from point a</param>
    /// <param name="distFromB">assignes distnace from point b</param>
    public static void GetLineEdgeDistances(Vector3 A, Vector3 B, Vector3 P, out float distFromA, out float distFromB)
    {
        var AP = P - A;
        var AB = B - A;

        var magnitudeAB = Vector3.SqrMagnitude(AB);
        var ABAPproduct = Vector3.Dot(AP, AB);
        var distance = ABAPproduct / magnitudeAB;

        var closest = A;
        distFromA = 0.0f;
        distFromB = AB.magnitude;
        if (distance > 1)
        {
            closest = B;
            distFromA = AB.magnitude;
            distFromB = 0.0f;
        }
        else
        {
            closest = A + AB * distance;
            distFromA = Vector3.Distance(closest, A);
            distFromB = Vector3.Distance(closest, B);
        }
    }

    /// <summary>
    /// Get closest point to plane.
    /// </summary>
    /// <param name="plane">plane reference</param>
    /// <param name="inpos">point reference</param>
    /// <returns>closest point</returns>
    public static Vector3 GetClosestPoint2Plane(ref Plane plane, ref Vector3 inpos)
    {
        var dist2Plane = plane.GetDistanceToPoint(inpos);
        var normal = plane.normal;
        return inpos - normal * dist2Plane;
    }

    /// <summary>
    /// Calculate transform line points.
    /// </summary>
    /// <param name="xform">transform from which line is calculated</param>
    /// <param name="pt1">out point 1 of line</param>
    /// <param name="pt2">out point 2 of line</param>
    /// <param name="zoffset">offset from middle in z direction</param>
    /// <param name="reversed">is reversed ?</param>
    public static void CalculateLinePoints(Transform xform, out Vector3 pt1, out Vector3 pt2, float zoffset, bool reversed = false)
    {
        var scaleX = xform.lossyScale.x;
        var halfScaleX = scaleX * 0.5f;
        pt1 = xform.position - xform.right * halfScaleX;
        pt2 = xform.position + xform.right * halfScaleX;

        var dir = (reversed ? -xform.forward : xform.forward);

        pt1 = pt1 + dir * zoffset;
        pt2 = pt2 + dir * zoffset;
    }

    /// <summary>
    /// Calculate direction axis of transform.
    /// </summary>
    /// <param name="xform">transform</param>
    /// <param name="direction">out axis direction</param>
    /// <param name="distance">out axis length</param>
    /// <returns>axis vector</returns>
    public static Vector3 CalculateDirection(Transform xform, out int direction, out float length)
    {
        var parent = xform.parent;
        var point = (xform.position - parent.position) + xform.position;
        point = xform.InverseTransformPoint(point);

        var axis = new Vector3(1.0f, 0.0f, 0.0f);

        // Calculate longest axis
        direction = 0;
        if (Mathf.Abs(point[1]) > Mathf.Abs(point[0]))
        {
            direction = 1;
            axis = new Vector3(0.0f, 1.0f, 0.0f);
        }
        if (Mathf.Abs(point[2]) > Mathf.Abs(point[direction]))
        {
            direction = 2;
            axis = new Vector3(0.0f, 0.0f, 1.0f);
        }

        length = point[direction];
        axis *= Mathf.Sign(length);
        return axis;
    }

    /// <summary>
    /// Get direction axis of transform.
    /// </summary>
    /// <param name="xform">transform</param>
    /// <returns></returns>
    public static Vector3 CalculateDirectionAxis(Transform xform)
    {
        var parent = xform.parent;
        var point = (xform.position - parent.position) + xform.position;
        point = xform.InverseTransformPoint(point);

        var axis = new Vector3(1.0f, 0.0f, 0.0f);

        // Calculate longest axis
        var direction = 0;
        var length = 0.0f;
        if (Mathf.Abs(point[1]) > Mathf.Abs(point[0]))
        {
            direction = 1;
            axis = new Vector3(0.0f, 1.0f, 0.0f);
        }
        if (Mathf.Abs(point[2]) > Mathf.Abs(point[direction]))
        {
            direction = 2;
            axis = new Vector3(0.0f, 0.0f, 1.0f);
        }

        length = point[direction];
        axis *= Mathf.Sign(length);

        return axis;
    }

    /// <summary>
    /// Get direction axis of transform.
    /// </summary>
    /// <param name="xform">transform</param>
    /// <returns></returns>
    public static Vector3 CalculateDirectionAxis(Transform xform, out Vector3 axis_out)
    {
        var parent = xform.parent;
        var point = (xform.position - parent.position) + xform.position;
        point = xform.InverseTransformPoint(point);
        axis_out = point;
        var axis = new Vector3(1.0f, 0.0f, 0.0f);

        // Calculate longest axis
        var direction = 0;
        var length = 0.0f;
        if (Mathf.Abs(point[1]) > Mathf.Abs(point[0]))
        {
            direction = 1;
            axis = new Vector3(0.0f, 1.0f, 0.0f);
        }
        if (Mathf.Abs(point[2]) > Mathf.Abs(point[direction]))
        {
            direction = 2;
            axis = new Vector3(0.0f, 0.0f, 1.0f);
        }

        length = point[direction];
        axis *= Mathf.Sign(length);

        return axis;
    }
    
    
    public static Vector3 Divide(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
    }
    
    public static Vector3 Multiply(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }
    #endregion
}
