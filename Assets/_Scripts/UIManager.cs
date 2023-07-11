using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject phoneScreen, TextArea;

    void Start()
    {
        TextArea.SetActive(true);
    }

    void Update()
    {
        if (phoneScreen.activeSelf)
        {
            TextArea.SetActive(false);
        }

        else
        {
            TextArea.SetActive(true);
        }
    }
}
