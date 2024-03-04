using System.Security.Cryptography;
using System.Text;

namespace project_navigator.services;

public interface IHashService
{
    string HashString(string str);
}

public class HashService : IHashService
{
    public string HashString(string str)
    {
        using (var sha512 = SHA512.Create())
        {
            // Преобразование строки в массив байтов и вычисление хэша
            var bytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(str));

            // Преобразование массива байтов в строку
            var builder = new StringBuilder();
            for (var i = 0; i < bytes.Length; i++) builder.Append(bytes[i].ToString("x2"));
            return builder.ToString();
        }
    }
}