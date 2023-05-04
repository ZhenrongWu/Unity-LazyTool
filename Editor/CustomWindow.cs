using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Lazy_Tool.Editor
{
    public class CustomWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset visualTreeAsset;

        private VisualElement _folderVisualElement;
        private VisualElement _buildVisualElement;
        private ToolbarButton _folderToolbarButton;
        private ToolbarButton _buildToolbarButton;

        private Button _createFoldersButton;
        private Button _organizingFilesButton;

        private DropdownField _platformDropdownField;
        private VisualElement _pcVisualElement;
        private VisualElement _androidVisualElement;
        private Toggle        _pcVersionToggle;
        private Button        _pcIncrementButton;
        private Toggle        _androidVersionToggle;
        private Toggle        _bundleVersionCodeToggle;
        private Button        _androidIncrementButton;
        private TextField     _keystorePasswordTextField;
        private TextField     _keyAliasNameTextField;
        private TextField     _keyAliasPasswordTextField;
        private Button        _saveKeystoreButton;

        private string _keystorePassword;
        private string _keyAliasName;
        private string _keyAliasPassword;

        public const string KEYSTORE_PASSWORD  = "KeystorePassword";
        public const string KEY_ALIAS_NAME     = "KeyAliasName";
        public const string KEY_ALIAS_PASSWORD = "KeyAliasPassword";

        [MenuItem("Tools/Lazy Tool")]
        public static void ShowExample()
        {
            CustomWindow wnd = GetWindow<CustomWindow>();
            wnd.titleContent = new GUIContent("Lazy Tool");
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            visualTreeAsset.CloneTree(root);

            _folderVisualElement = root.Q<VisualElement>("FolderVisualElement");
            _buildVisualElement  = root.Q<VisualElement>("BuildVisualElement");

            _folderToolbarButton         =  root.Q<ToolbarButton>("FolderToolbarButton");
            _folderToolbarButton.clicked += OnFolderToolbarButton;

            _buildToolbarButton         =  root.Q<ToolbarButton>("BuildToolbarButton");
            _buildToolbarButton.clicked += OnBuildToolbarButton;

            #region Folder

            _createFoldersButton         =  root.Q<Button>("CreateFoldersButton");
            _createFoldersButton.clicked += OnCreateFoldersButton;

            _organizingFilesButton         =  root.Q<Button>("OrganizingFilesButton");
            _organizingFilesButton.clicked += OnOrganizingFilesButton;

            #endregion

            #region Build

            _platformDropdownField         = root.Q<DropdownField>("PlatformDropdownField");
            _platformDropdownField.choices = new List<string> { "Windows, Mac, Linux", "Android" };
            _platformDropdownField.RegisterValueChangedCallback(OnPlatformDropdownField());

            _pcVisualElement      = root.Q<VisualElement>("PCVisualElement");
            _androidVisualElement = root.Q<VisualElement>("AndroidVisualElement");

            _pcVersionToggle           =  _pcVisualElement.Q<Toggle>("VersionToggle");
            _pcIncrementButton         =  _pcVisualElement.Q<Button>("IncrementButton");
            _pcIncrementButton.clicked += OnPCIncrementButton;

            _androidVersionToggle           =  _androidVisualElement.Q<Toggle>("VersionToggle");
            _bundleVersionCodeToggle        =  _androidVisualElement.Q<Toggle>("BundleVersionCodeToggle");
            _androidIncrementButton         =  _androidVisualElement.Q<Button>("IncrementButton");
            _androidIncrementButton.clicked += OnAndroidIncrementButton;

            _keystorePasswordTextField  =  _androidVisualElement.Q<TextField>("KeystorePasswordTextField");
            _keyAliasNameTextField      =  _androidVisualElement.Q<TextField>("KeyAliasNameTextField");
            _keyAliasPasswordTextField  =  _androidVisualElement.Q<TextField>("KeyAliasPasswordTextField");
            _saveKeystoreButton         =  _androidVisualElement.Q<Button>("SaveKeystoreButton");
            _saveKeystoreButton.clicked += OnSaveKeystoreButton;

            _keystorePasswordTextField.value = PlayerPrefs.GetString(KEYSTORE_PASSWORD);
            _keyAliasNameTextField.value     = PlayerPrefs.GetString(KEY_ALIAS_NAME);
            _keyAliasPasswordTextField.value = PlayerPrefs.GetString(KEY_ALIAS_PASSWORD);

            #endregion
        }

        private void OnFolderToolbarButton()
        {
            _folderVisualElement.style.display = DisplayStyle.Flex;
            _buildVisualElement.style.display  = DisplayStyle.None;
        }

        private void OnBuildToolbarButton()
        {
            _folderVisualElement.style.display = DisplayStyle.None;
            _buildVisualElement.style.display  = DisplayStyle.Flex;
        }

        private void CreateFolders(string path, IList<string> folders)
        {
            foreach (string folder in folders)
            {
                if (!Directory.Exists(@$"{path}/{folder}"))
                {
                    Directory.CreateDirectory(@$"{path}/{folder}");
                }
            }

            AssetDatabase.Refresh();
        }

        private void OnCreateFoldersButton()
        {
            CreateFolders(@"Assets", new List<string>
            {
                "Art",
                "Audio",
                "Prefabs",
                "Scripts",
            });

            CreateFolders(@"Assets/Art", new List<string>
            {
                "Animations",
                "Animators",
                "Fonts",
                "Materials",
                "Models",
                "Shaders",
                "Sprites",
                "Textures",
            });

            CreateFolders(@"Assets/Art/Sprites", new List<string>
            {
                "Animate",
                "UI",
            });

            CreateFolders(@"Assets/Art/Sprites/UI", new List<string>
            {
                "HUD",
                "Menu",
            });

            CreateFolders(@"Assets/Audio", new List<string>
            {
                "Music",
                "SoundEffect",
            });

            CreateFolders(@"Assets/Scenes", new List<string>
            {
                "Levels",
            });

            CreateFolders(@"Assets/Scripts", new List<string>
            {
                "Editor",
                "Runtime",
            });

            CreateFolders(@"Assets/Scripts/Runtime", new List<string>
            {
                "Core",
            });
        }

        private void OnOrganizingFilesButton()
        {
            FileInfo[] files = new DirectoryInfo(Application.dataPath).GetFiles("*.*", SearchOption.TopDirectoryOnly);
            foreach (FileInfo file in files)
            {
                int startIndex = file.ToString().IndexOf(".", StringComparison.Ordinal);
                switch (file.ToString().Substring(startIndex).ToLower())
                {
                    case ".anim":
                    case ".anim.meta":
                        AssetDatabase.MoveAsset("Assets/" + file.Name,
                            Path.Combine(@"Assets/Art/Animations", file.Name));
                        break;
                    case ".controller":
                    case ".controller.meta":
                        AssetDatabase.MoveAsset("Assets/" + file.Name,
                            Path.Combine(@"Assets/Art/Animators", file.Name));
                        break;
                    case ".ttf":
                    case ".ttf.meta":
                    case ".otf":
                    case ".otf.meta":
                        AssetDatabase.MoveAsset("Assets/" + file.Name,
                            Path.Combine(@"Assets/Art/Fonts", file.Name));
                        break;
                    case ".mat":
                    case ".mat.meta":
                        AssetDatabase.MoveAsset("Assets/" + file.Name,
                            Path.Combine(@"Assets/Art/Materials", file.Name));
                        break;
                    case ".fbx":
                    case ".fbx.meta":
                    case ".dae":
                    case ".dae.meta":
                    case ".dxf":
                    case ".dxf.meta":
                    case ".obj":
                    case ".obj.meta":
                    case ".max":
                    case ".max.meta":
                    case ".blend":
                    case ".blend.meta":
                        AssetDatabase.MoveAsset("Assets/" + file.Name,
                            Path.Combine(@"Assets/Art/Models", file.Name));
                        break;
                    case ".bmp":
                    case ".bmp.meta":
                    case ".exr":
                    case ".exr.meta":
                    case ".gif":
                    case ".gif.meta":
                    case ".hdr":
                    case ".hdr.meta":
                    case ".iff":
                    case ".iff.meta":
                    case ".jpg":
                    case ".jpg.meta":
                    case ".pict":
                    case ".pict.meta":
                    case ".png":
                    case ".png.meta":
                    case ".psd":
                    case ".psd.meta":
                    case ".tga":
                    case ".tga.meta":
                    case ".tiff":
                    case ".tiff.meta":
                        AssetDatabase.MoveAsset("Assets/" + file.Name,
                            Path.Combine(@"Assets/Art/Textures", file.Name));
                        break;
                    case ".mp3":
                    case ".mp3.meta":
                    case ".ogg":
                    case ".ogg.meta":
                    case ".wav":
                    case ".wav.meta":
                    case ".mod":
                    case ".mod.meta":
                    case ".it":
                    case ".it.meta":
                    case ".s3m":
                    case ".s3m.meta":
                    case ".xm":
                    case ".xm.meta":
                    case ".aiff":
                    case ".aiff.meta":
                    case ".aif":
                    case ".aif.meta":
                        AssetDatabase.MoveAsset("Assets/" + file.Name,
                            Path.Combine(@"Assets/Audio", file.Name));
                        break;
                    case ".prefab":
                    case ".prefab.meta":
                        AssetDatabase.MoveAsset("Assets/" + file.Name,
                            Path.Combine(@"Assets/Prefabs", file.Name));
                        break;
                    case ".unity":
                    case ".unity.meta":
                        AssetDatabase.MoveAsset("Assets/" + file.Name,
                            Path.Combine(@"Assets/Scenes", file.Name));
                        break;
                    case ".cs":
                    case ".cs.meta":
                        AssetDatabase.MoveAsset("Assets/" + file.Name,
                            Path.Combine(@"Assets/Scripts", file.Name));
                        break;
                }
            }
        }

        private EventCallback<ChangeEvent<string>> OnPlatformDropdownField()
        {
            return _ =>
            {
                switch (_platformDropdownField.index)
                {
                    case 0:
                        _pcVisualElement.style.display      = DisplayStyle.Flex;
                        _androidVisualElement.style.display = DisplayStyle.None;
                        break;
                    case 1:
                        _pcVisualElement.style.display      = DisplayStyle.None;
                        _androidVisualElement.style.display = DisplayStyle.Flex;
                        break;
                }
            };
        }

        private void OnPCIncrementButton()
        {
            if (!_pcVersionToggle.value) return;

            string originVersion = PlayerSettings.bundleVersion;

            float.TryParse(PlayerSettings.bundleVersion, out float version);
            version                      += .01f;
            PlayerSettings.bundleVersion =  version.ToString(CultureInfo.InvariantCulture);

            Debug.Log($"Version: {originVersion} -> {PlayerSettings.bundleVersion}");
        }

        private void OnAndroidIncrementButton()
        {
            if (_androidVersionToggle.value)
            {
                var oldVersion = PlayerSettings.bundleVersion;

                float.TryParse(PlayerSettings.bundleVersion, out var newVersion);
                newVersion                   += .01f;
                PlayerSettings.bundleVersion =  newVersion.ToString(CultureInfo.InvariantCulture);

                Debug.Log($"Version: {oldVersion} -> {newVersion}");
            }

            if (_bundleVersionCodeToggle.value)
            {
                var oldBundleVersionCode = PlayerSettings.Android.bundleVersionCode;

                var newBundleVersionCode = ++PlayerSettings.Android.bundleVersionCode;

                Debug.Log($"Bundle Version Code: {oldBundleVersionCode} -> {newBundleVersionCode}");
            }
        }

        private void OnSaveKeystoreButton()
        {
            _keystorePassword = _keystorePasswordTextField.value;
            _keyAliasName     = _keyAliasNameTextField.value;
            _keyAliasPassword = _keyAliasPasswordTextField.value;

            PlayerSettings.Android.keystorePass = _keystorePassword;
            PlayerSettings.Android.keyaliasName = _keyAliasName;
            PlayerSettings.Android.keyaliasPass = _keyAliasPassword;

            PlayerPrefs.SetString(KEYSTORE_PASSWORD,  _keystorePassword);
            PlayerPrefs.SetString(KEY_ALIAS_NAME,     _keyAliasName);
            PlayerPrefs.SetString(KEY_ALIAS_PASSWORD, _keyAliasPassword);
            PlayerPrefs.Save();

            Debug.Log("Keystore has been saved.");
        }
    }
}