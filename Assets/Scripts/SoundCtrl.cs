using UnityEngine;
using System.Collections;

public class SoundCtrl : MonoBehaviour {
  
  public AudioSource humSound;
  public AudioSource windSound;
  public AudioSource graveSound;


  public AudioSource metalSound;
  public AudioSource snowSound;

  private MyMovement move;

  //===================================================================================================================

  private void Start() {
    move = GameObject.FindWithTag("Player").GetComponent<MyMovement>();
    // lastStep = transform.position;
  }

  //===================================================================================================================

  private void Update() {

    //Walking sounds.
    if(move.IsMoving && metalSound.volume == 0) StartCoroutine(FadeIn(metalSound));
    else if(!move.IsMoving && metalSound.volume == 1) StartCoroutine(FadeOut(metalSound));
  }

  //===================================================================================================================

  private IEnumerator FadeIn(AudioSource aSource){
    float elapsedTime = 0;
    float duration = 0.25f;

    while(elapsedTime < duration) {
      elapsedTime += Time.deltaTime;

      aSource.volume = elapsedTime/duration;

      yield return null;
    }
  }

  //===================================================================================================================

    private IEnumerator FadeOut(AudioSource aSource){
    float elapsedTime = 0;
    float duration = 0.25f;

    while(elapsedTime < duration) {
      elapsedTime += Time.deltaTime;

      aSource.volume = 1 - elapsedTime/duration;

      yield return null;
    }
  }
}
