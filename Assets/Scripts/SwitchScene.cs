using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchScene : MonoBehaviour {
    public Button button;
    public string  scene;
    void Start() {
        Debug.Log("Starting");
        Debug.Log(button.name);
        button.onClick.AddListener(Call);
    }

    private void Call() {
        Debug.Log("CLICK!");
        SceneManager.LoadScene(scene);
    }
}
