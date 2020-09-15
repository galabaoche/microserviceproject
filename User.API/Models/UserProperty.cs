using System;
using System.Diagnostics.CodeAnalysis;

namespace User.API.Models
{
    public class UserProperty : IEquatable<UserProperty>
    {
        public int AppUserId { get; set; }

        public string Key { get; set; }
        public string Text { get; set; }

        public string Value { get; set; }

        public bool Equals(UserProperty other)
        {
            return other == null ? false : Key == other.Key && Value == other.Value;
        }
        public override int GetHashCode()
        {
            return Key.GetHashCode() ^ Value.GetHashCode();
        }
    }
}