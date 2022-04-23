public class CaesarsCipher : IEncryption
{
    public CaesarsCipher() { }

    public string Validate(string message, string[] options)
    {
        if (message == "")
        {
            return "Message should not be empty";
        }

        if (options[0] == "")
        {
            return "Please enter offset";
        }

        int offset = int.Parse(options[0]);

        if (offset <= 0 || offset >= 24)
        {
            return "Offset is out of bounds, set it between 1 and 23 (both numbers including)";
        }

        return "";
    }

    public string Encrypt(string message, string[] options)
    {
        return message;
    }
    
    public string Decrypt(string message, string[] options)
    {
        return message;
    }
}
