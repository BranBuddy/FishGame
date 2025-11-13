/**
    Written by Brandon Wahl

    A generic singleton class for Unity MonoBehaviours.
    Ensures that only one instance of the class exists in the scene.
    If no instance exists, it creates a new GameObject with the singleton component attached.
**/


using UnityEngine;


namespace Singletons
{

    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance; // private static instance of the singleton
        public static T Instance // public static property to access the singleton instance
        {
            get 
            {
                // if the instance doesnt exist already, this will try to create one
                if (_instance == null)
                {
                    // Try to find an existing instance in the scene
                    _instance = FindAnyObjectByType<T>();

                    if (_instance == null)
                    {
                        // if no instance is found, create a new GameObject with the singleton component
                        Debug.LogWarning($"No instance of {typeof(T).Name} found in the scene. Creating a new one.");
                        GameObject singletonObject = new GameObject(typeof(T).Name);
                        _instance = singletonObject.AddComponent<T>();
                        DontDestroyOnLoad(singletonObject);
                    }
                }
                return _instance;
        }

        private set {_instance = value; }
    }

    // Awake method to enforce the singleton pattern
    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Debug.LogWarning($"Another instance of {typeof(T).Name} already exists. Destroying this one.");
            //only destroys component to avoid destroying other components on the same GameObject
            Destroy(this);
        }

    }
    }
}
