using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LocalizationScriptableObject))]
public class LocalizationScriptableObjectEditor : Editor
{
    private string searchKeyTerm = "";
    private string searchTranslationTerm = "";
    private LocalizationEntry searchResult = null;
    private bool hasSearched = false;

    public override void OnInspectorGUI()
    {
        LocalizationScriptableObject localization = (LocalizationScriptableObject)target;

        GUILayout.Label("Search by Key", EditorStyles.boldLabel);
        searchKeyTerm = EditorGUILayout.TextField("Key", searchKeyTerm);

        GUILayout.Label("Search by Translation", EditorStyles.boldLabel);
        searchTranslationTerm = EditorGUILayout.TextField("Translation", searchTranslationTerm);

        if (GUILayout.Button("Search"))
        {
            SearchInLocalization(localization);
            hasSearched = true;
        }

        EditorGUILayout.Space();

        if (hasSearched)
        {
            if (searchResult != null)
            {
                EditorGUILayout.LabelField("Search Result", EditorStyles.boldLabel);
                searchResult.key = EditorGUILayout.TextField("Key", searchResult.key);
                searchResult.translation = EditorGUILayout.TextField("Translation", searchResult.translation);

                if (GUILayout.Button("Save"))
                {
                    EditorUtility.SetDirty(target);
                    AssetDatabase.SaveAssets();
                }
            }
            else
            {
                EditorGUILayout.LabelField("No results found.");
            }
        }

        EditorGUILayout.Space();
        GUILayout.Label("All Localizations", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("entries"), true);

        serializedObject.ApplyModifiedProperties();
    }

    private void SearchInLocalization(LocalizationScriptableObject localization)
    {
        searchResult = null;

        foreach (var entry in localization.entries)
        {
            if ((!string.IsNullOrEmpty(searchKeyTerm) && entry.key.Contains(searchKeyTerm)) ||
                (!string.IsNullOrEmpty(searchTranslationTerm) && entry.translation.Contains(searchTranslationTerm)))
            {
                searchResult = entry;
                break;
            }
        }
    }
}
