using UnityEngine;
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.Events;

public enum Language
{
    English,
    Chinese,
}

public class LocalizationManager : MonoBehaviour
{
    static LocalizationManager instance;
    public static LocalizationManager Instance => instance;
    [HideInInspector] public UnityEvent OnLanguageChanged;
    Dictionary<Language, TextAsset> localizationFiles = new();
    Dictionary<string, string> localizationData = new();
    [SerializeField] private Language language;

    private void Awake()
    {
        CreateSingleton();

        SetupLocalizationFiles();
        SetupLocalizationLanguage();
    }

    void CreateSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void SetupLocalizationFiles()
    {
        // Debug.Log("Setting up Localization Files");
        foreach (Language language in Enum.GetValues(typeof(Language)))
        {
            string textAssetPath = "Localization/" + language;
            TextAsset textAsset = Resources.Load(textAssetPath) as TextAsset;

            if (textAsset != null)
            {
                localizationFiles[language] = textAsset;
                // Debug.Log("Text Asset Added for: " + language);
            }
            else
            {
                Debug.LogError("No TextAsset found for: " + language);
            }
        }
    }

    void SetupLocalizationLanguage()
    {
        localizationData.Clear();

        TextAsset textAsset;

        if (localizationFiles.ContainsKey(language))
        {
            // Debug.Log("Loaded Initialization Data for: " + language);
            textAsset = localizationFiles[language];
        }
        else
        {
            Debug.LogError("Language Text Asset not found for: " + language + ". Using English instead");
            language = Language.English;
            textAsset = localizationFiles[Language.English];
        }

        XmlDocument xmlDocument = new();
        xmlDocument.LoadXml(textAsset.text);

        XmlNodeList entryList = xmlDocument.GetElementsByTagName("Entry");

        string key = "";
        string value = "";

        foreach (XmlNode entryNode in entryList)
        {
            key = entryNode.FirstChild.InnerText;
            value = entryNode.LastChild.InnerText;

            if (!localizationData.ContainsKey(key))
            {
                // Debug.Log("Adding Key: " + key + " with Value: " + value);
                localizationData.Add(key, value);
            }
            else
            {
                Debug.LogError("Already found entry for Key: " + key);
            }
        }
    }

    public string GetLocString(string key)
    {
        if (localizationData.ContainsKey(key))
        {
            return localizationData[key];
        }

        return "Error Loc Missing: " + key;
    }

    public void NextLanguage()
    {
        int next = (int)language + 1;
        if (next >= Enum.GetValues(typeof(Language)).Length)
            next = 0;

        ChangeLanguage(((Language)next).ToString());
    }

    public void ChangeLanguage(string targetLanguage)
    {
        foreach (Language language in Enum.GetValues(typeof(Language)))
        {
            if (language.ToString() == targetLanguage)
            {
                this.language = language;
                SetupLocalizationLanguage();
                OnLanguageChanged.Invoke();
                break;
            }
        }
    }
}
