using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(E1CubeMovementOnStart))]
public class OnStartEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.TextArea(@"`Cubes` variable indicate how much cubes will be instantiated.
`HeavyRotationIteration` variable indicate how much iteration,
that will be done per cube each frame to rotate them.
Work time = `HeavyRotationIteration` * `Cubes`.

NOTES:
-- Having 10000 cubes and 1000 iterations would already kill unity,
in the standard behaviour (single threaded).

-- The Eudi behaviour would still have perfomance impact, 
but only for the rotation system. 
(Rotation system don't have the same thread as position system)");
        EditorGUILayout.EndVertical();
    }
}