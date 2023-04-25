using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State
{
    /// <summary>
    /// 状态基类
    /// </summary>
    /// <typeparam name="T">状态持有者类型</typeparam>
    /// <typeparam name="U">子状态基类类型</typeparam>
    public abstract class StateBase<T, U>
    {
        //状态持有者
        public T _Owner;
        //上一个状态
        public U PreState;

        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void Update();
        public virtual void FixedUpdate() { }
    }
}