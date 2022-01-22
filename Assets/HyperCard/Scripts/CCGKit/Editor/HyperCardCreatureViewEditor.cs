/*************************************************************************************************\
// HyperCard
// Author : Bourgot Jean-Louis (Enixion)
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.
\*************************************************************************************************/
#if CCGKIT_HYPERCARD
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HyperCardCreatureView))]
[CanEditMultipleObjects]
public class CardEditor : Editor
{
    private Texture2D _editorLogo;

    public SerializedProperty HyperCardComponent;

    void OnEnable()
    {
        _editorLogo = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/HyperCard/HyperCard-CCG.png", typeof(Texture2D));
        HyperCardComponent = serializedObject.FindProperty("HyperCardComponent");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.BeginVertical();
        GUILayout.Space(10);
        GUILayout.Box(_editorLogo);
        GUILayout.Space(10);

        HyperCardComponent.objectReferenceValue = EditorGUILayout.ObjectField("HyperCard Compo.", HyperCardComponent.objectReferenceValue, typeof(HyperCard.Card), true);

        GUILayout.Space(10);

        GUILayout.EndVertical();

        if (!GUI.changed) return;

        serializedObject.ApplyModifiedProperties();
        Undo.RecordObject(target, "HyperCard change");
    }
}
#endif