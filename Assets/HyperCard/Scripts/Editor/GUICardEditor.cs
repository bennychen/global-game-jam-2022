/*************************************************************************************************\
// HyperCard
// Author : Bourgot Jean-Louis (Enixion)
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.
\*************************************************************************************************/
using UnityEditor;
using UnityEngine;

namespace HyperCard
{
    public static class GUICardEditor
    {
        public static bool DrawHeader(string text, int level = 0, bool forceOn = false)
        {
            return DrawHeader(text, text, forceOn, level, Color.black);
        }

        public static GUIStyle RichTextBox
        {
            get { return new GUIStyle("box") {richText = true}; }
        }

        public static void StartHeader()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(20f);
            GUILayout.BeginVertical();
            GUILayout.Space(10f);
        }

        public static void EndHeader()
        {
            GUILayout.EndVertical();
            GUILayout.Space(10f);
            GUILayout.EndHorizontal();
            GUILayout.Space(10f);
        }

        public static bool DrawHeader(string text, string key, bool forceOn, int level, Color textColor)
        {
            var state = EditorPrefs.GetBool(key, true);

            if (!forceOn && !state) GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
            GUILayout.BeginHorizontal();

            GUILayout.Space(level * 5);

            var style = new GUIStyle("dragtab");

            GUI.color = textColor;

            text = "<b><size=11>" + text + "</size></b>";
            if (state) text = "\u25BC " + text;
            else text = "\u25BA " + text;

            text = "<color=white>" + text + "</color>";

            if (!GUILayout.Toggle(true, text, style, GUILayout.MinWidth(20f)))
                state = !state;

            GUI.color = Color.white;
            GUI.contentColor = Color.white;

            if (GUI.changed) EditorPrefs.SetBool(key, state);

            GUILayout.Space(2f);
            GUILayout.EndHorizontal();
            GUI.backgroundColor = Color.white;
            if (!forceOn && !state) GUILayout.Space(3f);
            return state;
        }

        public static bool DrawToggle(bool state)
        {
            return DrawToggle("Enable ?", state);
        }

        public static bool DrawToggle(string text, bool state, string tooltip = null)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

            if (!string.IsNullOrEmpty(tooltip))
            {
                GUILayout.Label(new GUIContent(text, tooltip), GUILayout.ExpandWidth(true));
            }
            else
            {
                GUILayout.Label(text, GUILayout.ExpandWidth(true));
            }

            GUI.backgroundColor = !state ? new Color(0.8f, 0.8f, 0.8f) : new Color(0, 0.8f, 0f);

            if (!GUILayout.Toggle(true, state ? "Enabled" : "Disabled", "button", GUILayout.Width(100f)))
                state = !state;

            GUI.backgroundColor = Color.white;

            GUILayout.EndHorizontal();

            GUILayout.Space(20f);

            GUILayout.EndVertical();

            return state;
        }

        public static bool DrawMiniToggle(string text, bool state)
        {
            GUI.backgroundColor = !state ? new Color(0.8f, 0.8f, 0.8f) : new Color(0, 0.8f, 0f);

            if (!GUILayout.Toggle(true, text, "button", GUILayout.Width(50f)))
                state = !state;

            GUI.backgroundColor = Color.white;

            return state;
        }
    }
}
