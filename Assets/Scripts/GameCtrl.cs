using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameCtrl : MonoBehaviour {

  private IEnumerator Start() {

    //GET the current time from the internet.
    WWW www = new WWW("http://www.timeapi.org/utc/now");

    //Wait for the request to be completed.
    yield return www;

    //Prepare received data.
    string currentDate = www.text.Substring(0, www.text.IndexOf("+"));

    //Set up current time and end time.
    DateTime currentTime = DateTime.Parse(currentDate);
    // currentTime = new DateTime(2016, 5, 7, 23, 59, 55);
    DateTime startTime = new DateTime(1991, 5, 8, 0, 0, 0);

    //Get the room prefab.
    GameObject roomPrefab = Resources.Load("Room", typeof(GameObject)) as GameObject;

    //Build the first room.
    GameObject r = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity) as GameObject;
    RoomCtrl rCtrl = r.GetComponent<RoomCtrl>();
    rCtrl.setEndTime(startTime.AddYears(1), currentTime);
    rCtrl.setName(0);

    //Pass the current time to the time controller, it'll take it from here.
    GetComponent<TimeCtrl>().setCurrentTime(currentTime);
  }
}