using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace Lazy_Tool.Editor
{
    [InitializeOnLoad]
    public class KeystoreLoader
    {
        static KeystoreLoader()
        {
            PlayerSettings.Android.keystorePass = PlayerPrefs.GetString(CustomWindow.KEYSTORE_PASSWORD);
            PlayerSettings.Android.keyaliasName = PlayerPrefs.GetString(CustomWindow.KEY_ALIAS_NAME);
            PlayerSettings.Android.keyaliasPass = PlayerPrefs.GetString(CustomWindow.KEY_ALIAS_PASSWORD);
        }
    }
}

#endif