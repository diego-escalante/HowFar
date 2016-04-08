using UnityEngine;
using System.Collections;

public class MoveCtrl : MonoBehaviour {

  //How Far?
  private float lookSensitivity = 5f;           //Mouse sensitivity.
  private Rigidbody rb;                         //My rigidbody.
  private Transform head;                       //My head child.
  private float speed = 5f;                     //How fast I move.
  private bool isMoving = false;                //Am I currently moving?

  public bool IsMoving {get{return isMoving;}}  //Expose movement status for soundCtrl.

  //===================================================================================================================

  private void Awake() {
    rb = GetComponent<Rigidbody>();
    head = transform.GetChild(0);
  }

  //===================================================================================================================

  private void OnEnable() {
    InputCtrl.mouseXAxis += xCamMove;
    InputCtrl.mouseYAxis += yCamMove;
    InputCtrl.moveAxes += move;
  }

  //===================================================================================================================

  private void OnDisable() {
    rb.velocity = Vector3.zero;
    InputCtrl.mouseXAxis -= xCamMove;
    InputCtrl.mouseYAxis -= yCamMove;
    InputCtrl.moveAxes -= move;
  }

  //===================================================================================================================

  private void move(float x, float z) {
    //Get inputs and move.
    Vector3 direction = new Vector3(x, 0, z).normalized;
    rb.velocity = transform.TransformDirection(direction * speed);

    //If we are moving... (Also, update isMoving)
    if((isMoving = Vector3.zero != direction)) {
    //Keep me in the desired x-range.
    Vector3 newPos = transform.position;
    newPos.x = Mathf.Clamp(newPos.x, -2.5f, 2.5f);
    transform.position = newPos;
    }
  }

  //===================================================================================================================

  private void xCamMove(float f){
    //Rotate my body.
    transform.Rotate(0, f*lookSensitivity , 0);
  }

  //===================================================================================================================

  private void yCamMove(float f){
    //Tilt my head.
    float newAngle = -f*lookSensitivity + head.eulerAngles.x;
    if(newAngle < 270 && newAngle > 180) newAngle = 270;
    else if(newAngle > 90 && newAngle <= 180) newAngle = 90;
    head.localEulerAngles = new Vector3(newAngle, 0, 0);
  }
}