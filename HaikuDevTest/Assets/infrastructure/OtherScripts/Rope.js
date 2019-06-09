#pragma strict

function Start () {
	transform.localScale.x = 0.0f;
}

function Update () {

	if (Input.GetMouseButton(0)) {
		var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pos.z = 0.0;
		
		var dir = pos - transform.position;
		transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
		transform.localScale.x = dir.magnitude;
		GetComponent.<Renderer>().material.mainTextureScale = Vector2(dir.magnitude, 1.0);
	}

}