using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAlertSystem : MonoBehaviour
{
  public int alertLevel
  {
    get;
    private set;
  } = 5;

  [SerializeField]
  private int maxAlertLevel = 5;
  private int minAlertLevel = 1;

  [SerializeField]
  private float baseTimer = 5f;
  private float cooldown = 0f;

  void Update() {
    if (cooldown > 0) {
      cooldown -= Time.deltaTime;
    }

    if (cooldown <= 0 && alertLevel > minAlertLevel) {
      alertLevel -= 1;
      cooldown = baseTimer * alertLevel;
      Debug.Log("AlertLevelDown " + alertLevel);
    }
  }

  public void IncreaseAlertLevel() {
    if (alertLevel < maxAlertLevel) {
      alertLevel += 1;
    }
    cooldown = baseTimer * alertLevel;
    Debug.Log("AlertLevelUp " + alertLevel);
  }
}
