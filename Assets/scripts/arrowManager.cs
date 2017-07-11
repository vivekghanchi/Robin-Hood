using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowManager : MonoBehaviour {

	public static arrowManager instance;

	public SteamVR_TrackedObject trackedObj;

	private GameObject currentArrow;
	public GameObject arrowPrefab;
	public GameObject stringAttachPoint;
	public GameObject arrowStartPoint;
	public GameObject stringStartPoint;

	private bool isAttach = false;

	void Awake(){
		if (instance == null)
			instance = this;
	}

	void onDestroy(){
		if (instance == this)
			instance = null;
	}

	void Start() {
		
	}

	void Update() {
		attachArrow ();
		PullString ();
	}

	private void PullString(){
		if (isAttach) {
			float dist = (stringStartPoint.transform.position - trackedObj.transform.position).magnitude;
			stringAttachPoint.transform.position = stringStartPoint.transform.localPosition + new Vector3 (5F * dist, 0f, 0f);

			var device = SteamVR_Controller.Input((int)trackedObj.index);
			if (device.GetTouchUp (SteamVR_Controller.ButtonMask.Trigger)) {
				fire ();
			}
		}
	}

	private void fire(){

		float dist = (stringStartPoint.transform.position - trackedObj.transform.position).magnitude;

		currentArrow.transform.parent = null;
		currentArrow.GetComponent<Arrow> ().Fired ();
		Rigidbody r = currentArrow.GetComponent<Rigidbody> ();
		r.velocity = currentArrow.transform.forward * 10f* dist;
		r.useGravity = true;

		currentArrow.GetComponent<Collider> ().isTrigger = false;

		stringAttachPoint.transform.position = stringStartPoint.transform.position;
		currentArrow = null;
		isAttach = false;
	}

	private void attachArrow(){
		if(currentArrow == null){
			currentArrow = Instantiate (arrowPrefab);
			currentArrow.transform.parent = trackedObj.transform;
			currentArrow.transform.localPosition = new Vector3 (0f, 0f, .342f);
			currentArrow.transform.localRotation = Quaternion.identity;
		}
	}

	public void AttachBowToArrow(){
		currentArrow.transform.parent = stringAttachPoint.transform;
		currentArrow.transform.localPosition = arrowStartPoint.transform.localPosition;
		currentArrow.transform.rotation = arrowStartPoint.transform.rotation;

		isAttach = true;
	}
}