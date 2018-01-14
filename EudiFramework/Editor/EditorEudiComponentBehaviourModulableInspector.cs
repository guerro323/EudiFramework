using System.Collections.Generic;
using UnityEditor;

namespace EudiFramework.Editor
{
    [CustomEditor (typeof (EudiComponentBehaviourModulable))]
    public class EditorEudiComponentBehaviourModulableInspector : UnityEditor.Editor
    {
        void OnInspectorGUI ()
        {
            /*var script = (EudiComponentBehaviourModulable)target;
            new PropertyDrawer().
            EditorGUILayout.PropertyField(serializedObject.FindProperty("AttachedModules"), true);
            //script.AttachedModules = EditorGUILayout.ObjectField ("Modules", script.AttachedModules, typeof (List<IModule>));*/
            DrawDefaultInspector ();
        }
    }
}