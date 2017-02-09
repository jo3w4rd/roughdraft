using UnityEngine;
using System.Collections;
using Leap;

public class AimCalc : MonoBehaviour {

	public Matrix4x4 thumb;

	// Use this for initialization
	void Start () {
		Vector3 thumbforward = new Vector(0.436329113772f, 0.0f, -0.899787143982f).ToUnity();
		Vector3 thumbup = new Vector(0.804793943718f, 0.447213915513f, 0.390264553767f).ToUnity();
		Quaternion thumbQ = Quaternion.LookRotation (thumbforward, thumbup);

		Vector3 indexforward = new Vector(0.166044313785f, -0.14834045293f, -0.974897120667f).ToUnity();
		Vector3 indexup = new Vector(0.0249066470677f, 0.988936352868f, -0.1462345681f).ToUnity();
		Quaternion indexQ = Quaternion.LookRotation (indexforward, indexup);

		Vector3 midforward = new Vector(0.0295207858556f, -0.148340452932f, -0.988495641481f).ToUnity();
		Vector3 midup = new Vector(-0.145765270107f, 0.977715980076f, -0.151075968756f).ToUnity();
		Quaternion midQ = Quaternion.LookRotation (midforward, midup);

		Vector3 rforward = new Vector(-0.121317937368f, -0.148340347175f, -0.981466810174f).ToUnity();
		Vector3 rup = new Vector(-0.216910468316f, 0.968834928679f, -0.119619102602f).ToUnity();
		Quaternion rQ = Quaternion.LookRotation (rforward, rup);

		Vector3 pforward = new Vector(-0.259328923438f, -0.105851224797f, -0.959970847306f).ToUnity();
		Vector3 pup = new Vector(-0.353350220937f, 0.935459475557f, -0.00769356576168f).ToUnity();
		Quaternion pQ = Quaternion.LookRotation (pforward, pup);

		transform.rotation = pQ;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
