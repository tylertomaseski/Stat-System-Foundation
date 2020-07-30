namespace Stats
{
	using UnityEngine;
	using UnityEditor;
	using System;

	[CustomEditor(typeof(EntityStats))]
	public class EntityStatsEditor : Editor
	{
		bool _foldoutOriginal;

		public override void OnInspectorGUI()
		{

			var targetEntityStats = target as EntityStats;

			var property = serializedObject.FindProperty("Stats");

			this.DrawStatHeader();

			EditorGUI.indentLevel++;
			for (int i = 0; i < (int)Stat.Length; i++)
			{
				if (i == (int)Stat.Health)
					continue; //skip this, defaults to max

				var label = new GUIContent(((Stat)i).ToString());
				EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(i), label);
			}
			EditorGUI.indentLevel--;

			if (_foldoutOriginal = EditorGUILayout.BeginFoldoutHeaderGroup(_foldoutOriginal, "Original Editor"))
			{
				base.OnInspectorGUI();
			}
			EditorGUILayout.EndFoldoutHeaderGroup();


			this.serializedObject.ApplyModifiedProperties();
		}

		private void DrawStatHeader()
		{
			GUIStyle bigfont = new GUIStyle();
			bigfont.fontSize = 40;
			bigfont.fontStyle = FontStyle.BoldAndItalic;
			bigfont.alignment = TextAnchor.UpperCenter;
			bigfont.fixedHeight = 40f;
			if (EditorGUIUtility.isProSkin)
				bigfont.normal.textColor = Color.white;
			else
				bigfont.normal.textColor = Color.black;

			var content = new GUIContent("Base Stats");

			Rect r = GUILayoutUtility.GetRect(content, bigfont);

			EditorGUI.LabelField(r, content, bigfont);
		}
	}

}