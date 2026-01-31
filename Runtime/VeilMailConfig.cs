using UnityEngine;

namespace VeilMail
{
    /// <summary>
    /// ScriptableObject storing VeilMail configuration.
    /// Create via Edit > Project Settings > VeilMail.
    /// </summary>
    public class VeilMailConfig : ScriptableObject
    {
        private const string ResourcePath = "VeilMailConfig";

        [Tooltip("Your Veil Mail API key")]
        public string apiKey = "";

        [Tooltip("API base URL")]
        public string baseUrl = "https://api.veilmail.xyz";

        [Tooltip("Request timeout in seconds")]
        [Range(5, 120)]
        public int timeoutSeconds = 30;

        [Tooltip("Remove API key from builds (recommended for security)")]
        public bool stripKeyInBuilds = true;

        private static VeilMailConfig _instance;

        /// <summary>Get the singleton config instance from Resources.</summary>
        public static VeilMailConfig Instance
        {
            get
            {
                if (_instance == null)
                    _instance = UnityEngine.Resources.Load<VeilMailConfig>(ResourcePath);
                return _instance;
            }
        }

#if UNITY_EDITOR
        /// <summary>Get or create the config asset (Editor only).</summary>
        public static VeilMailConfig GetOrCreate()
        {
            var instance = Instance;
            if (instance != null) return instance;

            instance = CreateInstance<VeilMailConfig>();
            var dir = "Assets/Resources";
            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);

            UnityEditor.AssetDatabase.CreateAsset(instance, $"{dir}/{ResourcePath}.asset");
            UnityEditor.AssetDatabase.SaveAssets();
            _instance = instance;
            return instance;
        }
#endif
    }
}
