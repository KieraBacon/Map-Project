using System.Linq;
using UnityEngine;

namespace Project.Scripts
{
    public static class DependencyGetter
    {
        public static bool TryGetDependencies<T, U>(this T obj, out U dependency) where T : Component =>
            obj.TryGetComponent(out dependency);
        public static bool TryGetDependencies<T>(this T obj, Component[] dependencies)
            where T : Component
        {
            for (int i = 0; i < dependencies.Length; i++)
            {
                dependencies[i] = obj.GetComponent(dependencies[i].GetType());
            }

            return dependencies.All(x => x != null);
        }
    }
}