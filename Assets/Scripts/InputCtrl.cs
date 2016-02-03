using UnityEngine;
using System.Collections;

public class InputCtrl : MonoBehaviour {

  public delegate void InputButton();
  public delegate void InputAxis(float f);

  public static event InputButton cancelPressed;
  public static event InputButton mouseClick;
  
  public static event InputAxis horizontalAxis;
  public static event InputAxis verticalAxis;

  //===================================================================================================================

  private void Update() {
    if(Input.GetButtonDown("Cancel") && cancelPressed != null) cancelPressed();
    if(Input.GetButtonDown("mouseClick") && mouseClick != null) mouseClick();

    if(horizontalAxis != null) horizontalAxis(Input.GetAxisRaw("Horizontal"));
    if(verticalAxis != null) verticalAxis(Input.GetAxisRaw("Vertical"));
  }

}
