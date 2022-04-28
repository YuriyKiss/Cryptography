public class TritemiusTests
{
    public TritemiusTests() { }

    public bool ActivateTest()
    {
        TritemiusEncryption tritemius = new TritemiusEncryption();

        string[] options = new string[2];
        options[0] = "5";
        options[1] = "3";
        string result = tritemius.Encrypt("Message", options);

        if (result != "Mmejxhk")
            return false;

        return true;
    }
}
