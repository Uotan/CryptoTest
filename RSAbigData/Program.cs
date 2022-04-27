using System.Security.Cryptography;
using System.Text;
using XC.RSAUtil;

// thanck you Zhiqiang Li
//https://github.com/stulzq/RSAUtil


Console.WriteLine("Key Convert:");
var keyList = RsaKeyGenerator.Pkcs1Key(2048, false);
var privateKey = keyList[0];
var publicKey = keyList[1];



//Console.WriteLine("\npublic key pkcs1 -> xml:");
//Console.WriteLine(RsaKeyConvert.PublicKeyPemToXml(publicKey));

//Console.WriteLine("\nprivate key -> xml:");
//Console.WriteLine(RsaKeyConvert.PrivateKeyPkcs1ToXml(privateKey));

Console.WriteLine("\npublic key:" + privateKey);
Console.WriteLine("\nprivate key:" + publicKey);



RsaPkcs1Util bigDataRsa = new RsaPkcs1Util(Encoding.UTF8, publicKey, null, 2048);

var data = "Поддерживает RSACryptoServiceProvider размеры ключей от 384 до 16384 бит приращения 8 бит, если установлен расширенный поставщик шифрования Майкрософт. Он поддерживает размеры ключей от 384 до 512 бит приращения 8 бит, если установлен базовый поставщик шифрования Майкрософт.Допустимые размеры ключей зависят от поставщика служб шифрования(CSP), используемого экземпляром RSACryptoServiceProvider.Windows поставщики служб конфигурации обеспечивают размер ключа от 384 до 16384 бит для версий Windows до Windows 8.1 и размер ключа от 512 до 16384 бит для Windows 8.1.Дополнительные сведения см. в описании функции CryptGenKey в документации по Windows.Класс RSACryptoServiceProvider не позволяет изменять размеры ключей KeySize с помощью свойства.Любое значение, записанное в это свойство, не сможет обновить свойство без ошибок. Чтобы изменить размер ключа, используйте одну из перегрузок конструктора.";
var str = bigDataRsa.EncryptBigData(data, RSAEncryptionPadding.Pkcs1);
Console.WriteLine("Big Data Encrypt:");
Console.WriteLine(str);


RsaPkcs1Util bigDataRsa2 = new RsaPkcs1Util(Encoding.UTF8, null, privateKey, 2048);

Console.WriteLine("Big Data Decrypt:");
Console.WriteLine(string.Join("", bigDataRsa2.DecryptBigData(str, RSAEncryptionPadding.Pkcs1)));





Console.ReadKey();