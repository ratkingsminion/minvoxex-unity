using UnityEngine;
using System.Collections;

namespace RatKing.MinimalVoxelExample {

	public class EditorMouseMovement : MonoBehaviour {
		public Camera cam;
		public float rotateSpeed = 150.0f;
		public float moveSpeed = 10.0f;
		//
		Transform camTrans;
		Transform dummy;

		//

		void Start() {
			camTrans = cam.transform;
			dummy = new GameObject("_CAMERA").transform; // TODO: needed?
			dummy.position = camTrans.position;
			dummy.eulerAngles = new Vector3(0.0f, camTrans.eulerAngles.y, 0.0f);
			camTrans.parent = dummy;
		}

		void Update() {
			// move
			dummy.Translate(Time.deltaTime * moveSpeed * (
				camTrans.forward * Input.GetAxis("Vertical") +
				camTrans.right * Input.GetAxis("Horizontal") +
				camTrans.up * Input.GetAxis("UpAndDown")),
				Space.World);
			if (Input.GetButton("Fire3")) {
				// rotate
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				dummy.Rotate(Vector3.up, Time.deltaTime * rotateSpeed * Input.GetAxis("Mouse X"), Space.World);
				camTrans.Rotate(Vector3.right, Time.deltaTime * rotateSpeed * -Input.GetAxis("Mouse Y"), Space.Self);
			}
			else {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}
	}

}