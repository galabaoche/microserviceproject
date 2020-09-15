using System;
using System.Collections.Generic;
using Project.Domain.SeedWork;

namespace Project.Domain.AggregatesModel
{
    public class ProjectProperty : ValueObject
    {
        public int ProjectId { get; set; }
        public string Key { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }

        public ProjectProperty(string value, string text, string key)
        {
            Value = value;
            Text = text;
            Key = key;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Key;
            yield return Value;
            yield return Text;
        }
    }
}
