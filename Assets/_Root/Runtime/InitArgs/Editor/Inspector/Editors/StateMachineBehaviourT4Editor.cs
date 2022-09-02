﻿using System;
using Pancake.Init;
using UnityEditor;

namespace Pancake.Editor.Init
{
	[CustomEditor(typeof(StateMachineBehaviour<,,,>), true, isFallback = true), CanEditMultipleObjects]
    public class StateMachineBehaviourT4Editor : InitializableEditor
    {
        protected override Type BaseTypeDefinition => typeof(StateMachineBehaviour<,,,>);
    }
}