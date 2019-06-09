using System.Collections.Generic;
using UnityEngine;

public class Curve : MonoBehaviour {

	public Color pathColor = Color.cyan;
	public bool isLocal;
	public int smoothAmount = 20;

    [HideInInspector]
	public List<Vector3> nodes = new List<Vector3>() { Vector3.zero, Vector3.zero };

	public CatmullRomCurve GetLocalSpaceCurve(Transform pTransform) {
        return new CatmullRomCurve(GetLocalPassThroughPoints(pTransform));
	}

	public CatmullRomCurve GetWorldSpaceCurve() {
        return new CatmullRomCurve(GetWorldPassThroughPoints());
	}

	
    public void UpdateToLocalSpaceCurve(Transform pTransform, CatmullRomCurve pCurveToUpdate){
        pCurveToUpdate.SetPassThroughPoints(GetLocalPassThroughPoints(pTransform));
    }

	public void UpdateToWorldSpaceCurve(CatmullRomCurve pCurveToUpdate) {
		pCurveToUpdate.SetPassThroughPoints(GetWorldPassThroughPoints());
	}

    Vector3[] GetLocalPassThroughPoints(Transform pTransform){
		Vector3[] passThroughPoints;
		passThroughPoints = new Vector3[nodes.Count];
		Vector3 worldPosition;

		for (int i = 0; i < passThroughPoints.Length; ++i) {
			if (isLocal) {
				worldPosition = transform.TransformPoint(nodes[i]);
			} else {
				worldPosition = nodes[i];
			}
			passThroughPoints[i] = pTransform.InverseTransformPoint(worldPosition);
		}

        return passThroughPoints;
    }

    Vector3[] GetWorldPassThroughPoints(){
		Vector3[] passThroughPoints;
		if (isLocal) {
			passThroughPoints = new Vector3[nodes.Count];
			for (int i = 0; i < passThroughPoints.Length; ++i) {
				passThroughPoints[i] = transform.TransformPoint(nodes[i]);
			}
		} else {
			passThroughPoints = nodes.ToArray();
		}
        return passThroughPoints;
    }


}
