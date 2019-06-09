using UnityEngine;
using System.Collections;

public interface IPuzzleHandler
{
	void Skip();

	void Activate();

	void Deactivate();

	void ResetPuzzle();
}

