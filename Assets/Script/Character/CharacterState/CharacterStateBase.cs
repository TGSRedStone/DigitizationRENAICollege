using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State;

namespace Character
{
    public abstract class CharacterStateBase : StateBase<Character, CharacterStateBase>
    {
        public virtual void LateUpdate() { }
    }
}