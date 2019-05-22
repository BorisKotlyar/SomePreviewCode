using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Common.Utils
{
    public static class FileManager
    {
        private const string CacheFolderName = "AppCache";
        private const string ResourcesFolderName = "Resources";

        public static string SettingsPath => Application.persistentDataPath + "/Settings/";

        public static string CacheFolderPath
        {
            get
            {
                var cacheFolderPath = string.Format("{0}/{1}", Application.persistentDataPath, CacheFolderName);
                if (!Directory.Exists(cacheFolderPath))
                {
                    Directory.CreateDirectory(cacheFolderPath);
#if UNITY_IOS
					UnityEngine.iOS.Device.SetNoBackupFlag(cacheFolderPath);
#endif
                }
                return cacheFolderPath;
            }
        }

        public static JsonSerializerSettings DefaultSerializerSettings { get; } =
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Converters =
                    new System.Collections.Generic.List<JsonConverter>
                    {
                        new Newtonsoft.Json.Converters.StringEnumConverter()
                    }
            };

        private static string GetFileFullPath(string filePath)
        {
            return Path.Combine(CacheFolderPath, filePath);
        }

        public static bool IsFileExists(string filePath)
        {
            var path = GetFileFullPath(filePath);

            return File.Exists(path);
        }

        public static void DeleteFile(string filePath)
        {
            var path = GetFileFullPath(filePath);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static string Load(string filePath)
        {
            var path = GetFileFullPath(filePath);

            if (!File.Exists(path))
            {
                Debug.Log("[FileManager.Load] File not exist! path: " + path);
                return string.Empty;
            }

            var sr = new StreamReader(path);
            var data = sr.ReadToEnd();
            sr.Close();

            return Encoding.UTF8.GetString(Convert.FromBase64String(data));
        }

        public static void Save(string filePath, string text)
        {
            var path = GetFileFullPath(filePath);

            var sw = new StreamWriter(path);
            sw.Write(Convert.ToBase64String(Encoding.UTF8.GetBytes(text)));
            sw.Close();
        }

        public static T Load<T>(string filePath, JsonSerializerSettings serializerSettings = null)
        {
            var text = Load(filePath);

            if (string.IsNullOrEmpty(text))
                Debug.Log("[FileManager.Load] Saves content is empty, path: " + GetFileFullPath(filePath));

            return string.IsNullOrEmpty(text)
                ? default(T)
                : JsonConvert.DeserializeObject<T>(text, serializerSettings ?? DefaultSerializerSettings);
        }

        public static void Save<T>(string filePath, T data, JsonSerializerSettings serializerSettings = null)
        {
            var json =
                JsonConvert.SerializeObject(data, Formatting.Indented, serializerSettings ?? DefaultSerializerSettings);

            Save(filePath, json);
        }

        public static void Copy(string srcPath, string destPath)
        {
            var path = GetFileFullPath(srcPath);
            var dPath = GetFileFullPath(destPath);

            if (!File.Exists(path))
            {
                Debug.Log("[FileManager.Move] File not exists, path: " + path);
                return;
            }

            File.Copy(path, dPath, true);
        }

        public static T LoadFromResources<T>(string filePath, JsonSerializerSettings serializerSettings = null)
        {
            var id = filePath.LastIndexOf(".");
            filePath = id < 0 ? filePath : filePath.Remove(id, filePath.Length - id);
            var textAsset = Resources.Load<TextAsset>(filePath);
            return textAsset == null
                ? default(T)
                : JsonConvert.DeserializeObject<T>(textAsset.text, serializerSettings ?? DefaultSerializerSettings);
        }

        public static T LoadConfigFile<T>(string configName)
        {
            if (!Directory.Exists(SettingsPath))
                Directory.CreateDirectory(SettingsPath);

            var filePath = SettingsPath + configName;

            if (!File.Exists(filePath))
            {
                var localAsset = Resources.Load<TextAsset>("DefaultSettings/" + configName);
                if (localAsset != null)
                    return JsonConvert.DeserializeObject<T>(localAsset.text, DefaultSerializerSettings);

                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath), DefaultSerializerSettings);
        }

#if UNITY_EDITOR
        public static void SaveToResources<T>(string filePath, T data, JsonSerializerSettings serializerSettings = null)
        {
            var path = string.Format("{0}/{1}/{2}", Application.dataPath, ResourcesFolderName, filePath);
            var index = path.LastIndexOf("/");
            var directoryPath = path.Remove(index, path.Length - index);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var streamWriter = new StreamWriter(path);
            streamWriter.Write(JsonConvert.SerializeObject(data, Formatting.Indented,
                serializerSettings ?? DefaultSerializerSettings));
            streamWriter.Close();
            AssetDatabase.Refresh();
        }

        public static string GetResourcesPath(UnityEngine.Object asset)
        {
            if (asset == null) return string.Empty;
            var path = AssetDatabase.GetAssetPath(asset).Remove(0, 17);

            if (!path.Contains(ResourcesFolderName)) return string.Empty;
            var id = path.LastIndexOf(".");
            return path.Remove(id, path.Length - id);
        }
#endif

        public static void DeleteCache()
        {
            Directory.Delete(CacheFolderPath, true);
        }
    }
}
