using UnityEngine;
using System.Collections;

public class GameCtrl : MonoBehaviour {

  [SerializeField] private GameObject roomPrefab;
  private bool isPaused = false;
  private bool focusDisabled = true;
  private MoveCtrl move;
  private UnityStandardAssets.ImageEffects.BlurOptimized blur;

  private bool asyncOngoing = false;

  private const string URL = "/time";

  //===================================================================================================================

  private IEnumerator Start() {

    //Try to GET the current time.
    WWW www = new WWW(URL);
    yield return www;

    //If get fails, try again every 3 seconds.
    while(!string.IsNullOrEmpty(www.error) || !testString(www.text)) {
      yield return new WaitForSeconds(3);
      www = new WWW(URL);
      yield return www;
    }

    //Get some components.
    GameObject g = GameObject.FindWithTag("Player");
    move = g.GetComponent<MoveCtrl>();
    blur = g.GetComponentInChildren<UnityStandardAssets.ImageEffects.BlurOptimized>();

    // Prepare received data.
    string currentDate = www.text.Substring(0, www.text.IndexOf("+"));

    //Set up current time and end time.
    System.DateTime currentTime = System.DateTime.Parse(currentDate);
    System.DateTime startTime = new System.DateTime(1991, 5, 8, 15, 15, 0);

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

  private IEnumerator updateTime(){
    WWW www = new WWW(URL);
    yield return www;

    //If the string is valid.
    if(string.IsNullOrEmpty(www.error) && testString(www.text)) {
      string currentDate = www.text.Substring(0, www.text.IndexOf("+"));
      System.DateTime currentTime = System.DateTime.Parse(currentDate);
      GetComponent<TimeCtrl>().setCurrentTime(currentTime);
    }
  }

  //===================================================================================================================

  private bool testString(string s) {
    System.Text.RegularExpressions.Regex rgx = 
      new System.Text.RegularExpressions.Regex(@"^\d\d\d\d-\d\d-\d\dT\d\d:\d\d:\d\d\+00:00$");
    return rgx.IsMatch(s);
  }

  //===================================================================================================================

  public void enablePause() {
    focusDisabled = false;
    InputCtrl.mouseClick += pause;
    
    StartCoroutine(pauseCheck());
  }

  //===================================================================================================================

  public void initialLock() {
    StartCoroutine("asyncCursorLock", CursorLockMode.Locked);
  }

  //===================================================================================================================

  private void pause() {
    isPaused = !isPaused;
    StopCoroutine("smoothPause");
    StartCoroutine("smoothPause", isPaused);
  }

  //===================================================================================================================

  private void OnApplicationFocus(bool onFocus) {
    if(focusDisabled) return;

    //If focus is regained, update time if possible.
    if(onFocus) StartCoroutine(updateTime());

    //If we are not paused but focus is lost, pause. 
    else if(!isPaused) pause();

  }

  //===================================================================================================================

  private IEnumerator pauseCheck() {
    while(true) {
      //If cursor is free, pause.
      if(!asyncOngoing && !isPaused && Cursor.lockState != CursorLockMode.Locked)
        pause();
      yield return null;
    }
  }

  //===================================================================================================================

  private IEnumerator smoothPause(bool paused=false) {
    // isPaused = paused;
    float initialValue = blur.blurSize;
    float initialVolValue = AudioListener.volume;
    float finalValue = paused ? 3 : 0;
    float finalVolValue = paused ? 0 : 1;
    float elapsedTime = 0;
    float duration = 0.5f;

    if(paused) blur.enabled = true;

    move.enabled = !paused;
    if(!asyncOngoing) StartCoroutine("asyncCursorLock", paused ? CursorLockMode.None : CursorLockMode.Locked);

    while(elapsedTime < duration) {
      elapsedTime += Time.deltaTime;
      float t = elapsedTime/duration;
      blur.blurSize = Mathf.Lerp(initialValue, finalValue, t);
      AudioListener.volume = Mathf.Lerp(initialVolValue, finalVolValue, t);
      yield return null;
    }

    if(!paused) blur.enabled = false;
  }

  //===================================================================================================================

  private IEnumerator asyncCursorLock(CursorLockMode desiredMode) {
    asyncOngoing = true;

    Cursor.lockState = desiredMode;

    //Give a little time for the cursor to change lock state.
    while(Cursor.lockState != desiredMode) yield return null;

    //Change the visibility of the cursor based on the lock state.
    Cursor.visible = !(Cursor.lockState == CursorLockMode.Locked);

    asyncOngoing = false;
  }
}