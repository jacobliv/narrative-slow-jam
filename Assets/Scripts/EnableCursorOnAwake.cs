using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCursorOnAwake : MonoBehaviour {
    public Texture2D arrow;
    private void Start() {
        Cursor.SetCursor(arrow,Vector2.zero,CursorMode.Auto);

    }
}
