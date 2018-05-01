﻿using System.Reflection;
using GameJolt.API;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

namespace GameJolt.Editor {
	[CustomEditor(typeof(Settings))]
	public class SettingsEditor : UnityEditor.Editor {
		public override void OnInspectorGUI() {
			var settings = target as Settings;
			if(settings == null) return;
			if(string.IsNullOrEmpty(settings.encryptionKey))
				settings.encryptionKey = GetRandomPassword();
			base.OnInspectorGUI();
			if(GUILayout.Button("Clear All Settings")) {
				Undo.RecordObject(target, "Clear GameJolt API settings");
				var empty = CreateInstance<Settings>();
				foreach(var fieldInfo in typeof(Settings).GetFields(BindingFlags.Public | BindingFlags.Instance)) {
					var value = fieldInfo.GetValue(empty);
					fieldInfo.SetValue(settings, value);
				}
				DestroyImmediate(empty);
				EditorUtility.SetDirty(target);
				Selection.activeObject = null;
			}
		}

		private static string GetRandomPassword() {
			const int pwLength = 16;
			const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789+-*/=?!§$%&";
			var pw = new char[pwLength];
			var rnd = new Random();
			for(int i = 0; i < pwLength; i++)
				pw[i] = chars[rnd.Next(chars.Length)];
			return new string(pw);
		}
	}
}
