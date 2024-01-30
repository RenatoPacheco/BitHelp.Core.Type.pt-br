﻿using System;
using System.Text;
using System.Text.RegularExpressions;
using BitHelp.Core.Type.pt_BR.Helpers;
using BitHelp.Core.Type.pt_BR.Resources;

namespace BitHelp.Core.Type.pt_BR
{
    public struct PhoneType 
        : IFormattable, IComparable,
        IComparable<PhoneType>, IEquatable<PhoneType>
    {
        public PhoneType(string input)
        {
            TryParse(input, out PhoneType output);
            this = output;
        }

        private string _value;
        private bool _isValid;

        public static implicit operator string(PhoneType input) => input.ToString();
        public static implicit operator PhoneType(string input) => new PhoneType(input);

        public static PhoneType Parse(string input)
        {
            if (TryParse(input, out PhoneType result))
            {
                return result;
            }
            else
            {
                if (input == null)
                    throw new ArgumentException(
                        nameof(input), Resource.ItCannotBeNull);
                else
                    throw new ArgumentException(
                        nameof(input), Resource.ItIsNotInAValidFormat);
            }
        }

        public static bool TryParse(string input, out PhoneType output)
        {
            input = input?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(input))
            {
                string value = input;
                string pattern = @"^(\(0?[1-9]\d\) ?|0?[1-9]\d ?)?(9 ?)?([1-9]\d{3})(\-| )?(\d{4})$";
                if (Regex.IsMatch(value, pattern, RegexOptions.None, AppSettings.RegEx.TimeOut))
                {
                    StringBuilder result = new StringBuilder();
                    value = Regex.Replace(value, @"[^\d]", string.Empty, RegexOptions.None, AppSettings.RegEx.TimeOut);
                    value = Regex.Replace(value, @"^0", string.Empty, RegexOptions.None, AppSettings.RegEx.TimeOut);
                    MatchCollection matches = Regex.Matches(value, pattern, RegexOptions.None, AppSettings.RegEx.TimeOut);

                    if (matches[0].Groups[1].Value != string.Empty)
                        result.Append($"({matches[0].Groups[1].Value.Trim()}) ");

                    if (matches[0].Groups[2].Value != string.Empty)
                        result.Append($"{matches[0].Groups[2].Value.Trim()}");

                    result.Append(matches[0].Groups[3].Value);
                    result.Append("-");
                    result.Append(matches[0].Groups[5].Value);

                    output = new PhoneType { 
                        _value = result.ToString(),
                        _isValid = true
                    };
                    return true;
                }
            }
            output = new PhoneType
            {
                _value = input,
                _isValid = false
            };
            return false;
        }

        public bool IsValid() => _isValid;

        public override string ToString()
        {
            return ToString("D", null);
        }

        public string ToString(string format)
        {
            return ToString(format, null);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            format = format?.Trim().ToUpper() ?? string.Empty;

            if (format.Length != 1)
                throw new ArgumentException(
                    nameof(format), Resource.TheValueIsNotValid);

            char check = format[0];

            switch (check)
            {
                case 'D':
                    return _value;

                case 'N':
                    return Regex.Replace(_value, @"[^\d]", string.Empty, RegexOptions.None, AppSettings.RegEx.TimeOut);

                default:
                    throw new ArgumentException(
                        nameof(format), Resource.TheValueIsNotValid);
            }
        }

        public override int GetHashCode()
        {
            return $"{_value}:{GetType()}".GetHashCode();
        }

        public bool Equals(PhoneType other)
        {
            return GetHashCode() == other.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is PhoneType phone && Equals(phone);
        }

        public int CompareTo(PhoneType other)
        {
            return _value.CompareTo(other._value);
        }

        public int CompareTo(object obj)
        {
            if (obj is PhoneType phone)
            {
                return CompareTo(phone);
            }

            throw new ArgumentException(
                nameof(obj), Resource.ItIsNotAValidType);
        }

        public static bool operator ==(PhoneType left, PhoneType right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PhoneType left, PhoneType right)
        {
            return !(left == right);
        }

        public static bool operator >(PhoneType left, PhoneType right)
        {
            return left.CompareTo(right) == 1;
        }

        public static bool operator <(PhoneType left, PhoneType right)
        {
            return left.CompareTo(right) == -1;
        }

        public static bool operator >=(PhoneType left, PhoneType right)
        {
            return left > right || left == right;
        }

        public static bool operator <=(PhoneType left, PhoneType right)
        {
            return left < right || left == right;
        }
    }
}
