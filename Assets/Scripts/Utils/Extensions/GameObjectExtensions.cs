using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Yarde.Utils.Extensions
{
    public static class GameObjectExtensions
    {
        public static void EnableComponentsInChildren<TComponent>(this GameObject gameObject,
            Predicate<TComponent> condition = default) where TComponent : MonoBehaviour
        {
            foreach (TComponent component in gameObject.GetComponentsInChildren<TComponent>())
            {
                if (condition == null || condition.Invoke(component))
                {
                    component.enabled = true;
                }
            }
        }

        public static void DisableComponentsInChildren<TComponent>(this GameObject gameObject,
            Predicate<TComponent> condition = default) where TComponent : MonoBehaviour
        {
            foreach (TComponent component in gameObject.GetComponentsInChildren<TComponent>())
            {
                if (condition == null || condition.Invoke(component))
                {
                    component.enabled = false;
                }
            }
        }

        public static void Destroy(GameObject go)
        {
            if (Application.isPlaying)
            {
                Object.Destroy(go);
            }
            else
            {
                Object.DestroyImmediate(go);
            }
        }

        public static T AddMissingComponent<T>(this GameObject go) where T : Component
        {
            T component = go.GetComponent<T>();
            if (component == null)
            {
                component = go.AddComponent<T>();
            }

            return component;
        }

        public static bool DestroyComponent<TComponent>(this GameObject go, bool forceImmediate = false)
            where TComponent : Component
        {
            TComponent component = go.GetComponent<TComponent>();
            if (component)
            {
                if (!Application.isPlaying || forceImmediate)
                {
                    Object.DestroyImmediate(component);
                }
                else
                {
                    Object.Destroy(component);
                }

                return true;
            }

            return false;
        }

        public static bool DestroyComponent<TComponent>(this Component other) where TComponent : Component
        {
            return DestroyComponent<TComponent>(other.gameObject);
        }

        public static bool IsPrefab(this GameObject go)
        {
            return string.IsNullOrEmpty(go.scene.name);
        }

        public static string FullPath(this GameObject go)
        {
            if (go.transform.parent != null)
            {
                return $"{go.transform.parent.gameObject.FullPath()}/{go.name}";
            }
            else
            {
                return $"/{go.name}";
            }
        }

        public static T GetProvider<T>(this GameObject gameObject, T providerNone)
        {
            T provider = gameObject.GetComponent<T>() ?? gameObject.GetComponentInChildren<T>();
            return provider == null ? providerNone : provider;
        }
    }
}