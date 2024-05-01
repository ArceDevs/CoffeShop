using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CoffeeDBIntegrada.Core
{
    public class Adminstration
    {
        private const string Format = "{0:#.00}";
        private static readonly Regex _digit = new Regex("[^0-9]+");
        private static readonly Regex _date = new Regex("^(0?[1-9]|1[0-9]|2|2[0-9]|3[0-1])/(0?[1-9]|1[0-2])/(d{2}|d{4})$");
        private static readonly Regex _money = new Regex("^[0-9.]*$");


        #region Valid CIF
        public bool validateCIF(string cif)
        {
            if (string.IsNullOrEmpty(cif)) return false;
            cif = cif.Trim().ToUpper();


            if (cif.Length != 9) return false;

            string firstChar = cif.Substring(0, 1);
            string cadena = "ABCDEFGHJNPQRSUVW";

            if (cadena.IndexOf(firstChar) == -1) return false;
            try
            {
                Int32 sumaPar = default(Int32);
                Int32 sumaImpar = default(Int32);

                string cif_sinControl = cif.Substring(0, 8);
                string digits = cif_sinControl.Substring(1, 7);
                for (Int32 n = 0; n <= digits.Length - 1; n += 2)
                {
                    if (n < 6)
                    {
                        sumaPar += Int32.Parse(digits[n + 1].ToString());
                    }

                    Int32 dobleImpar = 2 * Int32.Parse(digits[n].ToString());

                    sumaImpar += (dobleImpar % 10) + (dobleImpar / 10);
                }

                Int32 sumaTotal = sumaPar + sumaImpar;

                sumaTotal = (10 - (sumaTotal % 10)) % 10;

                string digitoControl = "";
                switch (firstChar)
                {
                    case "N":
                    case "P":
                    case "Q":
                    case "R":
                    case "S":
                    case "W":

                        char[] characters = { 'J', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I' };
                        digitoControl = characters[sumaTotal].ToString();
                        break;
                    default:

                        digitoControl = sumaTotal.ToString();
                        break;
                }
                return digitoControl.Equals(cif.Substring(8, 1));
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region ValidDni
        public bool IsValidDni(string dni)
        {
            int minimum = 9;

            if (!(dni.Length >= minimum))
            {
                return false;
            }

            string dniNumbers = dni.Substring(0, dni.Length - 1);
            string dniLeter = dni.Substring(dni.Length - 1, 1);
            var numbersValid = int.TryParse(dniNumbers, out int dniInteger);
            if (!numbersValid)
            {
                return false;
            }
            if (CalculateDNILeter(dniInteger) != dniLeter)
            {
                return false;
            }
            return true;
        }

        public string CalculateDNILeter(int dniNumbers)
        {
            string[] control = { "T", "R", "W", "A", "G", "M", "Y", "F", "P", "D", "X", "B", "N", "J", "Z", "S", "Q", "V", "H", "L", "C", "K", "E" };
            var mod = dniNumbers % 23;
            return control[mod];
        }
        #endregion

        #region ValidPassword
        public bool IsValidPassword(string password)
        {
            int minimum = 5;
            bool hasNum = false;
            bool hasCap = false;
            bool hasLow = false;
            bool hasSpec = false;
            char currentCharacter;

            if (!(password.Length >= minimum))
            {
                return false;
            }

            for (int i = 0; i < password.Length; i++)
            {
                currentCharacter = password[i];
                if (char.IsDigit(currentCharacter))
                {
                    hasNum = true;
                }
                else if (char.IsUpper(currentCharacter))
                {
                    hasCap = true;
                }
                else if (char.IsLower(currentCharacter))
                {
                    hasLow = true;
                }
                else if (!char.IsLetterOrDigit(currentCharacter))
                {
                    hasSpec = true;
                }

                if (hasNum && hasCap && hasLow && hasSpec)
                {
                    return true;
                }
            }

            return false;

        }
        #endregion

        #region ValidDate
        public bool IsValidDate(string date)
        {
            int minimum = 10;
            int hasNum = 0;
            int hasSlash = 0;
            int hasDash = 0;
            char currentCharacter;

            if (!(date.Length >= minimum))
            {
                return false;
            }

            for (int i = 0; i < date.Length; i++)
            {
                currentCharacter = date[i];
                if (char.IsDigit(currentCharacter))
                {
                    hasNum += 1;
                }
                if ((i == 2 || i == 5) && currentCharacter.Equals('/'))
                {
                    hasSlash += 1;
                }
                if ((i == 2 || i == 5) && currentCharacter.Equals('-'))
                {
                    hasDash += 1;
                }

                if (hasNum == 8 && (hasDash == 2 || hasSlash == 2))
                {
                    return true;
                }
            }

            return false;

        }

        public bool IsDate(string date)
        {
            try
            {
                DateTime dt1 = DateTime.Parse(date);
                DateTime dt2 = DateTime.Now;

                if (dt1.Date > dt2.Date)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }


        public bool IsCorrectDate(string date)
        {
            try
            {
                DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region ValidEmail
        public bool IsValidEmail(string emailaddress)
        {
            try
            {
                if (string.IsNullOrEmpty(emailaddress))
                {
                    return true;
                }


                MailAddress m = new MailAddress(emailaddress);
                return true;

            }
            catch (FormatException)
            {
                return false;
            }
        }
        #endregion

        #region TextAllowed
        public bool IsTextAllowed(string text)
        {
            return !_digit.IsMatch(text);
        }

        public bool IsDateAllowed(string text)
        {
            return !_date.IsMatch(text);
        }



        #endregion

        #region MoneyInput
        public bool IsMoneyAllowed(string text)
        {
            return !_money.IsMatch(text);
        }

        public bool IsOneCharAllowed(string text)
        {
            int comma = 0;
            foreach (char currentCharacter in text)
            {

                if (currentCharacter == 44 || currentCharacter == 46)
                {
                    comma += 1;
                }

                if (comma >= 2)
                {
                    return false;
                }
            }
            return true;

        }

        public string Put0After(string text)
        {
            if (text.Substring(0, 1) == ",")
            {
                text = "0" + text;
            }
            return text;

        }

        public string RemoveDupe(string text)
        {
            int contador = 0;
            int dupe = 0;
            int LetterDelete = 80;
            foreach (char currentCharacter in text)
            {

                if (currentCharacter == 44 || currentCharacter == 46)
                {
                    dupe += 1;
                }

                if (dupe == 2)
                {
                    LetterDelete = contador;
                }
                contador++;
            }

            if (LetterDelete != 80)
            {
                text = text.Remove(LetterDelete, 1);
                return text;
            }

            return text;
        }


        public string Money(string text)
        {
            int LetterRemove = 0;
            int TwoDecimal = 0;
            int CommaDot = 0;

            foreach (char currentCharacter in text)
            {

                if (currentCharacter == 44 || currentCharacter == 46)
                {
                    CommaDot++;
                }

                if (CommaDot > 0)
                {
                    TwoDecimal++;
                }

                if (CommaDot > 1)
                {
                    text = text = text.Remove(LetterRemove, 1);
                    return text;
                }

                if (TwoDecimal > 3)
                {
                    text = text = text.Remove(text.Length - 1);
                    return text;
                }
                LetterRemove++;
            }

            return text;
        }


        public bool HasComma(string text)
        {
            foreach (char letter in text)
            {
                if (letter == ',')
                {
                    return true;
                }
            }

            return false;
        }

        public string MoneyString(string text)
        {
            if (text.Length > 6)
            {
                return text;
            }

            char[] sub = text.ToCharArray();
            string[] arr = new string[sub.Length];
            for (int i = 0; i < sub.Length; i++)
            {
                arr[i] = sub[i].ToString();
            }
            List<string> word = new List<string>(arr);

            int LetterDelete = 0;
            int TwoDecimal = 0;
            int CommaDot = 0;

            foreach (string letter in word)
            {
                if (letter == "." || letter == ",")
                {
                    CommaDot++;
                }

                if (CommaDot > 0)
                {
                    TwoDecimal++;
                }


                if (CommaDot > 1)
                {
                    word.RemoveAt(word.IndexOf(letter));

                    Console.WriteLine(letter);
                    Console.WriteLine(String.Join("", word.ToArray()));
                    return String.Join("", word.ToArray());
                }

                if (TwoDecimal > 3)
                {
                    word.RemoveAt(word.IndexOf(letter));

                    return String.Join("", word.ToArray());
                }

                /*if ((letter == "." || letter == ",") && CommaDot == 1 && LetterDelete == 0)
                {
                    word.Insert(LetterDelete, "0");

                    return String.Join("", word.ToArray());
                }*/

                LetterDelete++;
            }

            /*for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == "." || arr[i] == ",")
                {
                    CommaDot++;
                }

                if (CommaDot > 0)
                {
                    TwoDecimal++;
                }

                if (CommaDot > 1)
                {

                    nums.RemoveAt(nums.IndexOf(5));
                    array = nums.ToArray();

                    return text;
                }

                if (TwoDecimal > 3)
                {
                    text = text = text.Remove(text.Length - 1);
                    return text;
                }

            }*/

            return text;
        }

        #endregion

        #region ReverseString
        public string ReverseString(string text)
        {
            char[] charArray = text.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        #endregion

        #region HasLetter
        public bool HasLetter(string text)
        {

            foreach (char currentCharacter in text)
            {

                if (!char.IsDigit(currentCharacter))
                {
                    return true;
                }
            }


            return false;

        }

        public bool HasLetterAllowComma(string text)
        {

            foreach (char currentCharacter in text)
            {

                if (!char.IsDigit(currentCharacter) && (currentCharacter != ',' || currentCharacter != '.'))
                {
                    return true;
                }

            }


            return false;

        }

        public bool HasLetterAllowSlash(string text)
        {
            int contador = 0;
            foreach (char currentCharacter in text)
            {
                if (contador == 9)
                {
                    return false;
                }

                if (currentCharacter != '/' && !char.IsDigit(currentCharacter))
                {
                    return true;
                }



                contador++;
            }


            return false;

        }
        #endregion

        #region Change Comma to Dot
        public string MoneyConvert(string money)
        {
            money = money.Replace('.', ',');
            return money;

        }


        public string MoneyConvert2decimals(string money)
        {
            money = money.Replace('.', ',');

            money = string.Format(Format, money);

            return money;

        }
        #endregion

        #region IMG Byte[]
        public byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        #endregion


        #region ImgConverter
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] bytes = (byte[])value;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapSource image = (BitmapSource)value;
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                return ms.ToArray();
            }
        }
        #endregion


        #region ExportMethods
        public void DeleteIfExist(FileInfo file)
        {
            if (file.Exists)
            {
                file.Delete();
            }
        }

        #endregion


        #region ValidImage
        public bool ValidFile(string filename, long limitInBytes, int limitWidth, int limitHeight)
        {

            try
            {
                var fileSizeInBytes = new FileInfo(filename).Length;
                if (fileSizeInBytes > limitInBytes) return false;

                using (var img = new Bitmap(filename))
                {
                    if (img.Width > limitWidth || img.Height > limitHeight) return false;
                }

            } catch
            {
                MessageBox.Show("Formato de imagen incorrecto");
            }
            return true;
        }
        #endregion
    }

}

