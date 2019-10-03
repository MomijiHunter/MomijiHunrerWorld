using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace aojilu
{
    
    [CustomEditor(typeof(MapCreator))]
    public class MapEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            MapCreator myScript = (MapCreator)target;
            if(GUILayout.Button("create Map"))
            {
                myScript.CreatMap();
            }

            if(GUILayout.Button("Destroy map"))
            {
                myScript.DestroyMap();
            }
        }
    }
}
