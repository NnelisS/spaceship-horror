using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AIPathCreator))]
public class AIPathEditor : Editor
{
    AIPathCreator creator;
    AIPath path;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Reset Path")) {
            creator.CreatePath();
            path = creator.path;
            SceneView.RepaintAll();
        }

        if (GUILayout.Button("Toggle closed")) {
            path.ToggleClosed();
            SceneView.RepaintAll();
        }

        if(GUILayout.Button("Bake points")) {
           // creator.ResetY();
            creator.UpdatePoints();
            SceneView.RepaintAll();
        }

        if (GUILayout.Button("Reset Y")) {
            creator.ResetY();
            SceneView.RepaintAll();
        }

        if (GUILayout.Button("Lock Path")) {
            creator.LockPath();
            SceneView.RepaintAll();
        }
    }

    void OnSceneGUI()
    {
        Draw();
        Input();
    }

    void Input()
    {
        Event guiEvent = Event.current;
        Vector3 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

        if(guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift) {
            Undo.RecordObject(creator, "Add Point");
            path.AddPoint(mousePos);
        }
    }


    void Draw()
    {
        if(path.lockPath) { Handles.color = Color.red; }
        else Handles.color = Color.white; ;

        if (!path.isClosed) {
            for (int i = 0; i < path.numPoints - 1; i++) {
                Handles.DrawLine(path[i], path[i + 1], 4);
            }
        }
        else {
            for (int i = 0; i < path.numPoints; i++) {
                Handles.DrawLine(path[i], path[i + 1], 4);
            }
        }

        Handles.DrawWireCube(path.centroid, Vector3.one);

        Handles.color = Color.red;
        for (int i = 0; i < path.numPoints; i++) {
            Vector3 newPos = Handles.FreeMoveHandle(path[i], Quaternion.identity, .5f, Vector2.zero, Handles.CylinderHandleCap);

            if(path[i] != newPos) {
                Undo.RecordObject(creator, "Move Point");
                path.MovePoint(i, newPos);
            }
        }
    }


    void OnEnable()
    {
        creator = (AIPathCreator)target;
        if (creator.path == null) {
            creator.CreatePath();
        }
        path = creator.path;
    }

}