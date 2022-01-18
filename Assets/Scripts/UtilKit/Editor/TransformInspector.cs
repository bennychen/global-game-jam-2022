#if !UNITY_3_5
using UnityEngine;
using UnityEditor;

namespace Codeplay
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Transform))]
    public class TransformInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUIUtility.labelWidth = 15f;

            serializedObject.Update();

            DrawPosition();
            DrawRotation();
            DrawScale();

            serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable()
        {
            _positionProperty = serializedObject.FindProperty("m_LocalPosition");
            _rotationProperty = serializedObject.FindProperty("m_LocalRotation");
            _scaleProperty = serializedObject.FindProperty("m_LocalScale");
        }

        private void DrawPosition()
        {
            GUILayout.BeginHorizontal();
            {
                bool reset = GUILayout.Button("P", GUILayout.Width(20f));

                EditorGUILayout.PropertyField(_positionProperty.FindPropertyRelative("x"));
                EditorGUILayout.PropertyField(_positionProperty.FindPropertyRelative("y"));
                EditorGUILayout.PropertyField(_positionProperty.FindPropertyRelative("z"));

                if (reset) _positionProperty.vector3Value = Vector3.zero;
            }
            GUILayout.EndHorizontal();
        }

        private void DrawScale()
        {
            GUILayout.BeginHorizontal();
            {
                bool reset = GUILayout.Button("S", GUILayout.Width(20f));

                EditorGUILayout.PropertyField(_scaleProperty.FindPropertyRelative("x"));
                EditorGUILayout.PropertyField(_scaleProperty.FindPropertyRelative("y"));
                EditorGUILayout.PropertyField(_scaleProperty.FindPropertyRelative("z"));

                if (reset) _scaleProperty.vector3Value = Vector3.one;
            }
            GUILayout.EndHorizontal();
        }

        private enum Axes : int
        {
            None = 0,
            X = 1,
            Y = 2,
            Z = 4,
            All = 7,
        }

        private Axes CheckDifference(Transform t, Vector3 original)
        {
            Vector3 next = t.localEulerAngles;

            Axes axes = Axes.None;

            if (Differs(next.x, original.x)) axes |= Axes.X;
            if (Differs(next.y, original.y)) axes |= Axes.Y;
            if (Differs(next.z, original.z)) axes |= Axes.Z;

            return axes;
        }

        private Axes CheckDifference(SerializedProperty property)
        {
            Axes axes = Axes.None;

            if (property.hasMultipleDifferentValues)
            {
                Vector3 original = property.quaternionValue.eulerAngles;

                foreach (Object obj in serializedObject.targetObjects)
                {
                    axes |= CheckDifference(obj as Transform, original);
                    if (axes == Axes.All) break;
                }
            }
            return axes;
        }

        private static bool FloatField(string name, ref float value, bool hidden, bool greyedOut, GUILayoutOption opt)
        {
            float newValue = value;
            GUI.changed = false;

            if (!hidden)
            {
                if (greyedOut)
                {
                    GUI.color = new Color(0.7f, 0.7f, 0.7f);
                    newValue = EditorGUILayout.FloatField(name, newValue, opt);
                    GUI.color = Color.white;
                }
                else
                {
                    newValue = EditorGUILayout.FloatField(name, newValue, opt);
                }
            }
            else if (greyedOut)
            {
                GUI.color = new Color(0.7f, 0.7f, 0.7f);
                float.TryParse(EditorGUILayout.TextField(name, "--", opt), out newValue);
                GUI.color = Color.white;
            }
            else
            {
                float.TryParse(EditorGUILayout.TextField(name, "--", opt), out newValue);
            }

            if (GUI.changed && Differs(newValue, value))
            {
                value = newValue;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Because Mathf.Approximately is too sensitive.
        /// </summary>
        private static bool Differs(float a, float b) { return Mathf.Abs(a - b) > 0.0001f; }

        private void DrawRotation()
        {
            GUILayout.BeginHorizontal();
            {
                bool reset = GUILayout.Button("R", GUILayout.Width(20f));

                Vector3 visible = (serializedObject.targetObject as Transform).localEulerAngles;
                Axes changed = CheckDifference(_rotationProperty);
                Axes altered = Axes.None;

                GUILayoutOption opt = GUILayout.MinWidth(30f);

                if (FloatField("X", ref visible.x, (changed & Axes.X) != 0, false, opt)) altered |= Axes.X;
                if (FloatField("Y", ref visible.y, (changed & Axes.Y) != 0, false, opt)) altered |= Axes.Y;
                if (FloatField("Z", ref visible.z, (changed & Axes.Z) != 0, false, opt)) altered |= Axes.Z;

                if (reset)
                {
                    _rotationProperty.quaternionValue = Quaternion.identity;
                }
                else if (altered != Axes.None)
                {
                    foreach (Object obj in serializedObject.targetObjects)
                    {
                        Transform t = obj as Transform;
                        Vector3 v = t.localEulerAngles;

                        if ((altered & Axes.X) != 0) v.x = visible.x;
                        if ((altered & Axes.Y) != 0) v.y = visible.y;
                        if ((altered & Axes.Z) != 0) v.z = visible.z;

                        t.localEulerAngles = v;
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        private SerializedProperty _positionProperty;
        private SerializedProperty _rotationProperty;
        private SerializedProperty _scaleProperty;
    }
#endif
}
