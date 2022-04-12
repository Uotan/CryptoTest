using PasswordGenerator;

for (int i = 0; i < 15; i++)
{
    var pwd = new Password(16).IncludeLowercase().IncludeUppercase().IncludeNumeric().IncludeSpecial("!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~");
    string AESkey = pwd.Next();
    Console.WriteLine(AESkey);
}
Console.ReadKey();