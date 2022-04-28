public interface IEncryption
{
    public string Validate(string message, string[] options);

    public string Encrypt(string message, string[] options);

    public string Decrypt(string message, string[] options);
}
