using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State
{
    /// <summary>
    /// 状态机
    /// </summary>
    /// <typeparam name="T">子状态基类类型</typeparam>
    /// <typeparam name="U">状态持有者类型</typeparam>
    public class StateMachine<T, U> : MonoBehaviour where T : StateBase<U, T>
    {
        protected T State { get; private set; }
        private List<T> states = new List<T>();

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <typeparam name="S">要切换的状态</typeparam>
        /// <param name="owner">状态持有者</param>
        public void ChangeState<S>(U owner)where S : T, new()
        {
            if (State != null)
            {
                if (State is S) return;
                State.OnExit();
            }

            State = CreatState<S>(owner);
            State.OnEnter();
        }

        /// <summary>
        /// 创建状态
        /// </summary>
        /// <typeparam name="S">要创建的状态</typeparam>
        /// <param name="owner">状态持有者</param>
        /// <returns></returns>
        private T CreatState<S>(U owner) where S : T, new()
        {
            foreach (T state in states)
                if (state is S) return state;

            T temp = new S();
            temp._Owner = owner;
            temp.PreState = State;

            states.Add(temp);
            return temp;
        }
    }
}