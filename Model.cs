using System;
using System.Collections.Generic;

namespace FuncToString
{
    public class Model
    {
        public Foo foo { get; set; }
        public IList<Bar> bars { get; set; }

        public string FooBar { get; set; }

        public NestedType NestedType { get; set; }
    }

    public class NestedType
    {
        public MoreNesting MoreNesting { get; set; }
        public List<MoreNesting> MoreNestingList { get; set; }
    }

    public class MoreNesting
    {
        public EvenMore EvenMore { get; set; }
        public EvenMore[] EvenMoreArray { get; set; }
    }

    public class EvenMore
    {
        public Guid Field { get; set; }
    }

    public class Bar
    {
        public string name { get; set; }
    }

    public class Foo
    {
        public int id { get; set; }
    }
}
