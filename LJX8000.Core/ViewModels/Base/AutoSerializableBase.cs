using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Xml.Serialization;
using LJX8000.Core.Attributes;
using PropertyChanged;

namespace LJX8000.Core.ViewModels.Base
{
    public class AutoSerializableBase<T> : ViewModelBase
    {
        /// <summary>
        /// Fai name
        /// </summary>
        [XmlAttribute]
        [DoNotNotify]
        public virtual string Name { get; set; }

        /// <summary>
        /// Whether the object should be auto-serialize when changed
        /// </summary>
        [XmlIgnore][DoNotNotify]
        public bool ShouldAutoSerialize { get; set; }

        public AutoSerializableBase()
        {
            PropertyChanged += Serialize;
        }

        public void Serialize(object sender, PropertyChangedEventArgs e)
        {
            if (!ShouldAutoSerialize) return;
            if (string.IsNullOrEmpty(Name)) return;

            using (var fs = new FileStream(Path.Combine(SerializationDirectory, Name + ".xml")
                , FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(fs, this);
            }
        }

        [DoNotNotify][XmlIgnore] public string SerializationDirectory { get; set; }
    }
}