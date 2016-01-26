using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class RoomCtrl : MonoBehaviour {

  private int roomNumber;                   //The room number.
  private DateTime endTime;                 //The end time for this timer.
  private bool timeReached = false;         //Has this timer reached 0?
  private Text display;                     //The countdown display.
  private static GameObject roomPrefab;     //The room prefab.

  //===================================================================================================================

  private void Awake() {
    TimeCtrl.Tick += tick;
    display = GetComponentInChildren<Text>();
    if(roomPrefab == null) roomPrefab = Resources.Load("Room", typeof(GameObject)) as GameObject;
  }

  //===================================================================================================================

  private void tick(DateTime currentTime) {
    
    //If we have already reached the end date, just flash the display.
    if(timeReached) display.gameObject.SetActive(currentTime.Second % 2 == 0);

    //else if we just ticked down to 0? 
    else if(currentTime >= endTime) open(false, currentTime);

    //else update display.
    else updateDisplay(currentTime);
  }

  //===================================================================================================================

  private void updateDisplay(DateTime currentTime){
    //Calculate the time left, get a string from that.
    TimeSpan timeLeft = endTime.Subtract(currentTime);
    string text = timeLeft.ToString();

    //Modify the string for pretty display.
    if(timeLeft.Days == 0) text = text.Insert(0, "000:");
    else {
      text = text.Replace(".", ":");
      if(timeLeft.Days < 10) text = text.Insert(0, "0");
      if(timeLeft.Days < 100) text = text.Insert(0, "0");
    }

    //Display the text.
    display.text = text;
    
    //Todo: play a sound.
  }

  //===================================================================================================================

  private void open(bool instantly, DateTime currentTime){
    //Remember the timer in this room has reached 0.
    timeReached = true;

    //Create a new room, set its timer to end next year.
    GameObject r = Instantiate(roomPrefab, transform.position + new Vector3(0,0,8), Quaternion.identity) as GameObject;
    RoomCtrl rCtrl = r.GetComponent<RoomCtrl>();
    rCtrl.setName(roomNumber + 1);
    rCtrl.setEndTime(endTime.AddYears(1), currentTime);

    //Open this room's door.
    if(instantly) GetComponentInChildren<Animator>().SetTrigger("AlreadyOpen");
    else GetComponentInChildren<Animator>().SetTrigger("Open");
    display.text = "000:00:00:00";
  }

  //===================================================================================================================

  public void setEndTime(DateTime newTime, DateTime currentTime){
    endTime = newTime;
    if(currentTime > endTime) open(true, currentTime);
  }

  //===================================================================================================================

  public void setName(int i){
    gameObject.name = "Room " + i;
    roomNumber = i;
  }
}
