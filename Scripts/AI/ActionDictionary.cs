using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;

namespace Weggo.AI
{
    public class ActionDictionary
    {
        private static Type[] actions;
        
        [MenuItem("Tools/Weggo/Reload Action Dictionary")]
        public static void GetActions()
        {
            List<Type> tt = new List<Type>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                tt.AddRange(assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(AgentAction))).ToList());
            }

            actions = tt.ToArray();

            Debug.Log("Action types found: " + actions.Length);
        }

        public static AgentAction GetAction(int index) { return (AgentAction)Activator.CreateInstance(actions[index]); }
    }
}