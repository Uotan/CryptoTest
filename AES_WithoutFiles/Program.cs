using System.Security.Cryptography;
using System.Text;

// KeySizes retrieved from the System.Security.Cryptography.Aes
// object.
// Minimum key size bits: 128
// Maximum key size bits: 256
// Interval between key size bits: 64
// aes.blocksize:128

//пароль, еще не пробовал изменять длинну ключа
//string pass = "4elpsGky8'}|;I[*4elpsGky8'}|;I[*"; //256bit
//string pass = "4elpsGky8'}|;I[*11111111";//192bit
string pass = "4elpsGky8'}|;I[*";//128bit

//переводим в байты
byte[] key = Encoding.ASCII.GetBytes(pass);
//строка формата base64, в нее засунем 
string base64_cryptedData = String.Empty;

foreach (var item in key)
{
    Console.Write(item + " ");
}
Console.WriteLine("\n"+Encoding.UTF8.GetString(key));

Encrypt();
Decrypt();
Console.ReadKey();

void Encrypt()
{
    Aes aes = Aes.Create();

    aes.KeySize = 128;

    aes.Key = key;
    //зашифрованные данные в виде массива байтов
    byte[] cryptedMessageByteArray;
    using (MemoryStream fileStream = new MemoryStream())
    {
        byte[] iv = aes.IV;
        fileStream.Write(iv, 0, iv.Length);
        using (CryptoStream cryptoStream = new(
            fileStream,
            aes.CreateEncryptor(),
            CryptoStreamMode.Write))
        {
            using (StreamWriter encryptWriter = new(cryptoStream))
            {
                encryptWriter.WriteLine("Hello World!");
            }
        }
        cryptedMessageByteArray = fileStream.ToArray();
    }
    Console.WriteLine("The file was encrypted.");
    //конвертируем полученный массив в удобный для чтения и ДАЛЬНЕЙШЕЙ передачи формат
    base64_cryptedData = Convert.ToBase64String(cryptedMessageByteArray);
    //прочитаем по байтам потому, что - а почему бы и нет
    foreach (var item in cryptedMessageByteArray)
    {
        Console.Write(item + " ");
    }
    Console.WriteLine("\n"+Convert.ToBase64String(cryptedMessageByteArray));
}


void Decrypt()
{
    Aes aes = Aes.Create();
    aes.KeySize = 128;

    aes.Key = key;
    //ковертируем base64 зашифрованные данные обратно в белеберду
    byte[] base64EncodedBytes = Convert.FromBase64String(base64_cryptedData);
    try
    {
        using (MemoryStream fileStream = new MemoryStream(base64EncodedBytes))
        {
            byte[] iv = new byte[aes.IV.Length];
            int numBytesToRead = aes.IV.Length;
            int numBytesRead = 0;
            while (numBytesToRead > 0)
            {
                int n = fileStream.Read(iv, numBytesRead, numBytesToRead);
                if (n == 0) break;

                numBytesRead += n;
                numBytesToRead -= n;
            }

            using (CryptoStream cryptoStream = new(
               fileStream,
               aes.CreateDecryptor(key, iv),
               CryptoStreamMode.Read))
            {
                using (StreamReader decryptReader = new(cryptoStream))
                {
                    string decryptedMessage = decryptReader.ReadToEnd();
                    Console.WriteLine($"The decrypted original message:\n{decryptedMessage}");
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.Write("\n" + ex.Message);
    }
}