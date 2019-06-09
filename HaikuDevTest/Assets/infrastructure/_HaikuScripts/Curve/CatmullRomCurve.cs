using UnityEngine;
using System;

public class CatmullRomCurve{
    
    Vector3[] _controlPoints;

	float _lengthOfAllSegments;
    public float lengthOfAllSegments{
        get{
            return _lengthOfAllSegments;
        }
    }

    float[] _segmentLengths;

    public CatmullRomCurve(Vector3[] pPassThroughPoints){
        SetPassThroughPoints(pPassThroughPoints);
	}

    public void SetPassThroughPoints(Vector3[] pPassThroughPoints){
		GenerateControlPoints(pPassThroughPoints);

		int numSections = _controlPoints.Length - 3;

		_segmentLengths = new float[numSections];

		_lengthOfAllSegments = 0;

		for (int section = 0; section < numSections; ++section) {
            float length = Vector3.Distance(_controlPoints[section + 1], _controlPoints[section + 2]);
			_lengthOfAllSegments += length;
			_segmentLengths[section] = length;
		}
    }

    public Vector3 GetClosestPointOnPath(Vector3 pSourcePoint, out float pProgress){
		int numSections = _controlPoints.Length - 3;

        Vector3 pointOnSegment;
        Vector3 segmentStart;
		Vector3 segmentEnd;

        float minDistance = Mathf.Infinity;
        int index = -1;
        float u = -1;
        float distanceFromSource;
        float previousDistance = 0f;
        float pointToSegmentStartDistance;

        pProgress = -1f;

        for (int section = 0; section < numSections; ++section) {
            segmentStart = _controlPoints[section + 1];
            segmentEnd = _controlPoints[section + 2];
            pointOnSegment = GetClosestPointOnLineSegment(segmentStart, segmentEnd , pSourcePoint);
                
            distanceFromSource = Vector3.Distance(pointOnSegment, pSourcePoint);
          
            if(distanceFromSource < minDistance){
                minDistance = distanceFromSource;
                index = section;
                pointToSegmentStartDistance = Vector3.Distance(pointOnSegment, segmentStart);
                pProgress = Mathf.InverseLerp(0f, _lengthOfAllSegments, previousDistance + pointToSegmentStartDistance);
                u = Mathf.InverseLerp(0, _segmentLengths[section], pointToSegmentStartDistance);
            }

			previousDistance += _segmentLengths[section];
		}

		Vector3 a = _controlPoints[index];
		Vector3 b = _controlPoints[index + 1];
		Vector3 c = _controlPoints[index + 2];
		Vector3 d = _controlPoints[index + 3];

       
		//Catmull-Rom equation
		return .5f * (
			(-a + 3f * b - 3f * c + d) * (u * u * u)
			+ (2f * a - 5f * b + 4f * c - d) * (u * u)
			+ (-a + c) * u
			+ 2f * b
			);
    }

	public Vector3 GetPointOnPath(float pT) {

		pT = Mathf.Clamp(pT, 0f, 1f);

		float desiredDistance = _lengthOfAllSegments * pT;
		float currentDistance = 0;
		float previousDistance = 0;

        int numSections = _controlPoints.Length - 3;

		for (int section = 0; section < numSections; ++section) {
			currentDistance += _segmentLengths[section];

			if (currentDistance >= desiredDistance) {
				float u = Mathf.InverseLerp(previousDistance, currentDistance, desiredDistance);
				Vector3 a = _controlPoints[section];
				Vector3 b = _controlPoints[section + 1];
				Vector3 c = _controlPoints[section + 2];
				Vector3 d = _controlPoints[section + 3];


				//Catmull-Rom equation
				return .5f * (
					(-a + 3f * b - 3f * c + d) * (u * u * u)
					+ (2f * a - 5f * b + 4f * c - d) * (u * u)
					+ (-a + c) * u
					+ 2f * b
					);

			}
			previousDistance = currentDistance;

		}

		return Vector3.zero;
	}

	Vector3 GetClosestPointOnLineSegment(Vector3 vA, Vector3 vB, Vector3 vPoint) {
		Vector3 vVector1 = vPoint - vA;
		Vector3 vVector2 = (vB - vA).normalized;

        float d = Vector3.Distance(vA, vB);
        float t = Vector3.Dot(vVector2, vVector1);

		if (t <= 0)
			return vA;

		if (t >= d)
			return vB;

		Vector3 vVector3 = vVector2 * t;

		return vA + vVector3;
	}

    void GenerateControlPoints(Vector3[] pPassThroughPoints){
		Vector3[] suppliedPath;

		//create and store path points:
		suppliedPath = pPassThroughPoints;

		//populate calculate path;
		int offset = 2;
		_controlPoints = new Vector3[suppliedPath.Length + offset];
		Array.Copy(suppliedPath, 0, _controlPoints, 1, suppliedPath.Length);

		//populate start and end control points:
		_controlPoints[0] = _controlPoints[1] + (_controlPoints[1] - _controlPoints[2]);
		_controlPoints[_controlPoints.Length - 1] = _controlPoints[_controlPoints.Length - 2] +
		(_controlPoints[_controlPoints.Length - 2] - _controlPoints[_controlPoints.Length - 3]);

		//is this a closed, continuous loop? yes? well then so let's make a continuous Catmull-Rom spline!
		if (_controlPoints[1] == _controlPoints[_controlPoints.Length - 2]) {
			Vector3[] tmpLoopSpline = new Vector3[_controlPoints.Length];
			Array.Copy(_controlPoints, tmpLoopSpline, _controlPoints.Length);
			tmpLoopSpline[0] = tmpLoopSpline[tmpLoopSpline.Length - 3];
			tmpLoopSpline[tmpLoopSpline.Length - 1] = tmpLoopSpline[2];
			_controlPoints = new Vector3[tmpLoopSpline.Length];
			Array.Copy(tmpLoopSpline, _controlPoints, tmpLoopSpline.Length);
		}

    }
}
