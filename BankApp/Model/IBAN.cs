﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using PRBD_Framework;

namespace BankApp.Model
{
    public struct IBAN
    {
        private string _iban;

        private IBAN(string iban)
        {
            _iban = iban;
        }

        /// <summary>
        /// Permet de créer un objet IBAN à partir d'une chaîne de caractères
        /// </summary>
        /// <param name="iban">la chaîne de caractère à parser</param>
        /// <returns></returns>
        public static IBAN Parse(string iban)
        {
            iban = iban.ToUpper().Replace(" ", "");
            var res = Validate(iban);
            if (Validate(iban) != ValidationResult.IsValid)
                throw new Exception(res.ToString());
            return new IBAN(iban);
        }

        /// <summary>
        /// Donne une représentation textuelle formatée d'un IBAN
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_iban.Substring(0, 4));
            for (int i = 4; i < _iban.Length; i += 4)
            {
                sb.Append(' ');
                sb.Append(_iban.Substring(i, Math.Min(4, _iban.Length - i)));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Méthode utilitaire pour permettre la conversion implicite d'un string en IBAN
        /// </summary>
        /// <param name="iban">le string à convertir</param>
        public static implicit operator IBAN(string iban) => IBAN.Parse(iban);

        /// <summary>
        /// Méthode utilitaire pour permettre la conversion d'un IBAN en string
        /// </summary>
        /// <param name="iban">l'IBAN à convertir</param>
        public static implicit operator string(IBAN iban) => iban.ToString();

        /// <summary>
        /// Permet de générer un code IBAN aléatoire
        /// </summary>
        /// <param name="countryCode">si différent de null, permet d'imposer le code pays</param>
        /// <param name="bankCode">si différent de null, permet d'imposer le code de la banque</param>
        /// <param name="rnd">permet de réutiliser un générateur de nombres aléatoires existant ; si null, un nouveau générateur est créé par la méthode</param>
        /// <returns></returns>
        public static IBAN Random(string countryCode = null, string bankCode = null, Random rnd = null)
        {
            if (rnd == null)
                rnd = new Random();

            if (countryCode == null)
                countryCode = Lengths.Keys.ToArray()[rnd.Next(0, Lengths.Count)];

            if (bankCode == null)
                bankCode = "";

            int lengthForCountryCode;
            var countryCodeKnown = Lengths.TryGetValue(countryCode, out lengthForCountryCode);
            if (!countryCodeKnown)
                throw new Exception(nameof(ValidationResult.CountryCodeNotKnown));

            StringBuilder sb = new(bankCode);
            for (int i = 0; i < lengthForCountryCode - 4 - bankCode.Length; ++i)
                sb.Append((char)('0' + rnd.Next(0, 10)));
            var bban = sb.ToString();
            var iban = countryCode + "00" + bban;
            var newIban = iban.Substring(4) + iban.Substring(0, 4);
            newIban = Regex.Replace(newIban, @"\D", match => (match.Value[0] - 55).ToString());
            var remainder = (98 - BigInteger.Parse(newIban) % 97).ToString("00");
            iban = countryCode + remainder + bban;
            return iban;
        }

        /// <summary>
        /// Permet de valider un string qui contient un IBAN
        /// </summary>
        /// <param name="value">le string à valider</param>
        /// <returns>le résultat de la validation sur la forme d'une valeur de l'enum ValidationResult (soit IsValid, soit un des statuts d'erreur)</returns>
        public static ValidationResult Validate(string value)
        {
            //Check if value is missing
            if (string.IsNullOrEmpty(value))
                return ValidationResult.ValueMissing;

            if (value.Length < 2)
                return ValidationResult.ValueTooSmall;

            var countryCode = value.Substring(0, 2).ToUpper();

            int lengthForCountryCode;

            var countryCodeKnown = Lengths.TryGetValue(countryCode, out lengthForCountryCode);
            if (!countryCodeKnown)
            {
                return ValidationResult.CountryCodeNotKnown;
            }

            //Check length.
            if (value.Length < lengthForCountryCode)
                return ValidationResult.ValueTooSmall;

            if (value.Length > lengthForCountryCode)
                return ValidationResult.ValueTooBig;

            value = value.ToUpper();
            var newIban = value.Substring(4) + value.Substring(0, 4);

            newIban = Regex.Replace(newIban, @"\D", match => (match.Value[0] - 55).ToString());

            var remainder = BigInteger.Parse(newIban) % 97;

            if (remainder != 1)
                return ValidationResult.ValueFailsModule97Check;

            return ValidationResult.IsValid;
        }

        /// <summary>
        /// Résultat de la validation d'un string contenant un IBAN
        /// </summary>
        public enum ValidationResult
        {
            IsValid,
            ValueMissing,
            ValueTooSmall,
            ValueTooBig,
            ValueFailsModule97Check,
            CountryCodeNotKnown
        }

        private static readonly IDictionary<string, int> Lengths = new Dictionary<string, int>
        {
            {"AL", 28},
            {"AD", 24},
            {"AT", 20},
            {"AZ", 28},
            {"BE", 16},
            {"BH", 22},
            {"BA", 20},
            {"BR", 29},
            {"BG", 22},
            {"CR", 21},
            {"HR", 21},
            {"CY", 28},
            {"CZ", 24},
            {"DK", 18},
            {"DO", 28},
            {"EE", 20},
            {"FO", 18},
            {"FI", 18},
            {"FR", 27},
            {"GE", 22},
            {"DE", 22},
            {"GI", 23},
            {"GR", 27},
            {"GL", 18},
            {"GT", 28},
            {"HU", 28},
            {"IS", 26},
            {"IE", 22},
            {"IL", 23},
            {"IT", 27},
            {"KZ", 20},
            {"KW", 30},
            {"LV", 21},
            {"LB", 28},
            {"LI", 21},
            {"LT", 20},
            {"LU", 20},
            {"MK", 19},
            {"MT", 31},
            {"MR", 27},
            {"MU", 30},
            {"MC", 27},
            {"MD", 24},
            {"ME", 22},
            {"NL", 18},
            {"NO", 15},
            {"PK", 24},
            {"PS", 29},
            {"PL", 28},
            {"PT", 25},
            {"RO", 24},
            {"SM", 27},
            {"SA", 24},
            {"RS", 22},
            {"SK", 24},
            {"SI", 19},
            {"ES", 24},
            {"SE", 24},
            {"CH", 21},
            {"TN", 24},
            {"TR", 26},
            {"AE", 23},
            {"GB", 22},
            {"VG", 24}
        };
    }
}
