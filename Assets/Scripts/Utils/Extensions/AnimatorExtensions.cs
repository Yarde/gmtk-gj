using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Yarde.Utils.Extensions
{
    public static class AnimatorExtensions
    {
        public static UniTask WaitForState(this Animator animator, string stateName)
        {
            return UniTask.WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(stateName));
        }
        
        public static UniTask WaitForStateChange(this Animator animator)
        {
            int name = animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
            return UniTask.WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).shortNameHash != name);
        }

        public static UniTask WaitForStateEnd(this Animator animator)
        {
            return UniTask.WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        }
    }
}
