using TMPro;

using UnityEngine;
using UnityEngine.UI;

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

    [Header("User warning")]
    public TMP_Text warning;
    public float warningTime;

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
            StartCoroutine(SendUserMessage(validationMessage));
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
            StartCoroutine(SendUserMessage(validationMessage));
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
        warning.text = "";
    }

    private IEnumerator SendUserMessage(string message)
    {
        warning.text = message;
        yield return new WaitForSeconds(warningTime);
        warning.text = "";
    }
}
