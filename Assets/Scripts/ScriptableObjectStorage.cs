using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Managers
{
    public class ScriptableObjectStorage : MonoBehaviour
    {
        public static ScriptableObjectStorage Instance { get; private set; }

        private List<ScriptableObject> _configs;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

        }

        public T[] GetConfigsOfType<T>() where T : ScriptableObject
        {
            RefreshConfigs();
            List<T> res = new List<T>();
            foreach (var c in _configs)
            {
                if (c is T) res.Add(c as T);
            }
            return res.ToArray();
        }

        private void RefreshConfigs()
        {
            _configs = Resources.LoadAll<ScriptableObject>("Configs").ToList();
        }

    }
}