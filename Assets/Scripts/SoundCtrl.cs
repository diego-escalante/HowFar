using UnityEngine;
using System.Collections;

public class SoundCtrl : MonoBehaviour {
  
  public AudioSource humSound;         //Ambience sound: hum
  public AudioSource windSound;        //Ambience sound: wind
  public AudioSource howlSound;        //Ambience sound: howl
  private AudioSource[] ambientSounds; //Collection of all three ambient sounds.

  public AudioSource metalSound;       //Step sound: metal
  public AudioSource snowSound;        //Step sound: snow

  private float entry = -4;            //Point in the timeline dividing hum and wind, metal and snow.
  private float far = -256;            //Point in the timeline dividing wind and howl.
  private float delta = 32;            //Used between points to smoothly fade between sounds.
  
  private bool isInside = true;        //Are we inside or outside? Metal or snow?

  private MoveCtrl move;               //The movement script, for step sound queues.

  //===================================================================================================================

  private void Start() {
    StartCoroutine(FadeInSound());
    move = GameObject.FindWithTag("Player").GetComponent<MoveCtrl>();
    ambientSounds = new AudioSource[] {humSound, windSound, howlSound};
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

    foreach(AudioSource ambientSound in ambientSounds){
      if(ambientSound.volume != 0 && !ambientSound.isPlaying) ambientSound.Play();
      else if(ambientSound.volume == 0 && ambientSound.isPlaying) ambientSound.Pause();
    }

    //Walking sounds.
    isInside = z >= entry;

    metalSound.volume = 0;
    snowSound.volume = 0;

    if(!move.IsMoving) return;
    if(isInside) metalSound.volume = 1;
    else snowSound.volume = 1;
  }

  //===================================================================================================================

  private IEnumerator FadeInSound(){
    float elapsedTime = 0;
    float duration = 5;

    while(elapsedTime < duration) {
      elapsedTime += Time.deltaTime;
      AudioListener.volume = Mathf.Lerp(0, 1, elapsedTime/duration);
      yield return null;
    }
  }
}
