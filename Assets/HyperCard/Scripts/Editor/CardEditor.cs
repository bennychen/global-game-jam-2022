/*************************************************************************************************\
// HyperCard
// Author : Bourgot Jean-Louis (Enixion)
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.
\*************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;

namespace HyperCard
{
    [CustomEditor(typeof(Card))]
    [CanEditMultipleObjects]
    public class CardEditor : Editor
    {
        private Texture2D _editorLogo;
        private Card _target;
        public IDictionary<string, SerializedProperty> CardSettings = new Dictionary<string, SerializedProperty>();

        public SerializedProperty Properties;

        void OnEnable()
        {
            _editorLogo = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/HyperCard/HyperCard.png", typeof(Texture2D));
            _target = (Card) target;

            Properties = serializedObject.FindProperty("Properties");

            CardSettings = typeof(CardProperties).GetFields()
                .Where(prop => prop.IsDefined(typeof(SerializeField), false))
                .ToDictionary(fieldInfo => fieldInfo.Name, fieldInfo => Properties.FindPropertyRelative(fieldInfo.Name));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.BeginVertical();
            GUILayout.Space(10);
            GUILayout.Box(_editorLogo);
            GUILayout.Space(10);

            if(CardSettings["UseCollection"].boolValue)
            {
                CardSettings["Id"].intValue = EditorGUILayout.IntField("Collection #Id", Math.Max(0, CardSettings["Id"].intValue), GUILayout.ExpandWidth(false));
                
                GUILayout.Space(10);
            }

            if (GUICardEditor.DrawHeader("Card settings"))
            {
                GUICardEditor.StartHeader();

                CardSettings["SideShader"].objectReferenceValue = EditorGUILayout.ObjectField("Side shader",
                    CardSettings["SideShader"].objectReferenceValue, typeof(Shader), false);

                if(CardSettings["CCGKitMode"].boolValue)
                {
                    GUILayout.Space(10);
                    EditorGUILayout.HelpBox("Settings below are controlled by CCGKit", MessageType.Info);
                    EditorGUI.BeginDisabledGroup(true);
                }

                GUILayout.Space(10);

                CardSettings["Stencil"].intValue = Mathf.Min(2 * (EditorGUILayout.IntSlider("Stencil", CardSettings["Stencil"].intValue, 0, 254) / 2), 254);

                //CardSettings["RenderQueue"].intValue = EditorGUILayout.IntField("Render Queue", CardSettings["RenderQueue"].intValue);

                GUILayout.Space(10);

                CardSettings["Seed"].intValue = EditorGUILayout.IntField("Seed", Mathf.Clamp(CardSettings["Seed"].intValue, 0, 99999));

                GUILayout.Space(10);

                CardSettings["Opacity"].floatValue = EditorGUILayout.Slider("Opacity", CardSettings["Opacity"].floatValue, 0.0f, 1.0f);

                if (CardSettings["CCGKitMode"].boolValue)
                {
                    EditorGUI.EndDisabledGroup();
                }

                GUICardEditor.EndHeader();
            }

            GUILayout.Space(10);

            if (GUICardEditor.DrawHeader(string.Format("Custom sprites ({0})", CardSettings["CustomSpriteList"].arraySize), "CustomSprites", false, 0, Color.black))
            {
                GUICardEditor.StartHeader();

                CardSettings["SpritePrefab"].objectReferenceValue = EditorGUILayout.ObjectField("Sprite prefab",
                    CardSettings["SpritePrefab"].objectReferenceValue, typeof(GameObject), false);

                GUILayout.Space(20);

                EditorStyles.textField.wordWrap = true;

                GUILayout.BeginHorizontal();

                if (GUILayout.Button("\u002B New sprite", GUILayout.Width(150), GUILayout.Height(30)))
                {
                    CardSettings["CustomSpriteList"].InsertArrayElementAtIndex(CardSettings["CustomSpriteList"].arraySize);
                    CardSettings["CustomSpriteList"].serializedObject.ApplyModifiedProperties();

                    _target.CreateSprite(CardSettings["CustomSpriteList"].arraySize - 1);
                }

                GUILayout.EndHorizontal();
                GUILayout.BeginVertical();

                for (var i = 0; i < CardSettings["CustomSpriteList"].arraySize; i++)
                {
                    var spriteItem = CardSettings["CustomSpriteList"].GetArrayElementAtIndex(i);

                    var customSpriteParams = typeof(CustomSpriteComponent).GetFields()
                                                .Where(prop => prop.IsDefined(typeof(SerializeField), false))
                                                .ToDictionary(fieldInfo => fieldInfo.Name, fieldInfo => spriteItem.FindPropertyRelative(fieldInfo.Name));

                    GUI.backgroundColor = Color.white;
                    GUILayout.BeginVertical(GUICardEditor.RichTextBox);
                    GUILayout.BeginHorizontal();

                    customSpriteParams["IsActive"].boolValue = GUICardEditor.DrawMiniToggle("Active", customSpriteParams["IsActive"].boolValue);

                    GUI.backgroundColor = new Color(0.8f, 0f, 0f);
                    GUI.color = Color.white;

                    if (GUILayout.Button("\u2573", GUILayout.Width(20), GUILayout.Height(18)))
                    {
                        _target.RemoveSprite(i);

                        CardSettings["CustomSpriteList"].DeleteArrayElementAtIndex(i);
                        CardSettings["CustomSpriteList"].serializedObject.ApplyModifiedProperties();
                    }

                    GUI.backgroundColor = Color.white;
                    GUILayout.EndHorizontal();
                    GUILayout.Space(20);

                    if (i <= CardSettings["CustomSpriteList"].arraySize - 1)
                    {
                        customSpriteParams["Renderer"].objectReferenceValue = EditorGUILayout.ObjectField("Renderer", customSpriteParams["Renderer"].objectReferenceValue, typeof(SpriteRenderer), true);

                        customSpriteParams["Key"].stringValue = EditorGUILayout.TextField("Key", customSpriteParams["Key"].stringValue);
                        customSpriteParams["Texture"].objectReferenceValue = EditorGUILayout.ObjectField("Texture", customSpriteParams["Texture"].objectReferenceValue, typeof(Texture2D), false, GUILayout.ExpandWidth(false));
                        customSpriteParams["Zoom"].floatValue = EditorGUILayout.FloatField("Zoom", customSpriteParams["Zoom"].floatValue);
                        customSpriteParams["Color"].colorValue = EditorGUILayout.ColorField("Color", customSpriteParams["Color"].colorValue);
                        customSpriteParams["Exposure"].floatValue = EditorGUILayout.FloatField("Exposure", customSpriteParams["Exposure"].floatValue);                      

                        GUILayout.Space(20);

                        customSpriteParams["Position"].vector3Value = EditorGUILayout.Vector3Field("Position", customSpriteParams["Position"].vector3Value);
                        customSpriteParams["Scale"].vector2Value = EditorGUILayout.Vector2Field("Scale", customSpriteParams["Scale"].vector2Value);

                        GUILayout.Space(20);

                        customSpriteParams["RestrictToArtwork"].boolValue = GUICardEditor.DrawToggle("Restrict to artwork ?", customSpriteParams["RestrictToArtwork"].boolValue);

                        customSpriteParams["ShowAdvancedSettings"].boolValue = GUICardEditor.DrawMiniToggle("More...", customSpriteParams["ShowAdvancedSettings"].boolValue);

                        if (customSpriteParams["ShowAdvancedSettings"].boolValue)
                        {
                            GUILayout.Space(10);

                            if(!CardSettings["UseSortingGroups"].boolValue)
                            {
                                customSpriteParams["RenderQueue"].intValue = EditorGUILayout.IntField("RenderQueue", customSpriteParams["RenderQueue"].intValue);
                            }

                            GUILayout.Space(20);


                            customSpriteParams["DistortionMask"].objectReferenceValue = EditorGUILayout.ObjectField("Mask", customSpriteParams["DistortionMask"].objectReferenceValue, typeof(Texture2D), false, GUILayout.ExpandWidth(false));
                            customSpriteParams["MaskPosition"].vector2Value = EditorGUILayout.Vector2Field("Position", customSpriteParams["MaskPosition"].vector2Value);
                            customSpriteParams["MaskScale"].vector2Value = EditorGUILayout.Vector2Field("Scale", customSpriteParams["MaskScale"].vector2Value);

                            GUILayout.Space(20);

                            customSpriteParams["DistortionFrequency"].floatValue = EditorGUILayout.FloatField("Distortion frequency", customSpriteParams["DistortionFrequency"].floatValue);
                            customSpriteParams["DistortionAmplitude"].floatValue = EditorGUILayout.FloatField("Amplitude mult.", customSpriteParams["DistortionAmplitude"].floatValue);
                            customSpriteParams["DistortionSpeed"].floatValue = EditorGUILayout.FloatField("Distortion speed", customSpriteParams["DistortionSpeed"].floatValue);
                            customSpriteParams["DistortionDirection"].vector2Value = EditorGUILayout.Vector2Field("Direction", customSpriteParams["DistortionDirection"].vector2Value);

                            GUILayout.Space(20);

                            customSpriteParams["MoveDirection"].vector2Value = EditorGUILayout.Vector2Field("Move direction", customSpriteParams["MoveDirection"].vector2Value);

                            GUILayout.Space(20);

                            customSpriteParams["IsAffectedByFilters"].boolValue = GUICardEditor.DrawToggle("Affected by filters ?", customSpriteParams["IsAffectedByFilters"].boolValue);
                        }
                    }

                    GUILayout.EndVertical();
                    GUILayout.Space(10);
                }

                GUILayout.EndVertical();
                GUICardEditor.EndHeader();
            }

            if (GUICardEditor.DrawHeader(string.Format("Custom texts ({0})", CardSettings["CustomTextList"].arraySize), "CustomTexts", false, 0, Color.black))
            {
                GUICardEditor.StartHeader();
                EditorStyles.textField.wordWrap = true;
                GUILayout.BeginHorizontal();

                if (GUILayout.Button("\u002B Link TMP item", GUILayout.Width(150), GUILayout.Height(30)))
                {
                    CardSettings["CustomTextList"].InsertArrayElementAtIndex(CardSettings["CustomTextList"].arraySize);
                    CardSettings["CustomTextList"].serializedObject.ApplyModifiedProperties();
                }

                GUILayout.EndHorizontal();
                GUILayout.BeginVertical();

                for (var i = 0; i < CardSettings["CustomTextList"].arraySize; i++)
                {
                    var textItem = CardSettings["CustomTextList"].GetArrayElementAtIndex(i);

                    var customTextParams = typeof(CustomTextComponent).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                        .Where(prop => prop.IsDefined(typeof(SerializeField), false))
                        .ToDictionary(fieldInfo => fieldInfo.Name, fieldInfo => textItem.FindPropertyRelative(fieldInfo.Name));

                    customTextParams["Card"].objectReferenceValue = _target;

                    GUI.backgroundColor = Color.white;
                    GUILayout.BeginVertical(GUICardEditor.RichTextBox);
                    GUILayout.BeginHorizontal();

                    var isTextArea = GUICardEditor.DrawMiniToggle("T", (TextMeshProParamDisplayMode) customTextParams["DisplayMode"].intValue == TextMeshProParamDisplayMode.TextArea);
                    customTextParams["DisplayMode"].intValue = isTextArea ? (int) TextMeshProParamDisplayMode.TextArea : (int) TextMeshProParamDisplayMode.Field;

                    customTextParams["IsActive"].boolValue = GUICardEditor.DrawMiniToggle("Active", customTextParams["IsActive"].boolValue);

                    GUI.backgroundColor = new Color(0.8f, 0f, 0f);

                    if (GUILayout.Button("\u2573", GUILayout.Width(20), GUILayout.Height(18)))
                    {
                        CardSettings["CustomTextList"].DeleteArrayElementAtIndex(i);
                        CardSettings["CustomTextList"].serializedObject.ApplyModifiedProperties();
                    }

                    GUI.backgroundColor = Color.white;

                    GUILayout.EndHorizontal();

                    if (i <= CardSettings["CustomTextList"].arraySize - 1)
                    {
                        if (!CardSettings["CCGKitMode"].boolValue)
                        {
                            EditorGUILayout.Popup("Side", customTextParams["TextSide"].intValue, Enum.GetNames(typeof(TextSide)).Select(x => x.ToString()).ToArray(), GUILayout.ExpandWidth(false));
                        }

                        GUILayout.Space(10);

                        customTextParams["TmpObject"].objectReferenceValue = EditorGUILayout.ObjectField("TextMeshPro", customTextParams["TmpObject"].objectReferenceValue, typeof(TextMeshPro), true, GUILayout.ExpandWidth(true));

                        customTextParams["FontAsset"].objectReferenceValue = EditorGUILayout.ObjectField("Font Asset", customTextParams["FontAsset"].objectReferenceValue, typeof(TMP_FontAsset), false, GUILayout.ExpandWidth(true));

                        if (customTextParams["FontAsset"].objectReferenceValue != null)
                        {
                            var materialPresets = TMP_EditorUtility.FindMaterialReferences((TMP_FontAsset) customTextParams["FontAsset"].objectReferenceValue);

                            var matIndex = 0;
                            var material = (Material)(customTextParams["FontMaterial"].objectReferenceValue);

                            if (material != null)
                            {
                                matIndex = Array.IndexOf(materialPresets.Select(x => x.name).ToArray(), material.name);

                                if(matIndex == -1)
                                {
                                    customTextParams["FontMaterial"].objectReferenceValue = null;
                                    matIndex = 0;
                                }
                            }

                            matIndex = EditorGUILayout.Popup("Font Material", matIndex, materialPresets.Select(x => x.name).ToArray(), GUILayout.ExpandWidth(true));

                            customTextParams["FontMaterial"].objectReferenceValue = materialPresets[matIndex];
                        }

                        customTextParams["Key"].stringValue = EditorGUILayout.TextField("Key", customTextParams["Key"].stringValue);
                        customTextParams["_value"].stringValue = isTextArea
                            ? EditorGUILayout.TextArea(customTextParams["_value"].stringValue, GUILayout.Height(100))
                            : EditorGUILayout.TextField(customTextParams["_value"].stringValue);
                    }

                    GUILayout.EndVertical();
                    GUILayout.Space(10);
                }

                GUILayout.EndVertical();
                GUICardEditor.EndHeader();
            }

            GUILayout.Space(10);

            DrawCardSideLayout("Face", new Color(128, 0, 0), CardSettings["FaceSide"]);

            if (!CardSettings["CCGKitMode"].boolValue)
            {
                DrawCardSideLayout("Back", new Color(0, 0, 128), CardSettings["BackSide"]);
            }

            if (GUICardEditor.DrawHeader("Card - FX"))
            {
                GUICardEditor.StartHeader();

                if (GUICardEditor.DrawHeader("Outline", 1))
                {
                    GUICardEditor.StartHeader();

                    CardSettings["IsOutlineEnabled"].boolValue = GUICardEditor.DrawToggle(CardSettings["IsOutlineEnabled"].boolValue);

                    CardSettings["OutlineWidth"].floatValue = EditorGUILayout.Slider("Width", CardSettings["OutlineWidth"].floatValue, 0, 10f);
                    CardSettings["OutlineHeight"].floatValue = EditorGUILayout.Slider("Height", CardSettings["OutlineHeight"].floatValue, 0, 10f);
                    CardSettings["OutlineSmoothness"].floatValue = EditorGUILayout.Slider("Smoothness", CardSettings["OutlineSmoothness"].floatValue, 0.05f, 0.5f);
                    CardSettings["OutlineSmoothSpeed"].floatValue = EditorGUILayout.FloatField("Smooth speed", CardSettings["OutlineSmoothSpeed"].floatValue);
                    CardSettings["OutlineTrimOffset"].floatValue = EditorGUILayout.Slider("Trim offset", CardSettings["OutlineTrimOffset"].floatValue, 0, 1f);
                    CardSettings["OutlinePosOffset"].vector2Value = EditorGUILayout.Vector2Field("Pos. offset", CardSettings["OutlinePosOffset"].vector2Value);

                    GUILayout.Space(10);

                    CardSettings["OutlineStartColor"].colorValue = EditorGUILayout.ColorField(new GUIContent("Start Color"), CardSettings["OutlineStartColor"].colorValue);
                    CardSettings["OutlineEndColor"].colorValue = EditorGUILayout.ColorField(new GUIContent("End Color"), CardSettings["OutlineEndColor"].colorValue);
                    CardSettings["OutlineEndColorDistance"].floatValue = EditorGUILayout.Slider("End color distance", CardSettings["OutlineEndColorDistance"].floatValue, 0, 1);
                    CardSettings["OutlineColorExposure"].floatValue = EditorGUILayout.FloatField("Exposure", CardSettings["OutlineColorExposure"].floatValue);

                    GUILayout.Space(10);

                    CardSettings["OutlineNoiseFrequency"].floatValue = EditorGUILayout.FloatField("Noise freq.", CardSettings["OutlineNoiseFrequency"].floatValue);
                    CardSettings["OutlineNoiseSpeed"].floatValue = EditorGUILayout.FloatField("Noise speed", CardSettings["OutlineNoiseSpeed"].floatValue);
                    CardSettings["OutlineNoiseMult"].floatValue = EditorGUILayout.FloatField("Noise mult.", CardSettings["OutlineNoiseMult"].floatValue);
                    CardSettings["OutlineNoiseOffset"].floatValue = EditorGUILayout.Slider("Noise offset", CardSettings["OutlineNoiseOffset"].floatValue, 0, 1f);
                    CardSettings["OutlineNoiseThreshold"].floatValue = EditorGUILayout.Slider("Noise alpha threshold", CardSettings["OutlineNoiseThreshold"].floatValue, 0, 1);
                    CardSettings["OutlineNoiseDistance"].floatValue = EditorGUILayout.Slider("Noise distance", CardSettings["OutlineNoiseDistance"].floatValue, 0, 1);
                    CardSettings["OutlineNoiseVerticalAjust"].floatValue = EditorGUILayout.Slider("V-ajust", CardSettings["OutlineNoiseVerticalAjust"].floatValue, 0, 5);

                    GUICardEditor.EndHeader();
                }

                if (GUICardEditor.DrawHeader("Dissolve FX", 1))
                {
                    GUICardEditor.StartHeader();

                    CardSettings["IsDissolveEnabled"].boolValue = GUICardEditor.DrawToggle(CardSettings["IsDissolveEnabled"].boolValue);
                    CardSettings["DissolveNoiseFrequency"].floatValue = EditorGUILayout.FloatField("Noise freq.", CardSettings["DissolveNoiseFrequency"].floatValue);
                    CardSettings["DissolveStartColor"].colorValue = EditorGUILayout.ColorField("Inner color", CardSettings["DissolveStartColor"].colorValue);
                    CardSettings["DissolveEndColor"].colorValue = EditorGUILayout.ColorField("Outer color", CardSettings["DissolveEndColor"].colorValue);
                    CardSettings["DissolveColorExposure"].floatValue = EditorGUILayout.FloatField("Exposure", CardSettings["DissolveColorExposure"].floatValue);
                    CardSettings["DissolveOutline"].floatValue = EditorGUILayout.Slider("Burn outline", CardSettings["DissolveOutline"].floatValue, 0.0f, 3f);
                    CardSettings["DissolveAmount"].floatValue = EditorGUILayout.Slider("Burn amount", CardSettings["DissolveAmount"].floatValue, 0f, 1f);
                    CardSettings["DissolveAlphaCut"].floatValue = EditorGUILayout.Slider("Alpha cut", CardSettings["DissolveAlphaCut"].floatValue, 0.1f, 1f);
                    CardSettings["DissolveRotateSpeed"].floatValue = EditorGUILayout.FloatField("Rotation speed", CardSettings["DissolveRotateSpeed"].floatValue);

                    GUICardEditor.EndHeader();
                }

                if (GUICardEditor.DrawHeader("Filters", 1))
                {
                    GUICardEditor.StartHeader();

                    CardSettings["BlackAndWhite"].boolValue = GUICardEditor.DrawToggle("Black & white", CardSettings["BlackAndWhite"].boolValue);

                    GUICardEditor.EndHeader();
                }

                GUICardEditor.EndHeader();
            }

#if CCGKIT_HYPERCARD
            if (GUICardEditor.DrawHeader("Mods"))
            {
                GUICardEditor.StartHeader();

                CardSettings["CCGKitMode"].boolValue = GUICardEditor.DrawToggle("CCGKit Integration", CardSettings["CCGKitMode"].boolValue);

                if (CardSettings["CCGKitMode"].boolValue)
                {
                    EditorGUI.BeginDisabledGroup(true);
                }

                CardSettings["UseCollection"].boolValue = GUICardEditor.DrawToggle("Use Collection", CardSettings["UseCollection"].boolValue || CardSettings["CCGKitMode"].boolValue);

                CardSettings["UseSortingGroups"].boolValue = GUICardEditor.DrawToggle("Use Sorting Groups", CardSettings["UseSortingGroups"].boolValue || CardSettings["CCGKitMode"].boolValue);


                if (CardSettings["CCGKitMode"].boolValue)
                {
                    EditorGUI.EndDisabledGroup();
                }

                GUICardEditor.EndHeader();
            }
#endif

            GUILayout.Space(20);

            GUILayout.EndVertical();

            if (!GUI.changed) return;

            serializedObject.ApplyModifiedProperties();
            Undo.RecordObject(target, "HyperCard change");
            ((Card) target).Redraw();
        }

        private static void DrawCardSideLayout(string name, Color color, SerializedProperty side)
        {
            var sideParams = typeof(CardSide).GetFields()
                .Where(prop => prop.IsDefined(typeof(SerializeField), false))
                .ToDictionary(fieldInfo => fieldInfo.Name, fieldInfo => side.FindPropertyRelative(fieldInfo.Name));

            if (GUICardEditor.DrawHeader(string.Format("{0} settings", name), name, false, 0, color))
            {
                GUICardEditor.StartHeader();

                sideParams["IsEnabled"].boolValue = GUICardEditor.DrawToggle(sideParams["IsEnabled"].boolValue);

                sideParams["Renderer"].objectReferenceValue = EditorGUILayout.ObjectField("Renderer", sideParams["Renderer"].objectReferenceValue, typeof(Renderer), true, GUILayout.ExpandWidth(true));

                GUILayout.Space(10);

                sideParams["FrameMap"].objectReferenceValue = EditorGUILayout.ObjectField("Map", sideParams["FrameMap"].objectReferenceValue, typeof(Texture2D), false, GUILayout.ExpandWidth(false));

                EditorGUILayout.HelpBox("For the moment, only the green channel is used.", MessageType.Info);

                GUILayout.Space(10);

                sideParams["FrameDiffuse"].objectReferenceValue = EditorGUILayout.ObjectField("Diffuse", sideParams["FrameDiffuse"].objectReferenceValue, typeof(Texture2D), false, GUILayout.ExpandWidth(false));
                sideParams["FrameDiffuseColor"].colorValue = EditorGUILayout.ColorField("Diffuse color", sideParams["FrameDiffuseColor"].colorValue);

                GUILayout.Space(10);

                sideParams["Artwork"].objectReferenceValue = EditorGUILayout.ObjectField("Artwork", sideParams["Artwork"].objectReferenceValue, typeof(Texture2D), false, GUILayout.ExpandWidth(false));
                sideParams["ArtworkOffset"].vector2Value = EditorGUILayout.Vector2Field("Offset", sideParams["ArtworkOffset"].vector2Value);
                sideParams["ArtworkScale"].vector2Value = EditorGUILayout.Vector2Field("Scale", sideParams["ArtworkScale"].vector2Value);
                sideParams["ArtworkScaleFactor"].floatValue = EditorGUILayout.FloatField("Scale factor", sideParams["ArtworkScaleFactor"].floatValue);

                GUILayout.Space(20);

                if (GUICardEditor.DrawHeader("Artwork - Distortion FX", name + "ART_DISTORTION", false, 0, Color.black))
                {
                    GUICardEditor.StartHeader();

                    sideParams["IsDistortionEnabled"].boolValue = GUICardEditor.DrawToggle(sideParams["IsDistortionEnabled"].boolValue);
                    sideParams["DistortionMap"].objectReferenceValue = EditorGUILayout.ObjectField("Distortion Mask", sideParams["DistortionMap"].objectReferenceValue, typeof(Texture2D), false, GUILayout.ExpandWidth(false));

                    if (GUICardEditor.DrawHeader("Red channel", "DistRC", false, 1, Color.red))
                    {
                        GUICardEditor.StartHeader();

                        sideParams["DistortionRedFrequency"].floatValue = EditorGUILayout.FloatField("Distortion frequency", sideParams["DistortionRedFrequency"].floatValue);
                        sideParams["DistortionRedAmplitude"].floatValue = EditorGUILayout.FloatField("Amplitude mult.", sideParams["DistortionRedAmplitude"].floatValue);
                        sideParams["DistortionRedSpeed"].floatValue = EditorGUILayout.FloatField("Distortion speed", sideParams["DistortionRedSpeed"].floatValue);
                        sideParams["DistortionRedDirection"].vector2Value = EditorGUILayout.Vector2Field("Direction", sideParams["DistortionRedDirection"].vector2Value);

                        GUICardEditor.EndHeader();
                    }

                    if (GUICardEditor.DrawHeader("Green channel", "DistGC", false, 1, Color.green))
                    {
                        GUICardEditor.StartHeader();

                        sideParams["DistortionGreenFrequency"].floatValue = EditorGUILayout.FloatField("Distortion frequency", sideParams["DistortionGreenFrequency"].floatValue);
                        sideParams["DistortionGreenAmplitude"].floatValue = EditorGUILayout.FloatField("Amplitude mult.", sideParams["DistortionGreenAmplitude"].floatValue);
                        sideParams["DistortionGreenSpeed"].floatValue = EditorGUILayout.FloatField("Distortion speed", sideParams["DistortionGreenSpeed"].floatValue);
                        sideParams["DistortionGreenDirection"].vector2Value = EditorGUILayout.Vector2Field("Direction", sideParams["DistortionGreenDirection"].vector2Value);

                        GUICardEditor.EndHeader();
                    }

                    if (GUICardEditor.DrawHeader("Blue channel", "DistBC", false, 1, Color.blue))
                    {
                        GUICardEditor.StartHeader();

                        sideParams["DistortionBlueFrequency"].floatValue = EditorGUILayout.FloatField("Distortion frequency", sideParams["DistortionBlueFrequency"].floatValue);
                        sideParams["DistortionBlueAmplitude"].floatValue = EditorGUILayout.FloatField("Amplitude mult.", sideParams["DistortionBlueAmplitude"].floatValue);
                        sideParams["DistortionBlueSpeed"].floatValue = EditorGUILayout.FloatField("Distortion speed", sideParams["DistortionBlueSpeed"].floatValue);
                        sideParams["DistortionBlueDirection"].vector2Value = EditorGUILayout.Vector2Field("Direction", sideParams["DistortionBlueDirection"].vector2Value);

                        GUICardEditor.EndHeader();
                    }

                    GUICardEditor.EndHeader();
                }

                if (GUICardEditor.DrawHeader("Artwork - Sprites Sheet FX", name + "ART_SS", false, 0, Color.black))
                {
                    GUICardEditor.StartHeader();

                    sideParams["IsSpriteSheetEnabled"].boolValue = GUICardEditor.DrawToggle(sideParams["IsSpriteSheetEnabled"].boolValue);
                    sideParams["SpriteSheetTexture"].objectReferenceValue = (Texture2D)EditorGUILayout.ObjectField("Sprites sheet", sideParams["SpriteSheetTexture"].objectReferenceValue, typeof(Texture2D), false, GUILayout.ExpandWidth(false));
                    sideParams["SpriteSheetSize"].vector2Value = EditorGUILayout.Vector2Field("Sheet size", sideParams["SpriteSheetSize"].vector2Value);

                    EditorGUILayout.HelpBox("Size corresponds to the number of columns by the number of rows.", MessageType.Info);

                    sideParams["SpriteSheetRemoveBlack"].boolValue = GUICardEditor.DrawToggle("Remove black background", sideParams["SpriteSheetRemoveBlack"].boolValue);
                    sideParams["SpriteSheetSpeed"].floatValue = EditorGUILayout.FloatField("Speed", sideParams["SpriteSheetSpeed"].floatValue);
                    sideParams["SpriteSheetOffsetX"].floatValue = EditorGUILayout.Slider("Offset X", sideParams["SpriteSheetOffsetX"].floatValue, 0, 1);
                    sideParams["SpriteSheetOffsetY"].floatValue = EditorGUILayout.Slider("Offset Y", sideParams["SpriteSheetOffsetY"].floatValue, 0, 1);
                    sideParams["SpriteSheetColor"].colorValue = EditorGUILayout.ColorField("Color", sideParams["SpriteSheetColor"].colorValue);
                    sideParams["SpriteSheetScale"].vector2Value = EditorGUILayout.Vector2Field("Scale", sideParams["SpriteSheetScale"].vector2Value);

                    GUICardEditor.EndHeader();
                }

                if (GUICardEditor.DrawHeader("Side - Holo/Cubemap FX", name + "SIDE_HOLOC", false, 0, Color.black))
                {
                    GUICardEditor.StartHeader();

                    sideParams["IsHoloFxEnabled"].boolValue = GUICardEditor.DrawToggle(sideParams["IsHoloFxEnabled"].boolValue);
                    sideParams["HoloUseArtworkCoords"].boolValue = GUICardEditor.DrawToggle("Use Artwork coords", sideParams["HoloUseArtworkCoords"].boolValue);
                    sideParams["HoloMask"].objectReferenceValue = EditorGUILayout.ObjectField("Mask", sideParams["HoloMask"].objectReferenceValue, typeof(Texture2D), false, GUILayout.ExpandWidth(false));
                    sideParams["HoloCube"].objectReferenceValue = EditorGUILayout.ObjectField("Cubemap", sideParams["HoloCube"].objectReferenceValue, typeof(Cubemap), false, GUILayout.ExpandWidth(false));
                    sideParams["HoloCubeColor"].colorValue = EditorGUILayout.ColorField("Color", sideParams["HoloCubeColor"].colorValue);
                    sideParams["HoloCubeRotation"].floatValue = EditorGUILayout.Slider("Cubemap Rotation", sideParams["HoloCubeRotation"].floatValue, 0, 360);
                    sideParams["HoloAlpha"].floatValue = EditorGUILayout.Slider("Alpha mult.", sideParams["HoloAlpha"].floatValue, 0, 1);
                    sideParams["HoloCubeContrast"].floatValue = EditorGUILayout.FloatField("Contrast", sideParams["HoloCubeContrast"].floatValue);
                    sideParams["HoloCubeBoundingBoxScale"].vector3Value = EditorGUILayout.Vector3Field("Bounding box scale", sideParams["HoloCubeBoundingBoxScale"].vector3Value);
                    sideParams["HoloCubeBoundingBoxOffset"].vector3Value = EditorGUILayout.Vector3Field("Bounding box offset", sideParams["HoloCubeBoundingBoxOffset"].vector3Value);

                    GUILayout.Space(20);

                    sideParams["HoloMap"].objectReferenceValue = EditorGUILayout.ObjectField("Holo. Texture", sideParams["HoloMap"].objectReferenceValue, typeof(Texture2D), false, GUILayout.ExpandWidth(false));
                    sideParams["HoloMapScale"].vector2Value = EditorGUILayout.Vector2Field("Holo. Texure scale", sideParams["HoloMapScale"].vector2Value);
                    sideParams["HoloPower"].floatValue = EditorGUILayout.Slider("Holo. Power", sideParams["HoloPower"].floatValue, 0f, 5f);

                    GUILayout.Space(20);

                    sideParams["ShowHoloGuizmo"].boolValue = GUICardEditor.DrawToggle("Debug", sideParams["ShowHoloGuizmo"].boolValue);
                    
                    GUICardEditor.EndHeader();
                }

                if (GUICardEditor.DrawHeader("Side - Glitter FX", name + "SIDE_GLITTER", false, 0, Color.black))
                {
                    GUICardEditor.StartHeader();

                    sideParams["IsGlitterFxEnabled"].boolValue = GUICardEditor.DrawToggle(sideParams["IsGlitterFxEnabled"].boolValue);
                    sideParams["GlitterUseArtworkCoords"].boolValue = GUICardEditor.DrawToggle("Use Artwork coords", sideParams["GlitterUseArtworkCoords"].boolValue);
                    sideParams["GlitterColor"].colorValue = EditorGUILayout.ColorField("Glitter Color", sideParams["GlitterColor"].colorValue);
                    sideParams["GlitterContrast"].floatValue = EditorGUILayout.FloatField("Contrast", sideParams["GlitterContrast"].floatValue);
                    sideParams["GlitterPower"].floatValue = EditorGUILayout.Slider("Power", sideParams["GlitterPower"].floatValue, 0f, 1f);
                    sideParams["GlitterSize"].floatValue = EditorGUILayout.Slider("Size", sideParams["GlitterSize"].floatValue, 0f, 1f);
                    sideParams["GlitterSpeed"].floatValue = EditorGUILayout.Slider("Speed", sideParams["GlitterSpeed"].floatValue, 0f, 1f);

                    GUILayout.Space(20);

                    sideParams["GlitterMask"].objectReferenceValue = EditorGUILayout.ObjectField("Mask", sideParams["GlitterMask"].objectReferenceValue, typeof(Texture2D), false, GUILayout.ExpandWidth(false));
                    sideParams["GlitterMaskScale"].vector2Value = EditorGUILayout.Vector2Field("Mask scale", sideParams["GlitterMaskScale"].vector2Value);

                    GUILayout.Space(20);

                    sideParams["GlitterBackTex"].objectReferenceValue = EditorGUILayout.ObjectField("Back diffuse", sideParams["GlitterBackTex"].objectReferenceValue, typeof(Texture2D), false, GUILayout.ExpandWidth(false));
                    sideParams["GlitterBackTexScale"].vector2Value = EditorGUILayout.Vector2Field("Back scale", sideParams["GlitterBackTexScale"].vector2Value);
                    sideParams["GlitterBackContrast"].floatValue = EditorGUILayout.Slider("Back Contrast", sideParams["GlitterBackContrast"].floatValue, 0.1f, 2f);
                    sideParams["GlitterBackPower"].floatValue = EditorGUILayout.Slider("Back Power", sideParams["GlitterBackPower"].floatValue, 0f, 1f);

                    GUILayout.Space(20);

                    sideParams["GlitterLight"].floatValue = EditorGUILayout.Slider("Specular light", sideParams["GlitterLight"].floatValue, 0f, 5f);
                    sideParams["GlitterLightColor"].colorValue = EditorGUILayout.ColorField("Spec. light color", sideParams["GlitterLightColor"].colorValue);
                    sideParams["GlitterLightRadius"].floatValue = EditorGUILayout.Slider("Spec. light radius", sideParams["GlitterLightRadius"].floatValue, 0.1f, 5f);

                    GUILayout.Space(20);

                    sideParams["GlitterOpacity"].floatValue = EditorGUILayout.Slider("Opacity", sideParams["GlitterOpacity"].floatValue, 0f, 1f);

                    GUICardEditor.EndHeader();
                }

                if (GUICardEditor.DrawHeader("Side - Color FX"))
                {
                    GUICardEditor.StartHeader();

                    sideParams["IsSideColorEnabled"].boolValue = GUICardEditor.DrawToggle(sideParams["IsSideColorEnabled"].boolValue);
                    sideParams["SideColor"].colorValue = EditorGUILayout.ColorField("Side color", sideParams["SideColor"].colorValue);

                    GUILayout.Space(20);

                    sideParams["SideColorOverrideTextTags"].boolValue = GUICardEditor.DrawToggle("Override text tags", sideParams["SideColorOverrideTextTags"].boolValue);

                    GUICardEditor.EndHeader();
                }


                GUICardEditor.EndHeader();
            }
        }

        [UnityEditor.Callbacks.PostProcessScene]
        private static void OnPostProcessScene()
        {
            ReloadAllCards();
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            ReloadAllCards();
        }

        public static void ReloadAllCards()
        {
            Card[] cards = (Card[])FindObjectsOfType(typeof(Card));

            foreach (var card in cards)
            {
                card.Redraw();
            }
        }
    }
}
 