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

    public Toggle frequncyDistribution;

    [Header("Buttons")]
    public Button encryptButton;
    public Button decryptButton;

    [Header("User messaging")]
    public TMP_Text messageField;
    public float warningTime;
    public float authorMessageTime;

    private IEncryption encryption;

    #region Encryption/Decryption
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
            decryptionField.text = encryption.Encrypt(message, options);

            // Frequency distribution
            Toggle toggle = GetComponentInChildren<Toggle>(false);
            if (toggle != null && toggle.isOn)
            {
                SaveFile(FrequencyDistribution.Distribute(encryptionField.text, decryptionField.text));
            }

            encryptionField.text = "";
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
    #endregion

    #region Selecting Encryption
    public void OnSelectorValueChanged()
    {
        ClearWarningMessage();
        DisableAllEncryptionSettings();

        int selectorValue = encryptionSelector.value;

        if (selectorValue > 0) 
        {
            encryptButton.interactable = true;
            decryptButton.interactable = true;

            SetEncryptionSettings(selectorValue);
        }

        switch (selectorValue)
        {
            case 1:
                encryption = new CaesarsCipher();
                break;
            case 2:
                encryption = new TritemiusEncryption();
                break;
            case 3:
                encryption = new XOREncryption();
                break;
            case 4:
                encryption = new VigenereEncryption();
                break;
            case 5:
                encryption = new RSAEncryption();
                break;
            case 0: case 6:
                encryption = null;

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
    #endregion

    #region Open/Save
    public void OpenFileEncryption()
    {
        encryptionField.text = OpenFile();
    }

    public void OpenFileDecryption()
    {
        decryptionField.text = OpenFile();
    }

    private string OpenFile()
    {
        var extensions = new[] {
            new ExtensionFilter("Text Files ", "txt" ),
            new ExtensionFilter("Images ", "png" ),
            };

        string[] path = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);

        if (path == null)
        {
            StartCoroutine(SendUserMessage("Please select file", Color.yellow, warningTime));
            return "";
        }

        if (path[0].EndsWith(".txt"))
        {
            StreamReader reader = new StreamReader(path[0]);
            string message = reader.ReadToEnd();
            reader.Close();

            return message;
        }
        else if (path[0].EndsWith(".png"))
        {
            return Helper.ProcessImageToString(path[0]);
        }

        return "";
    }

    public void SaveFileEncryption()
    {
        SaveFile(encryptionField.text);
    }

    public void SaveFileDecryption()
    {
        SaveFile(decryptionField.text);
    }

    private void SaveFile(string message)
    {
        var extensionList = new[] {
            new ExtensionFilter("Text File ", "txt")
        };

        string path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "New File", extensionList);

        if (path == "")
        {
            StartCoroutine(SendUserMessage("Please select file", Color.yellow, warningTime));
            return;
        }

        StreamWriter writer = new StreamWriter(path, true);
        writer.Write(message);
        writer.Close();
    }
    #endregion

    #region  Messaging system
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
    #endregion

    #region Author/Exit
    public void ShowAuthor()
    {
        ClearWarningMessage();
        StartCoroutine(SendUserMessage("Made by Kis Yuriy\nPMi-33", Color.black, authorMessageTime));
    }

    public void ExitApplication() => Application.Quit();
    #endregion

    #region Additional Events
    // XOR Specific
    public void OnKeyLoadRequested()
    {
        TMP_InputField input = encryptionSettings.GetComponentInChildren<TMP_InputField>(false);

        input.text = OpenFile();
    }

    // RSA Specific
    public void OnKeyGenerationRequested()
    {
        TMP_InputField[] inputs = encryptionSettings.GetComponentsInChildren<TMP_InputField>(false);

        string[] options = new string[3];
        options[0] = inputs[0].text;
        options[1] = inputs[1].text;
        options[2] = inputs[2].text;

        string respone = encryption.Validate(encryptionField.text, options);

        if (respone != "")
        {
            StartCoroutine(SendUserMessage(respone, Color.red, warningTime));
            return;
        }

        var keys = (encryption as RSAEncryption).
            GetKeys(int.Parse(inputs[0].text), 
                    int.Parse(inputs[1].text), 
                    int.Parse(inputs[2].text));

        inputs[3].text = keys.open.Exp.ToString();
        inputs[4].text = keys.open.N.ToString();
        inputs[5].text = keys.closed.Exp.ToString();
        inputs[6].text = keys.closed.N.ToString();
    }
    #endregion
}