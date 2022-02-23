using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AIFov))]
public class AIFovEditor : Editor
{
    AIFov fov;

    void OnSceneGUI()
    {
        Draw();
    }

    void Draw()
    {
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);

        Handles.color = Color.red;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.innerRadius);

        Handles.color = Color.yellow;
        Vector3 viewAngleA = fov.DirFromAngle(-fov.angle / 2);
        Vector3 viewAngleB = fov.DirFromAngle(fov.angle / 2);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.radius);

    }

    void OnEnable()
    {
        fov = (AIFov)target;
    }

}
