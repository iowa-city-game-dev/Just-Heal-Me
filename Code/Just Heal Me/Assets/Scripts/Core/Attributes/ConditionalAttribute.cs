using System;
using UnityEditor;
using UnityEngine;

namespace Core.Attributes
{
#if UNITY_EDITOR
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class ConditionalAttribute : PropertyAttribute
    {
        public string ConditionalSourceField = string.Empty;
        public bool HideInInspector = false;
        public bool Inverse = false;

        public ConditionalAttribute(string conditionalSourceField, bool hideInInspector = false, bool inverse = false)
        {
            ConditionalSourceField = conditionalSourceField;
            HideInInspector = hideInInspector;
            Inverse = inverse;
        }
    }

    [CustomPropertyDrawer(typeof(ConditionalAttribute))]
    public class ConditionalHidePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var condAtt = attribute as ConditionalAttribute;
            var enabled = GetConditionalAttributeResult(condAtt, property);

            var wasEnabled = GUI.enabled;
            GUI.enabled = enabled;
            if (!condAtt.HideInInspector || enabled)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }

            GUI.enabled = wasEnabled;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var condAtt = attribute as ConditionalAttribute;
            var enabled = GetConditionalAttributeResult(condAtt, property);

            if (!condAtt.HideInInspector || enabled)
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }

            return -EditorGUIUtility.standardVerticalSpacing;
        }

        private bool GetConditionalAttributeResult(ConditionalAttribute conditionalAttribute, SerializedProperty property)
        {
            var enabled = true;
            var propertyPath = property.propertyPath;
            var conditionPath = propertyPath.Replace(property.name, conditionalAttribute.ConditionalSourceField);
            var sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

            if (sourcePropertyValue != null)
            {
                enabled = sourcePropertyValue.boolValue;

                if (conditionalAttribute.Inverse) enabled = !enabled;
            }
            else
            {
                Debug.LogWarning($"Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: {conditionalAttribute.ConditionalSourceField}");
            }

            return enabled;
        }
    }
#endif
}