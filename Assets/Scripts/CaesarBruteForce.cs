using UnityEngine;

using System.Linq;
using System.Collections.Generic;

public class CaesarBruteForce : MonoBehaviour
{
    EncryptionController controller;

    private void Awake()
    {
        controller = GetComponentInParent<EncryptionController>();
    }

    public void OnButtonClick()
    {
        string message = controller.decryptionField.text;
        string response = "";

        CaesarsCipher cipher = new CaesarsCipher();
        string[] options = new string[1];
        options[0] = "1";

        string validationMessage = cipher.Validate(message, options);

        if (validationMessage == "")
        {
            Dictionary<Language, string> language = LanguageDetection.DetectLanguage(message);
            int languageLength = language.First().Value.Length;

            for (int i = languageLength - 1; i > 0; i--)
            {
                options[0] = $"{i}";
                response += $"{languageLength - i}. " + cipher.Encrypt(message, options) + "\n";
            }

            controller.encryptionField.text = response;
        }
        else
        {
            StartCoroutine(controller.SendUserMessage(validationMessage, Color.red, controller.warningTime));
        }
    }
}
