using UnityEngine;
using System.Collections;
[System.Serializable]
public class SarcophagusNeighbour {
	
	public SarcophagusNode neighbourNode;
	public EdgeCollider2D connectingLine;

	public SarcophagusNeighbour(SarcophagusNode node, EdgeCollider2D edge)
	{
		this.neighbourNode = node;
		this.connectingLine = edge;
	}
}

