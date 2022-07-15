using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Yarde.WindowSystem.TransitionProvider
{
    internal static class ElementTransitionProviderExtensions
    {
        public static UniTask TransitionInSafe(this ITransitionProvider transition) => transition?.TransitionIn() ?? UniTask.CompletedTask;
        public static UniTask TransitionOutSafe(this ITransitionProvider transition) => transition?.TransitionOut() ?? UniTask.CompletedTask;

        /// <summary>
        /// Quick way to call all TransitionIn animations for TransitionProvider of this object (and optionally, its children)
        /// </summary>
        public static async UniTask TransitionIn(this GameObject gameObject, bool includeChildren)
        {
            if (includeChildren)
            {
                var transitions = gameObject.GetComponentsInChildren<ITransitionProvider>();
                await UniTask.WhenAll(transitions.Select(t => t.TransitionIn()));
            }
            else
            {
                var transition = gameObject.GetComponent<ITransitionProvider>();
                if (transition != null)
                {
                    await transition.TransitionIn();
                }
            }
        }

        /// <summary>
        /// Quick way to call all TransitionOut animations for TransitionProvider of this object (and optionally, its children)
        /// </summary>
        public static async UniTask TransitionOut(this GameObject gameObject, bool includeChildren)
        {
            if (includeChildren)
            {
                var transitions = gameObject.GetComponentsInChildren<ITransitionProvider>();
                await UniTask.WhenAll(transitions.Select(t => t.TransitionOut()));
            }
            else
            {
                var transition = gameObject.GetComponent<ITransitionProvider>();
                if (transition != null)
                {
                    await transition.TransitionOut();
                }
            }
        }
    }
}
