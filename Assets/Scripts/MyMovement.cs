using UnityEngine;
using System.Collections;

public class MyMovement : MonoBehaviour {

  //How Far?
  private Rigidbody rb;
  private float speed = 10f;

  //===================================================================================================================

  private void Start() {
    rb = GetComponent<Rigidbody>();
  }

  //===================================================================================================================

  private void Update() {
    Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
    rb.velocity = direction * speed;
  }
}