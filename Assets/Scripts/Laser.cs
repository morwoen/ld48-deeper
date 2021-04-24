using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
  private LineRenderer lazor;

  [SerializeField]
  private Material material;
  [SerializeField]
  private GameObject beamEnd;

  void Start() {
    lazor = gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
    lazor.startWidth = 0.02f;
    lazor.endWidth = 0.02f;
    lazor.material = material;
    beamEnd.SetActive(false);
  }

  void Update() {
    lazor.SetPosition(0, transform.position);

    RaycastHit hit;
    if (Physics.Raycast(transform.position, transform.up, out hit)) {
      if (hit.collider) {
        lazor.SetPosition(1, hit.point);
        beamEnd.transform.position = hit.point;
        beamEnd.SetActive(true);
      }
    } else {
      lazor.SetPosition(1, transform.up * 5000);
      beamEnd.SetActive(false);
    }
  }
}
