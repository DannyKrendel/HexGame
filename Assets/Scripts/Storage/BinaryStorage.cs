using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HexGame.Storage
{
    public class BinaryStorage<T> : IStorage<T>
    {
        public async UniTask Save(string path, T data)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path) ?? throw new InvalidOperationException());

                var binaryFormatter = new BinaryFormatter();
                var fileStream = File.Create(path);
                
                binaryFormatter.Serialize(fileStream, data);
                
                fileStream.Close();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't save object '{nameof(data)}' to '{path}'.", ex);
            }
        }

        public async UniTask<T> Load(string path)
        {
            if (!File.Exists(path))
                return default;

            try
            {
                var binaryFormatter = new BinaryFormatter();
                var fileStream = File.Open(path, FileMode.Open);

                var data = (T)binaryFormatter.Deserialize(fileStream);
                
                fileStream.Close();

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't load object from '{path}'.", ex);
            }
        }
    }
}