using UnityEditor;
using UnityEngine;

namespace VeilMail.Editor
{
    public class VeilMailSettingsProvider : SettingsProvider
    {
        private SerializedObject _serializedConfig;
        private bool _showApiKey;

        public VeilMailSettingsProvider()
            : base("Project/VeilMail", SettingsScope.Project) { }

        public override void OnGUI(string searchContext)
        {
            var config = VeilMailConfig.GetOrCreate();

            if (_serializedConfig == null || _serializedConfig.targetObject != config)
                _serializedConfig = new SerializedObject(config);

            _serializedConfig.Update();

            EditorGUILayout.Space(10);

            // Security warning
            EditorGUILayout.HelpBox(
                "API keys are sensitive. Never ship builds with API keys embedded. " +
                "Enable 'Strip API Key in Builds' to automatically remove the key from build artifacts.",
                MessageType.Warning);

            EditorGUILayout.Space(10);

            // API Key
            var apiKeyProp = _serializedConfig.FindProperty("apiKey");
            EditorGUILayout.BeginHorizontal();
            if (_showApiKey)
                apiKeyProp.stringValue = EditorGUILayout.TextField("API Key", apiKeyProp.stringValue);
            else
                apiKeyProp.stringValue = EditorGUILayout.PasswordField("API Key", apiKeyProp.stringValue);
            if (GUILayout.Button(_showApiKey ? "Hide" : "Show", GUILayout.Width(50)))
                _showApiKey = !_showApiKey;
            EditorGUILayout.EndHorizontal();

            // Base URL
            EditorGUILayout.PropertyField(_serializedConfig.FindProperty("baseUrl"));

            // Timeout
            EditorGUILayout.PropertyField(_serializedConfig.FindProperty("timeoutSeconds"));

            // Strip key
            EditorGUILayout.PropertyField(_serializedConfig.FindProperty("stripKeyInBuilds"),
                new GUIContent("Strip API Key in Builds"));

            EditorGUILayout.Space(10);

            // Test Connection
            if (GUILayout.Button("Test Connection"))
            {
                TestConnection(config);
            }

            _serializedConfig.ApplyModifiedProperties();
        }

        private async void TestConnection(VeilMailConfig config)
        {
            if (string.IsNullOrEmpty(config.apiKey))
            {
                EditorUtility.DisplayDialog("VeilMail", "Please enter an API key first.", "OK");
                return;
            }

            try
            {
                var client = new VeilMailClient(config);
                await client.Domains.ListAsync();
                EditorUtility.DisplayDialog("VeilMail", "Connection successful!", "OK");
            }
            catch (System.Exception ex)
            {
                EditorUtility.DisplayDialog("VeilMail", $"Connection failed: {ex.Message}", "OK");
            }
        }

        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            return new VeilMailSettingsProvider
            {
                keywords = new[] { "VeilMail", "Email", "API" }
            };
        }
    }
}
