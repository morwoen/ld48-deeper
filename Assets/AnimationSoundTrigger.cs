using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSoundTrigger : MonoBehaviour
{
  [SerializeField]
  private AudioSource source;
  [SerializeField]
  private AudioClip[] steps;

  public void FootStep() {
    source.PlayOneShot(steps[Random.Range(0, steps.Length)]);
  }
}
