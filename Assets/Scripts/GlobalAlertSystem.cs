using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAlertSystem : MonoBehaviour
{
  public int alertLevel
  {
    get;
    private set;
  } = 0;

  [SerializeField]
  private int maxAlertLevel = 5;
  private int minAlertLevel = 0;

  [SerializeField]
  private float baseTimer = 5f;
  private float cooldown = 0f;

  private StarsManager stars;

  void Start() {
    stars = FindObjectOfType<StarsManager>();
    stars?.UpdateStars(alertLevel);
  }

  void Update() {
    if (cooldown > 0) {
      cooldown -= Time.deltaTime;
    }

    if (cooldown <= 0 && alertLevel > minAlertLevel) {
      alertLevel -= 1;
      cooldown = baseTimer * alertLevel;
      stars?.UpdateStars(alertLevel);
      Debug.Log("AlertLevelDown " + alertLevel);
    }
  }

  public void IncreaseAlertLevel() {
    if (alertLevel < maxAlertLevel) {
      alertLevel += 1;
      stars?.UpdateStars(alertLevel);
    }
    cooldown = baseTimer * alertLevel;
    Debug.Log("AlertLevelUp " + alertLevel);
  }
}
