using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchBackground : MonoBehaviour {
    public GameObject       background;
    public List<GameObject> toDisable;
    public GameObject       newBackground;
    public List<GameObject> toEnable;

    public void RunSwitchBackground() {
        toDisable.ForEach((g)=>g.SetActive(false));
        background.SetActive(false);
        newBackground.SetActive(true);
        toEnable.ForEach((g)=>g.SetActive(true));

    }
}
