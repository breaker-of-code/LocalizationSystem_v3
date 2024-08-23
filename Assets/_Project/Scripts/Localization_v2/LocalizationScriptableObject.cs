using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LocalizationEntry
{
    public string key;
    public string translation;
}

[System.Serializable]
public class LanguageData
{
    public string languageName;
    public List<LocalizationEntry> entries = new List<LocalizationEntry>();
}

[System.Serializable]
public class LocalizationData
{
    public List<LanguageData> languages = new List<LanguageData>();
}

[CreateAssetMenu(fileName = "NewLocalization", menuName = "Localization/Language")]
public class LocalizationScriptableObject : ScriptableObject
{
    public List<LocalizationEntry> entries = new List<LocalizationEntry>();

    public void AddEntry(string key, string translation)
    {
        LocalizationEntry entry = entries.Find(e => e.key == key);
        if (entry != null)
        {
            entry.translation = translation;
        }
        else
        {
            entries.Add(new LocalizationEntry { key = key, translation = translation });
        }
    }

    public List<LocalizationEntry> SearchEntries(string searchTerm)
    {
        return entries.FindAll(e => e.key.Contains(searchTerm) || e.translation.Contains(searchTerm));
    }

    // Import localization data from JSON using JsonUtility
    public void ImportFromJson(string json)
    {
        LocalizationData data = JsonUtility.FromJson<LocalizationData>(json);

        foreach (var languageData in data.languages)
        {
            foreach (var entry in languageData.entries)
            {
                AddEntry(entry.key, entry.translation);
            }
        }
    }

    // Export localization data to JSON using JsonUtility
    public string ExportToJson()
    {
        LocalizationData data = new LocalizationData();

        foreach (var entry in entries)
        {
            LanguageData languageData = data.languages.Find(l => l.languageName == name.Replace("Localization", ""));
            if (languageData == null)
            {
                languageData = new LanguageData { languageName = name.Replace("Localization", "") };
                data.languages.Add(languageData);
            }

            languageData.entries.Add(new LocalizationEntry { key = entry.key, translation = entry.translation });
        }

        return JsonUtility.ToJson(data, true);
    }
}
