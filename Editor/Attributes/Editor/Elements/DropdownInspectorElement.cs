﻿using System;
using System.Collections.Generic;
using System.Linq;
using Pancake.Attribute;
using UnityEditor;
using UnityEngine;

namespace PancakeEditor.Attribute
{
    public class DropdownInspectorElement : InspectorElement
    {
        private readonly Property _property;
        private readonly Func<Property, IEnumerable<IDropdownItem>> _valuesGetter;

        private object _currentValue;
        private string _currentText;

        private bool _hasNextValue;
        private object _nextValue;

        public DropdownInspectorElement(Property property, Func<Property, IEnumerable<IDropdownItem>> valuesGetter)
        {
            _property = property;
            _valuesGetter = valuesGetter;
        }

        public override float GetHeight(float width) { return EditorGUIUtility.singleLineHeight; }

        public override void OnGUI(Rect position)
        {
            if (_hasNextValue)
            {
                object nextValue = _nextValue;
                _hasNextValue = false;
                _nextValue = null;

                _property.SetValue(nextValue);
                GUI.changed = true;
            }

            if (!_property.Comparer.Equals(_currentValue, _property.Value))
            {
                _currentValue = _property.Value;
                _currentText = _valuesGetter.Invoke(_property).FirstOrDefault(it => _property.Comparer.Equals(it.Value, _property.Value))?.Text ?? "";
            }

            var controlId = GUIUtility.GetControlID(FocusType.Passive);
            position = EditorGUI.PrefixLabel(position, controlId, _property.DisplayNameContent);

            if (GUI.Button(position, _currentText, EditorStyles.popup))
            {
                ShowDropdown(position);
            }
        }

        private void ShowDropdown(Rect position)
        {
            var items = _valuesGetter.Invoke(_property);
            var menu = new GenericMenu();

            foreach (var item in items)
            {
                var isOn = _property.Comparer.Equals(item.Value, _property.Value);
                menu.AddItem(new GUIContent(item.Text), isOn, ChangeValue, item.Value);
            }

            menu.DropDown(position);
            
            void ChangeValue(object v)
            {
                _nextValue = v;
                _hasNextValue = true;
                _property.PropertyTree.RequestRepaint();
            }
        }
    }
}