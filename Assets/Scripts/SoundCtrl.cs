using UnityEngine;
using System.Collections;

public class SoundCtrl : MonoBehaviour {
  
  public AudioSource humSound;    //Ambience sound: hum
  public AudioSource windSound;   //Ambience sound: wind
  public AudioSource howlSound;   //Ambience sound: howl

  public AudioSource metalSound;  //Step sound: metal
  public AudioSource snowSound;   //Step sound: snow

  private float entry = -4;       //Point in the timeline dividing hum and wind, metal and snow.
  private float far = -256;       //Point in the timeline dividing wind and howl.
  private float delta = 32;       //Used between points to smoothly fade between sounds.
  
  private bool isInside = true;   //Are we inside or outside? Metal or snow?

  private MoveCtrl move;          //The movement script, for step sound queues.

  //===================================================================================================================

  private void Start() {
    move = GameObject.FindWithTag("Player").GetComponent<MoveCtrl>();
  }

  //===================================================================================================================

  private void Update() {

    //Current position in the timeline.
    float z = transform.position.z;

    //Ambient sounds.
    humSound.volume = Mathf.Clamp(((z - entry)/delta), 0, 1);
    howlSound.volume = Mathf.Clamp((1-(z - far)/delta), 0, 1);
    if(z >= entry) windSound.volume = Mathf.Clamp((1-(z - entry)/delta), 0, 1);
    else windSound.volume = Mathf.Clamp(((z - far)/delta), 0, 1);

    //Walking sounds.
    isInside = z >= entry;

    metalSound.volume = 0;
    snowSound.volume = 0;

    if(!move.IsMoving) return;
    if(isInside) metalSound.volume = 1;
    else snowSound.volume = 1;
  }
}
