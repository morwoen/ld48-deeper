using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
  [SerializeField]
  private float maxHealth = 100f;
  [SerializeField]
  private float startingHealth = 100f;
  [SerializeField]
  private float regenPerSecond = 5f;
  [SerializeField]
  private float regenDelay = 5f;

  private bool isRegenerating = false;
  private IEnumerator regen;
  private HealthHUD hphud;

  public float Health
  {
    get;
    private set;
  }

  private void Start() {
    Health = startingHealth;
    hphud = FindObjectOfType<HealthHUD>();
    UpdateUI();
  }

  private void Update() {
    if (isRegenerating) {
      Health += regenPerSecond * Time.deltaTime;
      if (Health > maxHealth) {
        Health = maxHealth;
        isRegenerating = false;
      }
      UpdateUI();
    }
  }

  public bool Hit(float damage) {
    Health -= damage;
    isRegenerating = false;

    if (regen != null) {
      StopCoroutine(regen);
      regen = null;
    }

    if (Health <= 0) {
      Health = 0;
    } else {
      regen = Regen();
      StartCoroutine(regen);
    }

    UpdateUI();
    return Health <= 0;
  }

  public void Respawn() {
    if (regen != null) {
      StopCoroutine(regen);
      regen = null;
    }

    Health = maxHealth;
    UpdateUI();
  }

  IEnumerator Regen() {
    yield return new WaitForSeconds(regenDelay);
    isRegenerating = true;
    regen = null;
  }

  void UpdateUI() {
    hphud?.SetHealth(Health / maxHealth);
  }
}
