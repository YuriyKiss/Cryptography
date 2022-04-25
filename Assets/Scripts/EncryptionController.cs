using SFB;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

using System.IO;
using System.Collections;

public class EncryptionController : MonoBehaviour
{
    [Header("Fields")]
    public TMP_InputField encryptionField;
    public TMP_InputField decryptionField;

    [Header("Encryption Selection")]
    public TMP_Dropdown encryptionSelector;
    public GameObject encryptionSettings;

    [Header("Buttons")]
    public Button encryptButton;
    public Button decryptButton;

    [Header("User messaging")]
    public TMP_Text messageField;
    public float warningTime;
    public float authorMessageTime;

    private IEncryption encryption;

    public void OnEncryptButtonPressed()
    {
        ClearWarningMessage();
        string message = encryptionField.text;

        TMP_InputField[] optionInputFields = encryptionSettings.GetComponentsInChildren<TMP_InputField>(false);

        string[] options = new string[optionInputFields.Length];
        for (int i = 0; i < optionInputFields.Length; ++i)
        {
            options[i] = optionInputFields[i].text;
        }

        string validationMessage = encryption.Validate(message, options);

        if (validationMessage == "")
        {
            encryptionField.text = "";
            decryptionField.text = encryption.Encrypt(message, options);
        }
        else
        {
            StartCoroutine(SendUserMessage(validationMessage, Color.red, warningTime));
        }
    }

    public void OnDecryptButtonPressed()
    {
        ClearWarningMessage();
        string message = decryptionField.text;

        TMP_InputField[] optionInputFields = encryptionSettings.GetComponentsInChildren<TMP_InputField>(false);

        string[] options = new string[optionInputFields.Length];
        for (int i = 0; i < optionInputFields.Length; ++i)
        {
            options[i] = optionInputFields[i].text;
        }

        string validationMessage = encryption.Validate(message, options);

        if (validationMessage == "")
        {
            decryptionField.text = "";
            encryptionField.text = encryption.Decrypt(message, options);
        }
        else
        {
            StartCoroutine(SendUserMessage(validationMessage, Color.red, warningTime));
        }
    }

    public void OnSelectorValueChanged()
    {
        ClearWarningMessage();
        DisableAllEncryptionSettings();

        int selectorValue = encryptionSelector.value;

        switch (selectorValue)
        {
            case 1:
                encryptButton.interactable = true;
                decryptButton.interactable = true;

                SetEncryptionSettings(selectorValue);
                encryption = new CaesarsCipher();
                break;
            case 0:
                encryptButton.interactable = false;
                decryptButton.interactable = false;
                break;
        }
    }

    private void SetEncryptionSettings(int value)
    {
        Transform encryptionSettingsTransform = encryptionSettings.transform;

        Transform optionTransform = encryptionSettingsTransform.GetChild(value - 1);
        GameObject optionGameObject = optionTransform.gameObject;

        // Clearing all previous inputs before enabling
        TMP_InputField[] optionInputFields = optionGameObject.GetComponentsInChildren<TMP_InputField>();
        foreach(TMP_InputField inputField in optionInputFields)
        {
            inputField.text = "";
        }

        // Activating selected option
        optionGameObject.SetActive(true);
    }

    private void DisableAllEncryptionSettings()
    {
        Transform encryptionSettingsTransform = encryptionSettings.transform;

        foreach (Transform child in encryptionSettingsTransform)
        {
            child.gameObject.SetActive(false);
        }

        encryption = null;
    }

    private void ClearWarningMessage()
    {
        StopAllCoroutines();
        messageField.text = "";
    }

    public IEnumerator SendUserMessage(string message, Color color, float time)
    {
        messageField.color = color;
        messageField.text = message;
        yield return new WaitForSecondsRealtime(time);
        messageField.text = "";
    }

    public void OpenFileEncryption()
    {
        var extensions = new[] {
            new ExtensionFilter("Text Files", "txt" ),
            new ExtensionFilter("All Files", "*" ),
            };

        string[] path = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);

        if (path == null)
        {
            StartCoroutine(SendUserMessage("Please select file", Color.yellow, warningTime));
            return;
        }

        StreamReader reader = new StreamReader(path[0]);
        encryptionField.text = reader.ReadToEnd();
        reader.Close();
    }

    public void SaveFileEncryption()
    {
        var extensionList = new[] {
            new ExtensionFilter("Text", "txt")
        };

        string path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "New File", extensionList);

        if (path == "")
        {
            StartCoroutine(SendUserMessage("Please select file", Color.yellow, warningTime));
            return;
        }

        StreamWriter writer = new StreamWriter(path, true);
        writer.Write(encryptionField.text);
        writer.Close();
    }

    public void OpenFileDecryption()
    {
        var extensions = new[] {
            new ExtensionFilter("Text Files", "txt" ),
            new ExtensionFilter("All Files", "*" ),
            };

        string[] path = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);

        if (path == null)
        {
            StartCoroutine(SendUserMessage("Please select file", Color.yellow, warningTime));
            return;
        }

        StreamReader reader = new StreamReader(path[0]);
        decryptionField.text = reader.ReadToEnd();
        reader.Close();
    }

    public void SaveFileDecryption()
    {
        var extensionList = new[] {
            new ExtensionFilter("Text", "txt")
        };

        string path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "New File", extensionList);

        if (path == "")
        {
            StartCoroutine(SendUserMessage("Please select file", Color.yellow, warningTime));
            return;
        }

        StreamWriter writer = new StreamWriter(path, true);
        writer.Write(decryptionField.text);
        writer.Close();
    }

    public void ShowAuthor()
    {
        ClearWarningMessage();
        StartCoroutine(SendUserMessage("Made by Kis Yuriy\nPMi-33", Color.black, authorMessageTime));
    }

    public void ExitApplication() => Application.Quit();
}
