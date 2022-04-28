using UnityEngine;

public class TestController : MonoBehaviour
{
    public void ActivateAllTests()
    {
        string response = "";
        CaesarTests caesar = new CaesarTests();

        if (caesar.ActivateTest())
        {
            response += "Caesar tests passed; ";
        }
        else
        {
            response += "Caesar tests failed; ";
        }

        TritemiusTests tritemius = new TritemiusTests();

        if (tritemius.ActivateTest())
        {
            response += "Tritemius tests passed; ";
        }
        else
        {
            response += "Tritemius tests failed; ";
        }

        StartCoroutine(GetComponentInParent<EncryptionController>().SendUserMessage(response, Color.magenta, 5f));
    }
}
