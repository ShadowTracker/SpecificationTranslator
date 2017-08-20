using System;

namespace SpecificationTranslator.UnitTests.Query
{
    public class UserStub
    {
        public string Name { get; set; }

        public SexStub Sex { get; set; }

        public ContactInfoStub ContactInfo { get; set; }

        public decimal Balance { get; set; }

        public decimal? Point { get; set; }

        public int? TestCount { get; set; }

        public bool Enabled { get; set; }

        public DateTime InputDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        public int Version { get; set; }
    }

    public enum SexStub
    {
        Male = 1,
        Female = 2
    }
}
