using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
  private AudioSource source;
  private GlobalAlertSystem gas;

  [SerializeField]
  private AudioClip sneaking;
  [SerializeField]
  private AudioClip detected;

  private void Start() {
    source = GetComponent<AudioSource>();
    gas = FindObjectOfType<GlobalAlertSystem>();
  }

  private void Update() {
    if (gas?.alertLevel > 0) {
      source.clip = detected;
    } else {
      source.clip = sneaking;
    }
  }
}
