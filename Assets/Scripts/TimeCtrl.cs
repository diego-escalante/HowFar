using UnityEngine;
using System.Collections;
using System;

public class TimeCtrl : MonoBehaviour {

  private DateTime currentTime;
  
  public delegate void Del(DateTime currentTime);
  public static event Del Tick;

  //===================================================================================================================

  public void setCurrentTime(DateTime newTime) {
    currentTime = newTime;
    StartCoroutine(SecondLoop());
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
