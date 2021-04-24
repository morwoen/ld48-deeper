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
  [SerializeField]
  private float movementSpeed = 1;
  [SerializeField]
  private Transform[] lookAtLocations;

  private Vector3 currentLookLocation;
  private int nextLookLocation;

  void Start() {
    lazor = gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
    lazor.startWidth = 0.02f;
    lazor.endWidth = 0.02f;
    lazor.material = material;
    beamEnd.SetActive(false);
    currentLookLocation = transform.up;
    nextLookLocation = 0;
  }

  void Update() {
    lazor.SetPosition(0, transform.position + transform.up * 0.05f);

    if (lookAtLocations.Length > 0) {
      var currentTargetLocation = lookAtLocations[nextLookLocation];

      if (Vector3.Distance(currentTargetLocation.position, currentLookLocation) < 0.01) {
        nextLookLocation = (nextLookLocation + 1) % lookAtLocations.Length;
      }

      currentLookLocation = Vector3.MoveTowards(currentLookLocation, currentTargetLocation.position, movementSpeed * Time.deltaTime);
    }

    RaycastHit hit;
    if (Physics.Raycast(transform.position, currentLookLocation, out hit)) {
      if (hit.collider) {
        lazor.SetPosition(1, hit.point);
        beamEnd.transform.position = hit.point;
        beamEnd.SetActive(true);

        if (hit.collider.CompareTag("Player")) {
          // TODO: do stuff
        }
      }
    } else {
      lazor.SetPosition(1, transform.up * 5000);
      beamEnd.SetActive(false);
    }
  }
}
