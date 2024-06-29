using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Platformer.Utilities;

namespace Platformer.Services
{
    public class CoroutineService : GenericMonoSingleton<CoroutineService>
    {
        private Dictionary<string, Coroutine> runningCoroutines = new Dictionary<string, Coroutine>();

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        public static Coroutine StartCoroutine(IEnumerator routine, string id = null)
        {
            if (Instance == null)
            {
                GameObject go = new GameObject("CoroutineService");
                go.AddComponent<CoroutineService>();
            }
            return Instance.StartCoroutineInternal(routine, id);
        }

        public static new void StopCoroutine(string id)
        {
            if (Instance != null)
            {
                Instance.StopCoroutineInternal(id);
            }
        }

        public static new void StopAllCoroutines()
        {
            if (Instance != null)
            {
                Instance.StopAllCoroutinesInternal();
            }
        }

        private Coroutine StartCoroutineInternal(IEnumerator routine, string id)
        {
            Coroutine coroutine = base.StartCoroutine(routine);
            if (!string.IsNullOrEmpty(id))
            {
                if (runningCoroutines.ContainsKey(id))
                {
                    base.StopCoroutine(runningCoroutines[id]);
                }
                runningCoroutines[id] = coroutine;
            }
            return coroutine;
        }

        private void StopCoroutineInternal(string id)
        {
            if (runningCoroutines.TryGetValue(id, out Coroutine coroutine))
            {
                base.StopCoroutine(coroutine);
                runningCoroutines.Remove(id);
            }
        }

        private void StopAllCoroutinesInternal()
        {
            base.StopAllCoroutines();
            runningCoroutines.Clear();
        }
    }
}