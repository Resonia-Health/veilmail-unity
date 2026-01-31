using UnityEditor;
using UnityEngine;

namespace VeilMail.Editor
{
    [CustomEditor(typeof(VeilMailConfig))]
    public class VeilMailSettingsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox(
                "Configure VeilMail settings via Edit > Project Settings > VeilMail for the best experience.",
                MessageType.Info);

            EditorGUILayout.Space(5);
            DrawDefaultInspector();
        }
    }
}
