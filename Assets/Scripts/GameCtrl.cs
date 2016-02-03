using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class GameCtrl : MonoBehaviour {

  private bool isPaused = false;
  private MoveCtrl move;
  private UnityStandardAssets.ImageEffects.BlurOptimized blur;
  // private AudioListener audio;

  //===================================================================================================================

  private IEnumerator Start() {

    WWW www;

    //Try to GET the current time from the internet.
    do {
      www = new WWW("http://www.timeapi.org/utc/now");
      yield return www;
    } while(!string.IsNullOrEmpty(www.error) && !testString(www.text));

    //Get some components.
    GameObject g = GameObject.FindWithTag("Player");
    move = g.GetComponent<MoveCtrl>();
    blur = g.GetComponentInChildren<UnityStandardAssets.ImageEffects.BlurOptimized>();
    // audio = g.GetComponentInChildren<AudioListener>();

    // Prepare received data.
    string currentDate = www.text.Substring(0, www.text.IndexOf("+"));

    //Set up current time and end time.
    DateTime currentTime = DateTime.Parse(currentDate);
    // currentTime = new DateTime(2018, 1, 20, 23, 30, 55);
    DateTime startTime = new DateTime(1991, 5, 8, 15, 15, 0);

    //Get the room prefab.
    GameObject roomPrefab = Resources.Load("Room", typeof(GameObject)) as GameObject;

    //Build the first room.
    GameObject r = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity) as GameObject;
    RoomCtrl rCtrl = r.GetComponent<RoomCtrl>();
    rCtrl.setEndTime(startTime.AddYears(1), currentTime);
    rCtrl.setName(0);

    //Pass the current time to the time controller, it'll take it from here.
    GetComponent<TimeCtrl>().setCurrentTime(currentTime);

    //Tell the UI that we are ready, we can start the game.
    GameObject.FindWithTag("UI").GetComponent<UICtrl>().Ready = true;
  }

  //===================================================================================================================

  private bool testString(string s) {
    Regex rgx = new Regex(@"^\d\d\d\d-\d\d-\d\dT\d\d:\d\d:\d\d\+00:00$");
    return rgx.IsMatch(s);
  }

  //===================================================================================================================

  public void enablePause() {
    InputCtrl.cancelPressed += pause;
    InputCtrl.mouseClick += clickUnpause;
  }

  //===================================================================================================================

  private void pause() {
    isPaused = !isPaused;

    StartCoroutine(smoothPause(isPaused));
  }

  //===================================================================================================================

  private void clickUnpause() {
    if(isPaused) pause();
  }

  //===================================================================================================================

  private IEnumerator smoothPause(bool paused=false) {
    float initialValue = blur.blurSize;
    float initialVolValue = AudioListener.volume;
    float finalValue = paused ? 3 : 0;
    float finalVolValue = paused ? 0 : 1;
    float elapsedTime = 0;
    float duration = 0.5f;

    if(paused) blur.enabled = true;

    move.enabled = !paused;
    Cursor.visible = paused;  
    Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;

    while(elapsedTime < duration) {
      elapsedTime += Time.deltaTime;
      blur.blurSize = Mathf.Lerp(initialValue, finalValue, elapsedTime/duration);
      AudioListener.volume = Mathf.Lerp(initialVolValue, finalVolValue, elapsedTime/duration);
      yield return null;
    }

    if(!paused) blur.enabled = false;
  }


}