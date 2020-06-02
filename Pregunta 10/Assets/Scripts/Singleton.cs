using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LearningRace
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T instance;
        public static T Instance { get { return instance; } protected set { instance = value; } }
        public static event Action InstanceSet;

        protected virtual void Awake()
        {
            if (instance != null)
                Destroy(gameObject);
            else
            {
                instance = (T)this;

                if (InstanceSet != null)
                    InstanceSet();
            }
        }

        protected virtual void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
    }
}
