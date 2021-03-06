﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UICtrl : MonoBehaviour {

  private Image bg;
  private Text title, me, error, tutorial;
  private Image star;

  private bool ready;
  private bool click;
  public bool Ready {set{ready = value;}}

  //===================================================================================================================

  private void Start() {
    //Get all those UI components.
    bg = GetComponentInChildren<Image>();
    title = transform.Find("Background/Title").GetComponent<Text>();
    me = transform.Find("Background/Me").GetComponent<Text>();
    star = transform.Find("Background/Star").GetComponent<Image>();
    error = transform.Find("Background/Error").GetComponent<Text>();
    tutorial = transform.Find("Background/Tutorial").GetComponent<Text>();

    //Start the intro sequence.
    StartCoroutine(Intro());
  }

  //===================================================================================================================

  private IEnumerator Intro() {

    yield return new WaitForSeconds(1);

    StartCoroutine(FadeIn(title, 2));
    yield return new WaitForSeconds(3);

    StartCoroutine(FadeIn(me, 0.2f));
    yield return new WaitForSeconds(0.2f);

    //Wait until the game controller says the game is ready.
    float elapsedTime = 0;
    while(!ready) {
      elapsedTime += Time.deltaTime;

      //If we have been waiting 2 extra seconds, show spinning star.
      if(elapsedTime >= 2 && !star.gameObject.activeSelf) {
        star.gameObject.SetActive(true);
        StartCoroutine(FadeIn(star, 1));
        StartCoroutine(RotateStar());
      }

      //If we have been waiting 15 extra seconds, show error message.
      if(elapsedTime >= 15 && !error.gameObject.activeSelf) {
        error.gameObject.SetActive(true);
        StartCoroutine(FadeIn(error, 0.1f));
      }

      yield return null;
    }

    if(star.gameObject.activeSelf) StartCoroutine(FadeIn(star, 2, true));

    //Prompt player to click and start.
    if(error.gameObject.activeSelf) {
      // error.gameObject.SetActive(true);
      StartCoroutine(FadeIn(error, 0.2f, true));
      yield return new WaitForSeconds(0.2f);
      error.text = "Click.";
      StartCoroutine(FadeIn(error, 0.2f));
    }
    else {
      error.gameObject.SetActive(true);
      error.text = "Click.";
      StartCoroutine(FadeIn(error, 0.2f));
    }
    
    InputCtrl.mouseClick += setClick;
    while(!click) {yield return null;}
    InputCtrl.mouseClick -= setClick;

    StartCoroutine(FadeIn(title, 2, true));
    StartCoroutine(FadeIn(me, 2, true));
    StartCoroutine(FadeIn(error, 2, true));

    yield return new WaitForSeconds(2);

    StartCoroutine(FadeIn(bg, 2, true));
    yield return new WaitForSeconds(2.5f);

    //Give control to the player.
    GameObject.FindWithTag("Player").GetComponent<MoveCtrl>().enabled = true;
    GameObject.FindWithTag("GameController").GetComponent<GameCtrl>().enablePause();

    StartCoroutine(FadeIn(tutorial, 2));
    yield return new WaitForSeconds(5);

    StartCoroutine(FadeIn(tutorial, 2, true));
    yield return new WaitForSeconds(3);

    //Disable UI.
    gameObject.SetActive(false);
  }

  //===================================================================================================================

  private IEnumerator FadeIn<T>(T t, float duration, bool reversed=false) where T : MaskableGraphic{
    //Fades in (or out) the current UI component t.
    float elapsedTime = 0;
    while(elapsedTime < duration) {
      elapsedTime += Time.deltaTime;
      t.SetAlpha(reversed ? 1 - elapsedTime/duration : elapsedTime/duration);
      yield return null;
    }
  }

  //===================================================================================================================

  private IEnumerator RotateStar() {
    float starRot = 0;
    while(true) {
      starRot = (starRot + Time.deltaTime * 4) % 360;
      star.transform.Rotate(Vector3.forward * starRot);
      yield return null;
    }
  }


  //===================================================================================================================

  private void setClick() {
    click = true;
    GameObject.FindWithTag("GameController").GetComponent<GameCtrl>().initialLock();
  }
}

//=====================================================================================================================

//EXTENSION, outside UICtrl.

public static class Extensions {
  //Extension used for Image and Text to set their color's alpha component easily.
  public static void SetAlpha<T>(this T t, float a) where T : MaskableGraphic {
    Color c = t.color;
    c.a = a;
    t.color = c;
  }
}