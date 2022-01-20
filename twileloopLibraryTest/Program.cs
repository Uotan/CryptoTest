// See https://aka.ms/new-console-template for more information

using System.Text;


RSAmethod();

void AESmethod()
{
    string password = "123qweFatal";
    ExpressEncription.AESEncription.AES_Encrypt("data.txt", password);


    byte[] dataCrypted = Encoding.ASCII.GetBytes("data.txt.aes");
    foreach (var item in dataCrypted)
    {
        Console.Write(item + " ");
    }
    Console.WriteLine();

    ExpressEncription.AESEncription.AES_Decrypt("data.txt.aes", password);


    using (StreamReader sr = new StreamReader("data.txt.aes.decrypted"))
    {
        Console.WriteLine(sr.ReadToEnd());
    }
}

void RSAmethod()
{
    ExpressEncription.RSAEncription.MakeKey("publicKey.xml", "privateKey.xml");
    var plainText = "hello shitbuilder";
    var cypherText = ExpressEncription.RSAEncription.EncryptString(plainText, "publicKey.xml");

    Console.WriteLine(cypherText);
    var decryptedText = ExpressEncription.RSAEncription.DecryptString(cypherText, "privateKey.xml");
    Console.WriteLine(decryptedText);
}



Console.ReadKey();