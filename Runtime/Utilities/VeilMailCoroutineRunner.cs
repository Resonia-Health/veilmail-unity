using UnityEngine;

namespace VeilMail.Utilities
{
    /// <summary>
    /// Hidden MonoBehaviour used to run coroutines from non-MonoBehaviour code.
    /// </summary>
    [AddComponentMenu("")]
    public class VeilMailCoroutineRunner : MonoBehaviour
    {
        private static VeilMailCoroutineRunner _instance;

        public static VeilMailCoroutineRunner Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("[VeilMail CoroutineRunner]");
                    go.hideFlags = HideFlags.HideAndDontSave;
                    DontDestroyOnLoad(go);
                    _instance = go.AddComponent<VeilMailCoroutineRunner>();
                }
                return _instance;
            }
        }
    }
}
