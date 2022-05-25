using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public enum CharacterAnimatorMode
    {
        noAnimation = 0,
        onlyAnimation = 1,
        AnimationWithPhysic = 2,
        AnimationWithoutPhysic = 3,
        onlyPhysic = 4
    }

    public class CharacterAnimator : MonoBehaviour
    {
        public Animator _Animator { get; private set; }

        public CharacterAnimatorMode CurrAnimatorMode { get; private set; }

        private void OnAnimatorMove()
        {
            switch (CurrAnimatorMode)
            {
                case CharacterAnimatorMode.noAnimation:
                    break;
                case CharacterAnimatorMode.onlyAnimation:
                    break;
                case CharacterAnimatorMode.AnimationWithPhysic:
                    break;
                case CharacterAnimatorMode.AnimationWithoutPhysic:
                    break;
                case CharacterAnimatorMode.onlyPhysic:
                    break;
                default:
                    break;
            }
        }

        public void ChangeAnimatorMode(CharacterAnimatorMode newAnimatorMode)
        {
            if (CurrAnimatorMode == newAnimatorMode) return;
            CurrAnimatorMode = newAnimatorMode;
        }
    }
}