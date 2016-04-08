using UnityEngine;
using System.Collections;

public class TimeCtrl : MonoBehaviour {

  private System.DateTime currentTime;
  private bool loopStarted = false;
  
  public delegate void Del(System.DateTime currentTime);
  public static event Del Tick;
  public System.DateTime CurrentTime {get{return currentTime;}}

  //===================================================================================================================

  public void setCurrentTime(System.DateTime newTime) {
    currentTime = newTime;
    if(!loopStarted) {
      loopStarted = true;
      StartCoroutine(SecondLoop());
    }
  }

  //===================================================================================================================

  private IEnumerator SecondLoop() {

    //Do this every second.
    while(true) {
      //Add a second to current time.
      currentTime = currentTime.AddSeconds(1);

      //Make all subscribers do what they need to do.
      if(Tick != null) Tick(currentTime);

      //Wait one second.
      yield return new WaitForSeconds(1);
    }
  }

}
