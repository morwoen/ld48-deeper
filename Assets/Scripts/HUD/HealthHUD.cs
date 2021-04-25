using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour
{
  private Slider slider;

  void Awake()
  {
    slider = GetComponentInChildren<Slider>();
  }

  public void SetHealth(float hpPercentage) {
    slider.value = hpPercentage;
  }
}
