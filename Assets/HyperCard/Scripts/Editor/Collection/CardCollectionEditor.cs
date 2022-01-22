/*************************************************************************************************\
// HyperCard
// Author : Bourgot Jean-Louis (Enixion)
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.
\*************************************************************************************************/
#if CCGKIT_HYPERCARD
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HyperCard
{
    public class CardCollectionEditor : EditorWindow
    {
        private Texture2D _editorLogo;
        public CardCollection Collection;
        public static string PrefabPath = "Assets/HyperCard/Data";
        public static string CollectionPath = "Assets/HyperCard/Data/Collection.asset";

        [MenuItem("HyperCard/Collection")]
        static void Init()
        {
            var window = GetWindow(typeof(CardCollectionEditor));
            window.maxSize = new Vector2(335, 400);
            window.minSize = window.maxSize;
            window.titleContent = new GUIContent("Collection");
            window.Show();
        }

        void OnEnable()
        {
            _editorLogo = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/HyperCard/HyperCard.png", typeof(Texture2D));

            Collection = AssetDatabase.LoadAssetAtPath(CollectionPath, typeof(CardCollection)) as CardCollection;
        }

        Vector2 scrollPos;
        Vector2 scrollPosEdit;

        void OnGUI()
        {
            GUILayout.BeginVertical();

            GUILayout.Space(10);
            GUILayout.Box(_editorLogo);
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("HyperCard Cards Collection", EditorStyles.boldLabel);
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            var window = GetWindow(typeof(CardCollectionEditor));

            if (Collection != null)
            {
                GUILayout.BeginVertical();

                scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Width(window.maxSize.x), GUILayout.Height(window.maxSize.y - 200));

                foreach (var card in Collection.Cards.OrderBy(x => x.GetComponent<Card>().Properties.Id).ToList())
                {
                    if(card == null)
                    {
                        Collection.Cards.Remove(card);
                        EditorUtility.SetDirty(Collection);
                    }

                    GUI.backgroundColor = Color.white;
                    GUILayout.BeginVertical(GUICardEditor.RichTextBox);
                    GUILayout.BeginHorizontal();

                    //EditorGUI.BeginDisabledGroup(true);
                    GUILayout.Box(card.GetComponent<Card>().Properties.FaceSide.Artwork, GUILayout.Width(50), GUILayout.Height(50));
                    GUILayout.Label("Collection #Id : " + card.GetComponent<Card>().Properties.Id);
                    //EditorGUI.EndDisabledGroup();

                    GUILayout.EndHorizontal();

                    GUILayout.Space(10);

                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button("Instanciate", GUILayout.Width(100)))
                    {
                        var cardClone = Instantiate(card, Vector3.zero, Quaternion.identity);
                        cardClone.name = card.name;
                    }

                    GUI.backgroundColor = new Color(0.8f, 0f, 0f);
                    var style = new GUIStyle(GUI.skin.button);
                    style.normal.textColor = Color.white;

                    if (GUILayout.Button("\u2573 Remove", style, GUILayout.Width(100)))
                    {
                        if (EditorUtility.DisplayDialog("REMOVE",
                           "Delete object from Collection (prefab will remain in folder) ?",
                           "DELETE !",
                           "No !"))
                        {
                            Collection.Cards.Remove(card);
                            EditorUtility.SetDirty(Collection);
                            return;
                        }
                    }

                    GUI.backgroundColor = Color.white;

                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                }

                GUILayout.Space(20);

                GUILayout.EndScrollView();

                GUILayout.Space(20);

                GUILayout.BeginVertical();

                if (GUILayout.Button("Add current", GUILayout.Width(100)))
                {
                    if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Card>() != null)
                    {
                        var card = Selection.activeGameObject;

                        if (!card.GetComponent<Card>().Properties.UseCollection)
                        {
                            EditorUtility.DisplayDialog("Error", "The Collection Mode isn't enabled on this card.", "Ok");
                            return;
                        }

                        if (Collection.Cards.Any(x => x.GetComponent<Card>().Properties.Id == card.GetComponent<Card>().Properties.Id))
                        {
                            if(!EditorUtility.DisplayDialog("Card already exists",
                               "A card with the same Id already exists in the Collection. Do you want to overwrite it ?",
                               "Yes",
                               "No, keep it!"))
                            {
                                return;
                            }
                        }

                        AddToCollection(card);
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Error", "Please select a GO or a prefab with the HyperCard component.", "Ok");
                        return;
                    }
                }

                GUILayout.EndVertical();

                GUILayout.Space(10);

                GUILayout.EndVertical();

                if (GUI.changed)
                {
                    EditorUtility.SetDirty(Collection);
                }
            }
            else
            {
                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Create Collection"))
                {
                    CreateCollection();
                    EditorUtility.FocusProjectWindow();
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
        }

        private void CreateCollection()
        {
            var collection = CreateInstance<CardCollection>();

            AssetDatabase.CreateAsset(collection, CollectionPath);
            AssetDatabase.SaveAssets();
        }

        private void AddToCollection(GameObject prefab)
        {
            Collection.Cards.RemoveAll(x => x.GetComponent<Card>().Properties.Id == prefab.GetComponent<Card>().Properties.Id);

            Debug.Log(PrefabUtility.GetCorrespondingObjectFromSource(prefab));

            if(PrefabUtility.GetCorrespondingObjectFromSource(prefab) == null && 
                prefab.hideFlags != HideFlags.HideInHierarchy)
            {
                prefab = PrefabUtility.CreatePrefab(Path.Combine(PrefabPath, prefab.GetComponent<Card>().Properties.Id + ".prefab").Replace("\\", "/"), prefab);
            }

            Collection.Cards.Add(prefab);

            EditorUtility.SetDirty(Collection);
        }

        void OnInspectorUpdate()
        {
            Repaint();
        }
    }
}
#endif