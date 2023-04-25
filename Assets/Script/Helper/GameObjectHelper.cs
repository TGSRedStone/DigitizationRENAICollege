using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    public static class GameObjectHelper
    {
        private static GameObject tempGameObject;
        private static Transform[] tempTransforms;

        /// <summary>
        /// 根据名称在子物体中查找GameObject
        /// <para>默认只对子辈查找，onlyInSon为false时对所有子物体进行查找</para>
        /// <para>对连续在一个物体上查找时有一定优化</para>
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="name"></param>
        /// <param name="onlyInSon"></param>
        /// <returns></returns>
        public static GameObject FindGameObjectWithNameInChild(this GameObject gameObject, string name, bool onlyInSon = true)
        {
            if (onlyInSon)
            {
                return gameObject.transform.Find(name).gameObject;
            }
            else
            {
                if (!tempGameObject)
                {
                    tempGameObject = gameObject;
                    tempTransforms = gameObject.GetComponentsInChildren<Transform>();
                }
                foreach (Transform temp in tempTransforms)
                    if (temp.name == name) return temp.gameObject;
            }
#if UNITY_EDITOR
            Debug.LogWarning($"无法在\0{gameObject.name}\0及其子物体中找到\0{name}\0");
#endif
            return null;
        }

        /// <summary>
        /// 获取组件
        /// <para>没有该组件时自动添加</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static T GetAndAddComponent<T>(this GameObject gameObject)where T : Component
        {
            if (gameObject.TryGetComponent<T>(out T temp))
            {
                return temp;
            }
            else
            {
                return gameObject.AddComponent<T>();
            }
        }
    }
}