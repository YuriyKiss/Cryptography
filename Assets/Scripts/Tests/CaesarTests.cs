public class CaesarTests
{
    public CaesarTests() { }

    public bool ActivateTest()
    {
        CaesarsCipher caesar = new CaesarsCipher();

        string[] options = new string[1];
        options[0] = "1";
        string result = caesar.Encrypt("abcdef", options);

        if (result != "bcdefg")
            return false;

        return true;
    }
}
