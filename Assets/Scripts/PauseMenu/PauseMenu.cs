using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
  public static bool GamePaused = false;
  public PlayerInput inputManager;
  private GameObject menu;

  void Awake() {
    menu = gameObject.transform.GetChild(0).gameObject;

    inputManager = new PlayerInput();
    var action = inputManager.UI.Pause;

    action.performed += ButtonPress;
  }

  void OnEnable() {
    inputManager.UI.Enable();
  }

  void OnDisable() {
    inputManager.UI.Disable();
  }

  void ButtonPress(InputAction.CallbackContext ctx) {
    if (GamePaused) {
      Resume();
    } else {
      Pause();
    }
  }

  public void Pause() {
    menu.SetActive(true);
    Time.timeScale = 0f;
    GamePaused = true;
  }

  public void Resume() {
    menu.SetActive(false);
    Time.timeScale = 1f;
    GamePaused = false;
  }

  public void Menu() {
    Time.timeScale = 1f;
    SceneManager.LoadScene(0);
  }

  public void Quit() {
    Application.Quit();
  }
}
