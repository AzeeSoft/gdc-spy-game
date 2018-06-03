using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EasyButtons
{
    public static class EasyButtonsEditorExtensions
    {
        public static void DrawEasyButtons(this Editor editor)
        {
            DrawEasyButtonForObject(editor.target);
        }

        public static void DrawEasyButtonForObject(object target)
        {
            // Loop through all methods with no parameters
            var methods = target.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.GetParameters().Length == 0);
            foreach (var method in methods)
            {
                // Get the ButtonAttribute on the method (if any)
                var ba = (ButtonAttribute)Attribute.GetCustomAttribute(method, typeof(ButtonAttribute));

                if (ba != null)
                {
                    // Determine whether the button should be enabled based on its mode
                    GUI.enabled = ba.Mode == ButtonMode.AlwaysEnabled
                                  || (EditorApplication.isPlaying ? ba.Mode == ButtonMode.EnabledInPlayMode : ba.Mode == ButtonMode.DisabledInPlayMode);

                    // Draw a button which invokes the method
                    var buttonName = String.IsNullOrEmpty(ba.Name) ? ObjectNames.NicifyVariableName(method.Name) : ba.Name;
                    if (GUILayout.Button(buttonName))
                    {
                        method.Invoke(target, null);
                    }

                    GUI.enabled = true;
                }
            }

            /*foreach (FieldInfo fieldInfo in target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
            {
//                Debug.Log("Field: " + fieldInfo.Name);

                object field = fieldInfo.GetValue(target);

                if (field is Object)
                    DrawEasyButtonForObject(field);
                /*else if (field is GameManagerOld)
                {
                    foreach (var method in field.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Where(m => m.GetParameters().Length == 0))
                    {
                        // Get the ButtonAttribute on the method (if any)
                        var ba = (ButtonAttribute)Attribute.GetCustomAttribute(method, typeof(ButtonAttribute));

                        if (ba != null)
                        {
                            // Determine whether the button should be enabled based on its mode
                            GUI.enabled = ba.Mode == ButtonMode.AlwaysEnabled
                                          || (EditorApplication.isPlaying ? ba.Mode == ButtonMode.EnabledInPlayMode : ba.Mode == ButtonMode.DisabledInPlayMode);

                            // Draw a button which invokes the method
                            var buttonName = String.IsNullOrEmpty(ba.Name) ? ObjectNames.NicifyVariableName(method.Name) : ba.Name;
                            if (GUILayout.Button(buttonName))
                            {
                                method.Invoke(field, null);
                            }

                            GUI.enabled = true;
                        }
                    }
                }#1#
            }*/
        }
    }
}
