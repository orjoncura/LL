namespace LL.Core.Model.DataTransferObjects;

public class HashPasswordModel
{
    public string Password { get; set; }
    public string Salt { get; set; }

    public HashPasswordModel(string password, string salt) 
    { 
        Password = password;
        Salt = salt;
    }
}
