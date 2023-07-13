using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CatAppManager : MonoBehaviour
{
    [SerializeField] CatScriptableObject cat;
    [SerializeField] Image catImage;
    [SerializeField] TMP_Text catNameDisplay, catAgeDisplay;

    void Start()
    {
        catNameDisplay.text = cat.catName;
        catAgeDisplay.text = cat.catAge.ToString();
        catImage.sprite = cat.catImage;
    }
}
