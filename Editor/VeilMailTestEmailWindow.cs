using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VeilMail.Editor
{
    public class VeilMailTestEmailWindow : EditorWindow
    {
        private string _from = "";
        private string _to = "";
        private string _subject = "";
        private string _body = "<h1>Test Email</h1><p>Sent from Unity Editor.</p>";
        private string _status = "";
        private bool _sending;

        [MenuItem("Tools/VeilMail/Send Test Email")]
        public static void ShowWindow()
        {
            var window = GetWindow<VeilMailTestEmailWindow>("VeilMail Test Email");
            window.minSize = new Vector2(400, 350);
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Send Test Email", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            _from = EditorGUILayout.TextField("From", _from);
            _to = EditorGUILayout.TextField("To", _to);
            _subject = EditorGUILayout.TextField("Subject", _subject);

            EditorGUILayout.LabelField("HTML Body");
            _body = EditorGUILayout.TextArea(_body, GUILayout.Height(100));

            EditorGUILayout.Space(10);

            EditorGUI.BeginDisabledGroup(_sending);
            if (GUILayout.Button(_sending ? "Sending..." : "Send Email", GUILayout.Height(30)))
            {
                SendTestEmail();
            }
            EditorGUI.EndDisabledGroup();

            if (!string.IsNullOrEmpty(_status))
            {
                EditorGUILayout.Space(5);
                EditorGUILayout.HelpBox(_status, _status.StartsWith("Error") ? MessageType.Error : MessageType.Info);
            }
        }

        private async void SendTestEmail()
        {
            var config = VeilMailConfig.Instance;
            if (config == null || string.IsNullOrEmpty(config.apiKey))
            {
                _status = "Error: Configure your API key in Project Settings > VeilMail";
                return;
            }

            _sending = true;
            _status = "Sending...";
            Repaint();

            try
            {
                var client = new VeilMailClient(config);
                var result = await client.Emails.SendAsync(new Dictionary<string, object>
                {
                    ["from"] = _from,
                    ["to"] = _to,
                    ["subject"] = _subject,
                    ["html"] = _body,
                });

                var id = result.ContainsKey("id") ? result["id"]?.ToString() : "unknown";
                _status = $"Email sent successfully! ID: {id}";
            }
            catch (System.Exception ex)
            {
                _status = $"Error: {ex.Message}";
            }

            _sending = false;
            Repaint();
        }
    }
}
