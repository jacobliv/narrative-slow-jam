using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextAreaManager : MonoBehaviour
{
    [SerializeField] bool preWrite;
    [SerializeField] bool midWrite;
    [SerializeField] bool postWrite;

    [SerializeField] TMP_Text dialogue;
    //[SerializeField] DialogueSO Dialogue;

    void Update()
    {
        if (preWrite)
        {
        }

        else if (midWrite)
        {
        }

        else if (postWrite)
        {
        }
    }
}
