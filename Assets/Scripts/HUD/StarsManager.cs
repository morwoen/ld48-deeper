using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsManager : MonoBehaviour
{
  private List<Image> stars;

  void Start() {
    stars = new List<Image>();

    for (int i = 0; i < transform.childCount; i++) {
      stars.Add(transform.GetChild(i).GetComponent<Image>());
    }
  }

  public void UpdateStars(int alertLevel) {
    for (var i = 0; i < stars.Count; i++) {
      var c = stars[i].color;
      if (i < alertLevel) {
        c.a = 1;
      } else {
        c.a = 0.1f;
      }
      stars[i].color = c;
    }
  }
}
