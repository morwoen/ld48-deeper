using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiloDoorOpener : MonoBehaviour
{
  public Animation animation;
  private bool once = false;

  private void OnTriggerEnter(Collider other) {
    if (!once) {
      animation.Play();
      once = true;
    }
  }
}
