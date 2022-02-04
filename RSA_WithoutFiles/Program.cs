// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;



RsaEncryptionClass rsa = new RsaEncryptionClass();
rsa.RSAexportKeys();
byte[] publicKeyBytes = rsa.GetPublicKey();
Console.WriteLine("public key: " + Encoding.UTF8.GetString(publicKeyBytes));//отправим на сервер
Console.WriteLine();
byte[] privateKeyBytes = rsa.GetPrivateKey();
Console.WriteLine("private key:" + Encoding.UTF8.GetString(privateKeyBytes));//сохраняем в настройки приложения (локально)
Console.WriteLine();

string cypher = string.Empty;
Console.WriteLine("текст для шифровки:");
var text = Console.ReadLine();
if (!string.IsNullOrEmpty(text))
{
    cypher = rsa.Encrypt(text);
    Console.WriteLine($"Encrypted text:\n" + cypher);
}
Console.WriteLine();
var plainText = rsa.Decrypt(cypher);
Console.WriteLine("Decrypted text:\n" + plainText);

Console.ReadKey();

class RsaEncryptionClass
{
    RSACryptoServiceProvider cspNew;
    RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
    RSAParameters _privatekey;
    RSAParameters _publickey;

   
    public void RSAexportKeys()
    {
        _privatekey = csp.ExportParameters(true);
        _publickey = csp.ExportParameters(false);
    }

    //public void SetKeySize()
    //{
    //    csp.KeySize = 16384;
    //}

    public byte[] GetPublicKey()
    {
        var sw = new StringWriter();
        XmlSerializer xs = new XmlSerializer(typeof(RSAParameters));
        xs.Serialize(sw, _publickey);
        byte[] key = Encoding.ASCII.GetBytes(sw.ToString());
        return key;

    }

    public byte[] GetPrivateKey()
    {
        var sw = new StringWriter();
        XmlSerializer xs = new XmlSerializer(typeof(RSAParameters));
        xs.Serialize(sw, _privatekey);
        byte[] key = Encoding.ASCII.GetBytes(sw.ToString());
        return key;
    }

    public string Encrypt(string plainText)
    {
        cspNew = new RSACryptoServiceProvider();
        cspNew.ImportParameters(_publickey);
        var data = Encoding.Unicode.GetBytes(plainText);
        var cypher = csp.Encrypt(data, false);
        return Convert.ToBase64String(cypher);
    }

    public string Decrypt(string cypherText)
    {
        var dataBytes = Convert.FromBase64String(cypherText);
        cspNew.ImportParameters(_privatekey);
        var plainText = csp.Decrypt(dataBytes, false);
        return Encoding.Unicode.GetString(plainText);
    }
}


