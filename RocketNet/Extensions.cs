using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace RocketNet
{
    public static class Extensions
    {
        public static int ToInt(this object obj)
        {
            return Convert.ToInt32(obj);
        }

        public static long ToInt64(this object obj)
        {
            return Convert.ToInt64(obj);
        }

        public static DateTime ToDateTime(this object obj)
        {
            return Convert.ToDateTime(obj);
        }

        public static DateTime ToDateTime(this string text, string dateFormat)
        {
            return DateTime.ParseExact(text, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        public static decimal ToDecimal(this object obj)
        {
            return Convert.ToDecimal(obj);
        }

        public static bool ToBoolean(this object obj)
        {
            return Convert.ToBoolean(obj);
        }

        public static float ToFloat(this object obj)
        {
            return Convert.ToSingle(obj);
        }

        public static char ToChar(this object obj)
        {
            return Convert.ToChar(obj);
        }

        public static byte ToByte(this object obj)
        {
            return Convert.ToByte(obj);
        }

        public static bool IsNull(this object obj)
        {
            return (obj == null);
        }

        public static bool IsNotNull(this object obj)
        {
            return (obj != null);
        }

        public static bool IsNumeric(this string obj)
        {
            return new Regex(@"^-*[0-9,\.]+$").IsMatch(obj);
        }

        public static bool IsEmail(this string text)
        {
            return new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$").IsMatch(text);
        }

        public static string GetTypeName(this object obj)
        {
            return obj.GetType().Name;
        }

        public static string GetTypeFullName(this object obj)
        {
            return obj.GetType().FullName;
        }

        public static string ToCapitalize(this string obj)
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(obj);
        }

        public static string GeneratePassword()
        {
            return GeneratePassword(8);
        }

        public static string GeneratePassword(int length)
        {
            try
            {
                string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
                char[] chars = new char[length];
                Random rd = new Random();

                for (int i = 0; i < length; i++)
                {
                    chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
                }
                return new string(chars);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string ToSHA1(this string text)
        {
            if (string.IsNullOrEmpty(text.Trim()))
                throw new Exception("Şifrelenecek string türünde nesne bulunamadı.");

            SHA1 sha1 = new SHA1CryptoServiceProvider();
            return BitConverter.ToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(text)));
        }

        public static string ToSHA256(this string text)
        {
            if (string.IsNullOrEmpty(text.Trim()))
                throw new Exception("Şifrelenecek string türünde nesne bulunamadı.");

            SHA256 sha = new SHA256Managed();
            return BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(text)));
        }

        public static string ToSHA512(this string text)
        {
            if (string.IsNullOrEmpty(text.Trim()))
                throw new Exception("Şifrelenecek string türünde nesne bulunamadı.");

            SHA512 sha = new SHA512Managed();
            return BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(text)));
        }

        public static string ToMD5(this string text)
        {
            if (string.IsNullOrEmpty(text.Trim()))
                throw new Exception("Şifrelenecek string türünde nesne bulunamadı.");

            MD5 md5 = new MD5CryptoServiceProvider();
            return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(text)));
        }

        public static string ToBase64(this string text)
        {
            if (string.IsNullOrEmpty(text.Trim()))
                throw new Exception("Şifrelenecek string türünde nesne bulunamadı.");

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }

        public static string FromBase64(this string text)
        {
            if (string.IsNullOrEmpty(text.Trim()))
                throw new Exception("Çözülecek string türünde nesne bulunamadı.");

            return Encoding.UTF8.GetString(Convert.FromBase64String(text));
        }

        public static string ToEncrypt(this string text)
        {
            string EncryptionKey = "?E9QjrfA!P+GJVD3G^*Rw=*C2";
            byte[] clearBytes = Encoding.Unicode.GetBytes(text);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = rfc.GetBytes(32);
                encryptor.IV = rfc.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    text = Convert.ToBase64String(ms.ToArray());
                }
            }
            return text;
        }

        public static string ToDecrypt(this string text)
        {
            string EncryptionKey = "?E9QjrfA!P+GJVD3G^*Rw=*C2";
            text = text.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(text);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    text = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return text;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> collection)
        {
            DataTable dt = null;
            try
            {
                dt = new DataTable();
                PropertyInfo[] propinfo = typeof(T).GetProperties();
                foreach (var item in propinfo)
                {
                    dt.Columns.Add(item.Name, item.PropertyType);
                }
                foreach (T item in collection)
                {
                    DataRow drow = dt.NewRow();
                    drow.BeginEdit();
                    foreach (var pi in propinfo)
                        drow[pi.Name] = pi.GetValue(item, null);
                    drow.EndEdit();
                    dt.Rows.Add(drow);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public static List<T> ToList<T>(this DataTable dt) where T : new()
        {
            try
            {
                List<string> columns = new List<string>();
                foreach (DataColumn item in dt.Columns)
                {
                    columns.Add(item.ColumnName);
                }
                return dt.AsEnumerable().ToList().ConvertAll<T>(x => GetObject<T>(x, columns)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static T GetObject<T>(DataRow row, List<string> columns) where T : new()
        {
            T obj = default(T);
            try
            {
                obj = new T();
                foreach (PropertyInfo item in typeof(T).GetProperties())
                {
                    string name = columns.Find(x => x.ToLower() == item.Name.ToLower());
                    if (!string.IsNullOrEmpty(name))
                    {
                        string value = row[name].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (Nullable.GetUnderlyingType(item.PropertyType) != null)
                            {
                                value = row[name].ToString().Replace("$", string.Empty).Replace(",", string.Empty);
                                item.SetValue(obj, Convert.ChangeType(value, Type.GetType(Nullable.GetUnderlyingType(item.PropertyType).ToString())), null);
                            }
                            else
                            {
                                value = row[name].ToString().Replace("%", "");
                                item.SetValue(obj, Convert.ChangeType(value, Type.GetType(item.PropertyType.ToString())), null);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }

        public static bool IsAvailable(this SqlConnection connection)
        {
            bool sonuc = false;
            try
            {
                connection.Open();
                connection.Close();
                sonuc = true;
            }
            catch
            {
                sonuc = false;
            }
            return sonuc;
        }

        public static bool IsProcessOpen(string name)
        {
            foreach (Process prcs in Process.GetProcesses())
            {
                if (prcs.ProcessName.Contains(name))
                    return true;
            }
            return false;
        }

        public static bool DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            return false;
        }

        public static string GetIPAddress()
        {
            try
            {
                return new WebClient().DownloadString("http://phihag.de/ip/");
            }
            catch
            {
                return "IP adresi tanınmıyor.";
            }
        }

        public static string GetLocalIPAddress()
        {
            string result = string.Empty;
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        result = ip.ToString();
                    }
                }
            }
            catch
            {
                result = "IP adresi tanınmıyor.";
            }
            return result;
        }

        public static string BytesToString(long byteCount)
        {
            string[] suf = { " Bayt", " KB", " MB", " GB", " TB", " PB", " EB" };
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }

        public static string Duration(DateTime Date, DateTime Now)
        {
            string Tense;
            double Comparison = (double)DateTime.Compare(Date, Now);
            if (Comparison < 0)
            {
                Tense = "önce";
            }
            else
            {
                Tense = "şimdi";
            }
            String[] Periods = new String[8] { "saniye", "dakika", "saat", "gün", "hafta", "ay", "yıl", "decade" };
            double[] Lengths = new double[7] { 60, 60, 24, 7, 4.35, 12, 10 };

            double Diff = Now.Subtract(Date).TotalSeconds;
            int j;
            for (j = 0; Diff >= Lengths[j] && j < Lengths.Length; j++)
            {
                Diff /= Lengths[j];
            }
            Diff = Math.Round(Diff);
            if (Diff < 0)
            {
                Diff *= -1;
            }
            return Convert.ToString(Diff) + " " + Periods[j] + " " + Tense;
        }

        public static string AssemblyGuid()
        {
            GuidAttribute attr = (GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false)[0];
            return attr.Value;
        }

        public static string GetHddSerialNumber()
        {
            string result = string.Empty;
            using (ManagementClass mc = new ManagementClass("Win32_DiskDrive"))
            {
                using (ManagementObjectCollection moc = mc.GetInstances())
                {
                    foreach (ManagementObject mo in moc)
                    {
                        if (result == string.Empty)
                        {
                            result = mo["SerialNumber"].ToString();
                            break;
                        }
                    }
                }
            }
            return result.Trim();
        }

        public static string GetCPUSerialNumber()
        {
            string result = string.Empty;
            using (ManagementClass mc = new ManagementClass("Win32_Processor"))
            {
                using (ManagementObjectCollection resultcollection = mc.GetInstances())
                {
                    foreach (var item in resultcollection)
                    {
                        if (!string.IsNullOrEmpty(item["ProcessorID"].ToString()))
                            result += item["ProcessorID"].ToString();
                    }
                }
            }
            return result.Trim();
        }

        public static string GetMacAddress()
        {
            string result = string.Empty;
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up && (!nic.Description.Contains("Virtual") && !nic.Description.Contains("Pseudo")))
                {
                    if (nic.GetPhysicalAddress().ToString() != string.Empty)
                    {
                        result = nic.GetPhysicalAddress().ToString();
                    }
                }
            }
            return result;
        }

        public static void Open(this SqlCommand cmd)
        {
            if (cmd != null && cmd.Connection != null && cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();
        }

        /// <summary>
        /// Belirtilen SqlCommand nesnesinin veritabanı bağlantısı "Açık" durumundaysa "Kapalı" konumuna getirir.
        /// </summary>
        /// <param name="cmd"></param>
        public static void Close(this SqlCommand cmd)
        {
            if (cmd != null && cmd.Connection != null && cmd.Connection.State != ConnectionState.Closed)
                cmd.Connection.Close();
        }
    }
}