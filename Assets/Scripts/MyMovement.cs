using UnityEngine;
using System.Collections;

public class MyMovement : MonoBehaviour {

  //How Far?
  private Rigidbody rb;
  private float speed = 10f;

  //===================================================================================================================

  private void Start() {
    rb = GetComponent<Rigidbody>();
    transform.position = new Vector3(0, -0.5f, 8 * 24);
  }

  //===================================================================================================================

  private void Update() {
    Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
    rb.velocity = direction * speed;

    Vector3 newPos = transform.position;
    if(transform.position.x > 2.5f) newPos.x = 2.5f;
    else if(transform.position.x < -2.5f) newPos.x = -2.5f;
    transform.position = newPos;


    if (Input.GetMouseButton(0)) {
      // transform.LookAt(target);
      transform.Rotate(0, Input.GetAxis("Mouse X")*speed, 0);
      
      // transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Mouse X")*speed);
     }
  }
}