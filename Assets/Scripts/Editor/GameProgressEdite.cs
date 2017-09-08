using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using GameProgressManager;
[CustomEditor(typeof(GameProgressSystem))]
public class GameProgressEdite : Editor {

    GameProgressSystem gp;
    private void OnEnable()
    {
        gp = target as GameProgressSystem;

    }

    public override void OnInspectorGUI()
    {
        if (!EditorGUIUtility.editingTextField)
        {
            gp.Sort();
        }
      
        base.OnInspectorGUI();
    }
}
