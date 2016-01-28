using UnityEngine;
using System.Collections;

public class MyMovement : MonoBehaviour {

  //How Far?
  private float lookSensitivity = 5f;
  private Rigidbody rb;
  private Transform head;
  private float speed = 5f;
  private bool isMoving = false;

  public bool IsMoving {get{return isMoving;}}

  //===================================================================================================================

  private void Start() {
    rb = GetComponent<Rigidbody>();
    transform.position = new Vector3(0, -0.5f, 8 * 24);
    head = transform.GetChild(0);
  }

  //===================================================================================================================

  private void Update() {

    Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
    rb.velocity = transform.TransformDirection(direction * speed);
    isMoving = Vector3.zero == direction ? false : true;

    //Keep me in the desired x-range.
    Vector3 newPos = transform.position;
    newPos.x = Mathf.Clamp(newPos.x, -2.5f, 2.5f);
    transform.position = newPos;

    //Rotate my body.
    transform.Rotate(0, Input.GetAxis("Mouse X")*lookSensitivity , 0);

    //Tilt my head.
    float newAngle = -Input.GetAxis("Mouse Y")*lookSensitivity + head.eulerAngles.x;
    if(newAngle < 270 && newAngle > 180) newAngle = 270;
    else if(newAngle > 90 && newAngle <= 180) newAngle = 90;
    head.localEulerAngles = new Vector3(newAngle, 0, 0);
  }
}