using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class TritemiusBruteForce : MonoBehaviour
{
    EncryptionController controller;

    private void Awake()
    {
        controller = GetComponentInParent<EncryptionController>();
    }

    public void OnButtonClicked()
    {
        string initialText = controller.encryptionField.text;
        string modifiedText = controller.decryptionField.text;

        KeyValuePair<Language, string> language = LanguageDetection.DetectLanguage(initialText).First();
        TritemiusEncryption encryption = new TritemiusEncryption();

        if (initialText.Length > modifiedText.Length)
        {
            initialText = initialText.Substring(0, modifiedText.Length);
        }
        else if (initialText.Length < modifiedText.Length)
        {
            modifiedText = modifiedText.Substring(0, initialText.Length);
        }

        if (initialText.Length >= 5 && modifiedText.Length >= 5)
        {
            for (int i = 0; i < language.Value.Length; ++i)
            {
                for (int j = 0; j < language.Value.Length; ++j)
                {
                    for (int k = 0; k < language.Value.Length; ++k)
                    {
                        string[] options = new string[3];
                        options[0] = i.ToString();
                        options[1] = j.ToString();
                        options[2] = k.ToString();

                        string attackedText = encryption.Encrypt(initialText, options);

                        if (attackedText == modifiedText)
                        {
                            StartCoroutine(controller.SendUserMessage($"Keys:\n\t a: {Helper.Mod(i, language.Value.Length)}\n" +
                                $"\t b: {Helper.Mod(j, language.Value.Length)}\n" +
                                $"\t c: {Helper.Mod(k, language.Value.Length)}\n", Color.green, 5f));

                            return;
                        }
                    }
                }
            }
            string[] options2 = new string[1];
            options2[0] = initialText;

            StartCoroutine(controller.SendUserMessage(encryption.Decrypt(modifiedText, options2), Color.green, 5f));
        }
        else
        {
            StartCoroutine(controller.SendUserMessage("Text length is too short (please use >5 symbols).", Color.red, controller.warningTime));
        }
    }
}
