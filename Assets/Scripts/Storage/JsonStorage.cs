using System;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HexGame.Storage
{
    public class JsonStorage<T> : IStorage<T>
    {
        public async UniTask Save(string path, T value)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path) ?? throw new InvalidOperationException());
                var jsonStr = JsonUtility.ToJson(value, true);
                await File.WriteAllTextAsync(path, jsonStr);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't save object '{nameof(value)}' to '{path}'.", ex);
            }
        }

        public async UniTask<T> Load(string path)
        {
            if (!File.Exists(path))
                return default;

            try
            {
                var dataAsJson = await File.ReadAllTextAsync(path);
                return JsonUtility.FromJson<T>(dataAsJson);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't load object from '{path}'.", ex);
            }
        }
    }
}