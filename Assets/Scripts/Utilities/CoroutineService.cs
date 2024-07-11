using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Platformer.Utilities;

namespace Platformer.Services
{
    public class CoroutineService : GenericMonoSingleton<CoroutineService>
    {
        private Dictionary<int, Coroutine> runningCoroutines = new();


        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }


        public static int StartCoroutine(IEnumerator routine)
        {
            if (Instance == null)
            {
                GameObject coroutineServiceObject = new GameObject();
                coroutineServiceObject.AddComponent<CoroutineService>();
            }
            return Instance.StartCoroutineInternal(routine);
        }

        public static void StopCoroutine(int id)
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

        private int StartCoroutineInternal(IEnumerator routine)
        {
            int id = UniqueIdGenerator.GenerateUniqueId();
            Coroutine coroutine = base.StartCoroutine(routine);

            if (runningCoroutines.ContainsKey(id))
                base.StopCoroutine(runningCoroutines[id]);

            runningCoroutines[id] = coroutine;
            return id;
        }

        private void StopCoroutineInternal(int id)
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