﻿using UnityEditor;
using UnityEngine;

namespace MustHave.UI
{
    [CustomPropertyDrawer(typeof(ArrayElementTitleAttribute))]
    public class ArrayElementTitlePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        protected virtual ArrayElementTitleAttribute Attribute { get { return (ArrayElementTitleAttribute)attribute; } }

        private SerializedProperty titleProperty;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string fullPathName = property.propertyPath + "." + Attribute.title;
            titleProperty = property.serializedObject.FindProperty(fullPathName);
            string newlabel = GetTitle();
            if (string.IsNullOrEmpty(newlabel))
                newlabel = label.text;
            EditorGUI.PropertyField(position, property, new GUIContent(newlabel, label.tooltip), true);
        }

        private string GetTitle()
        {
            switch (titleProperty.propertyType)
            {
                case SerializedPropertyType.Generic:
                    break;
                case SerializedPropertyType.Integer:
                    return titleProperty.intValue.ToString();
                case SerializedPropertyType.Boolean:
                    return titleProperty.boolValue.ToString();
                case SerializedPropertyType.Float:
                    return titleProperty.floatValue.ToString();
                case SerializedPropertyType.String:
                    return titleProperty.stringValue;
                case SerializedPropertyType.Color:
                    return titleProperty.colorValue.ToString();
                case SerializedPropertyType.ObjectReference:
                    return titleProperty.objectReferenceValue.ToString();
                case SerializedPropertyType.LayerMask:
                    break;
                case SerializedPropertyType.Enum:
                    return titleProperty.enumNames[titleProperty.enumValueIndex];
                case SerializedPropertyType.Vector2:
                    return titleProperty.vector2Value.ToString();
                case SerializedPropertyType.Vector3:
                    return titleProperty.vector3Value.ToString();
                case SerializedPropertyType.Vector4:
                    return titleProperty.vector4Value.ToString();
                case SerializedPropertyType.Rect:
                    break;
                case SerializedPropertyType.ArraySize:
                    break;
                case SerializedPropertyType.Character:
                    break;
                case SerializedPropertyType.AnimationCurve:
                    break;
                case SerializedPropertyType.Bounds:
                    break;
                case SerializedPropertyType.Gradient:
                    break;
                case SerializedPropertyType.Quaternion:
                    break;
                default:
                    break;
            }
            return "";
        }
    } 
}