﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using LJX8000.Core.ViewModels.Base;

namespace LJX8000.Core.Helpers
{
    public static class AutoSerializableHelper
    {
        /// <summary>
        /// Load auto-serializables from disk
        /// If anyone not exist, a new one will be created
        /// </summary>
        /// <param name="objectNames">Names of the <see cref="AutoSerializableBase{T}"/></param>
        /// <param name="serializationDir">The directory that the auto-serializables exists</param>
        /// <typeparam name="T">Type of the auto-serializaables to load</typeparam>
        /// <returns>
        /// If <see cref="objectNames"/> has no names, return default constructed List
        /// </returns>
        public static IEnumerable<T> LoadAutoSerializables<T>(IEnumerable<string> objectNames, string serializationDir)
            where T : AutoSerializableBase<T>, new()
        {
            var outputs = new List<T>();
            var names = objectNames.ToList();
            if (names.Count == 0) return outputs;

            Directory.CreateDirectory(serializationDir);

            foreach (var name in names)
            {
                var filePath = Path.Combine(serializationDir, name + ".xml");
                try // Load it from disk
                {
                    using (var fs = new FileStream(filePath, FileMode.Open))
                    {
                        var serializer = new XmlSerializer(typeof(T));
                        T item = (T) serializer.Deserialize(fs);
                        item.SerializationDirectory = serializationDir;
                        outputs.Add(item);
                    }
                }
                catch (FileNotFoundException e) // If not in disk, create a new one
                {
                    outputs.Add(new T
                    {
                        Name = name,
                        SerializationDirectory = serializationDir
                    });
                }
            }

            foreach (var item in outputs)
            {
                item.ShouldAutoSerialize = true;
            }

            return outputs;
        }

        /// <summary>
        /// Load all the auto-serializables from <see cref="serializationDir"/>
        /// </summary>
        /// <param name="serializationDir"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> LoadAllAutoSerializables<T>(string serializationDir)
            where T : AutoSerializableBase<T>, new()
        {
            var outputs = new List<T>();

            Directory.CreateDirectory(serializationDir);
            var xmlFiles = Directory.GetFiles(serializationDir).Where(file => file.EndsWith("xml")).ToArray();
            if (xmlFiles.Length == 0) return outputs;

            foreach (var filePath in xmlFiles)
            {
                using (var fs = new FileStream(filePath, FileMode.Open))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    T item = (T) serializer.Deserialize(fs);
                    item.SerializationDirectory = serializationDir;
                    item.ShouldAutoSerialize = true;
                    outputs.Add(item);
                }
            }

            return outputs;
        }


        /// <summary>
        /// Set ShouldAutoSerialize of auto-serializables to true
        /// </summary>
        /// <param name="autoSerializables"></param>
        /// <typeparam name="T">Concrete type of auto-serializable</typeparam>
        public static void StartAutoSerializing<T>(this IEnumerable<T> autoSerializables)
            where T : AutoSerializableBase<T>
        {
            foreach (var autoSerializable in autoSerializables)
            {
                autoSerializable.ShouldAutoSerialize = true;
            }
        }

        /// <summary>
        /// Set ShouldAutoSerialize of auto-serializables to false
        /// </summary>
        /// <param name="autoSerializables"></param>
        /// <typeparam name="T">Concrete type of auto-serializable</typeparam>
        public static void StopAutoSerializing<T>(this IEnumerable<T> autoSerializables)
            where T : AutoSerializableBase<T>
        {
            foreach (var autoSerializable in autoSerializables)
            {
                autoSerializable.ShouldAutoSerialize = false;
            }
        }

        /// <summary>
        /// Return the first auto-serializable with the required name
        /// </summary>
        /// <param name="autoSerializables"></param>
        /// <param name="name"></param>
        /// <typeparam name="T">Concrete type of auto-serializable</typeparam>
        /// <returns></returns>
        public static T ByName<T>(this IEnumerable<T> autoSerializables, string name)
            where T : AutoSerializableBase<T>
        {
            return autoSerializables.FirstOrDefault(ele => ele.Name == name);
        }
    }
}