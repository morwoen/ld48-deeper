using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  [SerializeField]
  private GameObject mainMenu;
  [SerializeField]
  private GameObject controlsMenu;

  private bool onMain = true;

  public void Play() {
    SceneManager.LoadScene(1);
  }

  public void ToggleControlsMenu() {
    mainMenu.SetActive(!onMain);
    controlsMenu.SetActive(onMain);
    onMain = !onMain;
  }

  public void Quit() {
    Application.Quit();
  }
}
