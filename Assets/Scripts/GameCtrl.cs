using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameCtrl : MonoBehaviour {

  public GameObject roomPrefab;

  private GameObject currentRoom;
  private Text currentDisplay;
  private List<Text> oldDisplays = new List<Text>();

  private DateTime currentTime;
  private DateTime endTime;

  //===================================================================================================================

  private IEnumerator Start() {

    //GET the current time from the internet.
    WWW www = new WWW("http://www.timeapi.org/utc/now");

    //Wait for the request to be completed.
    yield return www;

    //Prepare received data.
    string currentDate = www.text.Substring(0, www.text.IndexOf("+"));

    //Set up current time and end time.
    currentTime = DateTime.Parse(currentDate);
    currentTime = new DateTime(2016, 5, 6, 23, 59, 45);
    endTime = new DateTime(currentTime.Year, 5, 8);

    //If we are already past the threshold this year, add another year to the endTime.
    if(currentTime > endTime) endTime = new DateTime(currentTime.Year + 1, 5, 8);

    //Build rooms.
    Vector3 position = Vector3.zero;
    for(int i = 0; i < currentTime.Year - 1991; i++){
      GameObject room = Instantiate(roomPrefab, position, Quaternion.identity) as GameObject;
      room.name = "Room " + i.ToString();
      position += new Vector3(0, 0, 8);

      if(i == currentTime.Year - 1992) {
        currentRoom = room;
        currentDisplay = currentRoom.GetComponentInChildren<Text>();
      }
      else {
        room.GetComponentInChildren<Animator>().SetTrigger("AlreadyOpen");
        oldDisplays.Add(room.GetComponentInChildren<Text>());
      }
    }

    //Start ticking down.
    StartCoroutine(Tick());
  }

  //===================================================================================================================

  private IEnumerator Tick() {

    bool showDisplays = true;

    //Do this every second.
    while(true) {
      //Add a second to current time.
      currentTime += new TimeSpan(0, 0, 1);

      //Have we reached end time? If so, push end time back another year.
      if(currentTime > endTime) {
        //Open the current room's door.
        currentRoom.GetComponentInChildren<Animator>().SetTrigger("Open");

        //Add the display to list of old displays.
        oldDisplays.Add(currentDisplay);

        //Create a new current room ahead of the old current room.
        currentRoom = Instantiate(roomPrefab, 
                                  currentRoom.transform.position + new Vector3(0, 0, 8), 
                                  Quaternion.identity) as GameObject;

        currentDisplay = currentRoom.GetComponentInChildren<Text>();
        
        //Push the timer 1 year ahead.
        endTime = new DateTime(currentTime.Year + 1, 5, 8); 
      }

      //Prep string for display.
      string timeText = endTime.Subtract(currentTime).ToString();
      if(timeText.Contains("."))
        timeText = timeText.Replace(".", ":");
      else
        timeText = "000:" + timeText;

      //Display current time in current display.
      currentDisplay.text = timeText;

      //Flash old displays.
      showDisplays = !showDisplays;
      foreach(Text display in oldDisplays) {
        display.gameObject.SetActive(showDisplays);
      }

      //Wait one second.
      yield return new WaitForSeconds(1);
    }
  }
}