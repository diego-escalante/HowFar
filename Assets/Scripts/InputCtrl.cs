using UnityEngine;
using System.Collections;

public class InputCtrl : MonoBehaviour {

  public delegate void InputButton();
  public delegate void InputAxis(float f);
  public delegate void InputAxes(float x, float z);

  public static event InputButton cancelPressed;
  public static event InputButton mouseClick;
  
  public static event InputAxes moveAxes;

  public static event InputAxis mouseXAxis;
  public static event InputAxis mouseYAxis;

  //===================================================================================================================

  private void Update() {
    if(Input.GetButtonDown("Cancel") && cancelPressed != null) cancelPressed();
    if(Input.GetButtonDown("mouseClick") && mouseClick != null) mouseClick();

    if(moveAxes != null) moveAxes(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    
    float f;
    if((f = Input.GetAxis("Mouse X")) != 0 && mouseXAxis != null) mouseXAxis(f);
    if((f = Input.GetAxis("Mouse Y")) != 0 && mouseYAxis != null) mouseYAxis(f);
  }

}
