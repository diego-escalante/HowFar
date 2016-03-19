using UnityEngine;
using System.Collections;

public class GameCtrl : MonoBehaviour {

  [SerializeField] private GameObject roomPrefab;
  private bool isPaused = false;
  private MoveCtrl move;
  private UnityStandardAssets.ImageEffects.BlurOptimized blur;

  //===================================================================================================================

  private IEnumerator Start() {

    WWW www;

    // yield return new WaitForSeconds(30);

    // Try to GET the current time from the internet.
    do {
      www = new WWW("heytherediego.com/time");
      yield return www;
    } while(!string.IsNullOrEmpty(www.error) && !testString(www.text));

    //Get some components.
    GameObject g = GameObject.FindWithTag("Player");
    move = g.GetComponent<MoveCtrl>();
    blur = g.GetComponentInChildren<UnityStandardAssets.ImageEffects.BlurOptimized>();

    // Prepare received data.
    string currentDate = www.text.Substring(0, www.text.IndexOf("+"));

    //Set up current time and end time.
    System.DateTime currentTime = System.DateTime.Parse(currentDate);
    // DateTime currentTime = new DateTime(2016, 5, 8, 15, 14, 40);
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

  private bool testString(string s) {
    System.Text.RegularExpressions.Regex rgx = 
      new System.Text.RegularExpressions.Regex(@"^\d\d\d\d-\d\d-\d\dT\d\d:\d\d:\d\d\+00:00$");
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
      float t = elapsedTime/duration;
      blur.blurSize = Mathf.Lerp(initialValue, finalValue, t);
      AudioListener.volume = Mathf.Lerp(initialVolValue, finalVolValue, t);
      yield return null;
    }

    if(!paused) blur.enabled = false;
  }
}