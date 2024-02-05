using System.IO;
using UnityEngine;

namespace BugiGames.Tools
{
    public class SOLoader<T> : UnityEngine.ScriptableObject, ISOLoader where T : UnityEngine.ScriptableObject
    {
        private static T immutableValue;

        /// <summary>
        /// Warning: This value can't be changed during runtime.
        /// </summary>
        public static T ImmutableValue
        {
            get
            {
                if (immutableValue == null)
                {
                    var path = $"{SOProps.folderName}/{typeof(T)}";
                    immutableValue = Resources.Load<T>(path);

                    if (immutableValue == null)
                    {
                        Debug.LogWarning($"Cant load Scriptable Object in path: \"{path}\", its null," +
                                         $" file name is {typeof(T)}, path is not Specific");
                    }
                }

                return immutableValue;
            }
        }

        public static T Value
        {
            get
            {
                var path = $"{SOProps.folderName}/{typeof(T)}";

                var loadedInstance = Resources.Load<T>(path);

                if (loadedInstance == null)
                {
                    Debug.LogWarning($"Cant load Scriptable Object in path: \"{path}\", its null, file name is {typeof(T)}," +
                                     $" path is not Specific");
                }

                return loadedInstance;
            }
        }

        public static T SpecificValue(string path)
        {
            var loadedInstance = Resources.Load<T>(path);

            if (loadedInstance == null)
            {
                Debug.LogWarning($"Cant load Scriptable Object in path: \"{path}\", its null, file name is {typeof(T)}," +
                                 $" path is Specific");
            }

            return loadedInstance;
        }
    }

    interface ISOLoader
    {

    }
}
