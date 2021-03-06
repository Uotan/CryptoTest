using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;



classRSA rsa = new classRSA();
string _publicKey = rsa.GetPublicBase64();
string _privateKey = rsa.GetPrivateBase64();

string _cryptedText = rsa.Encrypt("hello",_publicKey);
Console.WriteLine(_cryptedText+"\n\n");
string _DEcryptedText = rsa.Decrypt(_cryptedText,_privateKey);
Console.WriteLine(_DEcryptedText);
Console.ReadKey();


public class classRSA
{
    byte[] publicKeyBytes;
    byte[] privateKeyBytes;
    RSACryptoServiceProvider csp;
    RSAParameters _privatekeyParametrs;
    RSAParameters _publickeyParametrs;

    public classRSA()
    {
        csp = new RSACryptoServiceProvider();
        RSAexportKeys();
        publicKeyBytes = GetPublicKey();
        privateKeyBytes = GetPrivateKey();
    }

    void RSAexportKeys()
    {
        SetKeySize();
        _privatekeyParametrs = csp.ExportParameters(true);
        _publickeyParametrs = csp.ExportParameters(false);
    }

    void SetKeySize()
    {
        csp.KeySize = 2048;
    }

    byte[] GetPublicKey()
    {
        var sw = new StringWriter();
        XmlSerializer xs = new XmlSerializer(typeof(RSAParameters));
        xs.Serialize(sw, _publickeyParametrs);
        byte[] key = Encoding.ASCII.GetBytes(sw.ToString());
        return key;

    }

    byte[] GetPrivateKey()
    {
        var sw = new StringWriter();
        XmlSerializer xs = new XmlSerializer(typeof(RSAParameters));
        xs.Serialize(sw, _privatekeyParametrs);
        byte[] key = Encoding.ASCII.GetBytes(sw.ToString());
        return key;
    }



    public string GetPublicBase64()
    {
        string key = Convert.ToBase64String(publicKeyBytes);
        return key;
    }

    public string GetPrivateBase64()
    {
        string key = Convert.ToBase64String(privateKeyBytes);
        return key;
    }



    public string Encrypt(string _plainText, string _publickeyBase64)
    {
        byte[] base64EncodedBytes = Convert.FromBase64String(_publickeyBase64);
        RSACryptoServiceProvider cspNew = new RSACryptoServiceProvider();
        cspNew.FromXmlString(Encoding.UTF8.GetString(base64EncodedBytes));

        var data = Encoding.Unicode.GetBytes(_plainText);
        var cypher = cspNew.Encrypt(data, false);
        return Convert.ToBase64String(cypher);
    }

    public string Decrypt(string _cypherText, string _privatekeyBase64)
    {
        byte[] base64EncodedBytes = Convert.FromBase64String(_privatekeyBase64);
        RSACryptoServiceProvider cspNew = new RSACryptoServiceProvider();
        cspNew.FromXmlString(Encoding.UTF8.GetString(base64EncodedBytes));
        var dataBytes = Convert.FromBase64String(_cypherText);
        var plainText = cspNew.Decrypt(dataBytes, false);
        return Encoding.Unicode.GetString(plainText);
    }

}