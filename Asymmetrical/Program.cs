// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

if (!File.Exists("public.xml")|| !File.Exists("private.xml"))
{
    File.Create("public.xml");
    File.Create("private.xml");
}




RsaEncryptionClass rsa = new RsaEncryptionClass();

Console.WriteLine("public key: "+rsa.GetPublicKey());
Console.WriteLine();
Console.WriteLine("private key:" + rsa.GetPrivateKey());
Console.WriteLine();
string cypher = string.Empty;
Console.WriteLine("текст для шифровки:");
var text = Console.ReadLine();
if (!string.IsNullOrEmpty(text))
{
    cypher = rsa.Encrypt(text);
    Console.WriteLine($"Encrypted text:\n"+cypher);
}
Console.WriteLine();
var plainText = rsa.Decrypt(cypher);
Console.WriteLine("Decrypted text:\n" + plainText);

Console.ReadKey();

class RsaEncryptionClass
{
    RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
    RSAParameters _privatekey;
    RSAParameters _publickey;


    public RsaEncryptionClass()
    {
        _privatekey = csp.ExportParameters(true);
        _publickey = csp.ExportParameters(false);
    }

    public string GetPublicKey()
    {
        var sw = new StringWriter();
        XmlSerializer xs = new XmlSerializer(typeof(RSAParameters));    
        xs.Serialize(sw, _publickey);
        if (File.Exists("public.xml"))
        {
            File.WriteAllText("public.xml", String.Empty);
            byte[] key = Encoding.ASCII.GetBytes(sw.ToString());
            File.WriteAllBytes("public.xml", key);
        }
        return sw.ToString();

    }

    public string GetPrivateKey()
    {
        var sw = new StringWriter();
        XmlSerializer xs = new XmlSerializer(typeof(RSAParameters));
        xs.Serialize(sw, _privatekey);
        if (File.Exists("public.xml"))
        {
            File.WriteAllText("private.xml", String.Empty);
            byte[] key = Encoding.ASCII.GetBytes(sw.ToString());
            File.WriteAllBytes("private.xml", key);
        }
        return sw.ToString();
    }

    public string Encrypt(string plainText)
    {
        csp = new RSACryptoServiceProvider();
        csp.ImportParameters(_publickey);
        var data = Encoding.Unicode.GetBytes(plainText);
        var cypher = csp.Encrypt(data, false);
        return Convert.ToBase64String(cypher);
    }

    public string Decrypt(string cypherText)
    {
        var dataBytes = Convert.FromBase64String(cypherText);
        csp.ImportParameters(_privatekey);
        var plainText = csp.Decrypt(dataBytes, false);
        return Encoding.Unicode.GetString(plainText);
    }
}


