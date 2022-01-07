// See https://aka.ms/new-console-template for more information
using Microsoft.Data.Sqlite;
using System.Security.Cryptography;
using System.Text;


Aes aes = Aes.Create();
//aes.KeySize = 128;
//aes.BlockSize = 128;
string pass = "1234567890123456";
byte[] key = Encoding.ASCII.GetBytes(pass);


foreach (var item in key)
{
    Console.Write(item + " ");
}
Console.WriteLine();
Console.WriteLine(Encoding.UTF8.GetString(key));
aes.Key = key;

Encrypt();

//сначала расшифруй
//Decrypt();


Console.ReadKey();

void Encrypt()
{
    using (FileStream fileStream = new("TestData.txt", FileMode.OpenOrCreate))
    {
        using (Aes aes = Aes.Create())
        {
            byte[] iv = aes.IV;
            fileStream.Write(iv, 0, iv.Length);

            using (CryptoStream cryptoStream = new(fileStream,aes.CreateEncryptor(),CryptoStreamMode.Write))
            {
                using (StreamWriter encryptWriter = new(cryptoStream))
                {
                    encryptWriter.WriteLine("Hello World!");
                }
            }
        }
    }

    Console.WriteLine("The file was encrypted.");
    //зашифровали - молодцы

    //byte[] data = File.ReadAllBytes(@"TestData.txt");
    //StreamReader streamReader = new StreamReader("TestData.txt");
    //Console.WriteLine(streamReader.ReadToEnd());
    //streamReader.Close();
    Console.WriteLine("\n\n");
    //foreach (var item in data)
    //{
    //    Console.Write(item + " ");
    //}



    //using (var connection = new SqliteConnection("Data Source=data.db"))
    //{
    //    connection.Open();

    //    SqliteCommand command = new SqliteCommand();
    //    command.Connection = connection;
    //    command.CommandText = "INSERT INTO main (info) VALUES (@info)";
    //    // создаем параметр для имени
    //    SqliteParameter nameParam = new SqliteParameter("@info", data);
    //    //SqliteParameter nameParam = new SqliteParameter("@info", "test");
    //    // добавляем параметр к команде
    //    command.Parameters.Add(nameParam);
    //    command.ExecuteNonQuery();
    //}



    Decrypt();
}


async void Decrypt()
{
    //goto skip;

    //string dataCrypt = "";
    //string sqlExpression = "SELECT info FROM main where id = 21";
    //using (var connection = new SqliteConnection("Data Source=data.db"))
    //{
    //    connection.Open();

    //    SqliteCommand command = new SqliteCommand(sqlExpression, connection);
    //    using (SqliteDataReader reader = command.ExecuteReader())
    //    {
    //        if (reader.HasRows) // если есть данные
    //        {
    //            while (reader.Read())   // построчно считываем данные
    //            {
    //                dataCrypt = (string)reader.GetValue(0);
    //            }
    //        }
    //    }
    //}
    ////var base64EncodedBytes = Convert.FromBase64String(dataCrypt);

    ////Console.WriteLine(Encoding.UTF8.GetString(key));
    ////Console.WriteLine(Encoding.UTF8.GetString(base64EncodedBytes));
    //Console.WriteLine(dataCrypt);

    //using (StreamWriter sw = new StreamWriter(@"TestData.txt", false, System.Text.Encoding.Default))
    //{
    //    sw.Write(dataCrypt);
    //}

    //skip:

    using (FileStream fileStream = new("TestData.txt", FileMode.Open))
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

            using (CryptoStream cryptoStream = new(fileStream,aes.CreateDecryptor(key, iv),CryptoStreamMode.Read))
            {
                using (StreamReader decryptReader = new(cryptoStream))
                {
                    string decryptedMessage = await decryptReader.ReadToEndAsync();
                    Console.WriteLine($"The decrypted original message: {decryptedMessage}");
                }
            }
    }
}





