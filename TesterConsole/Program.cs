// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography;

SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
string rdata = Convert.ToBase64String(sha1.ComputeHash(System.Text.Encoding.UTF8.GetBytes("123456")));


Console.WriteLine(rdata);
