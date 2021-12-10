using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using TupleList = System.Collections.Generic.List<System.Tuple<string, System.Linq.Expressions.Expression<System.Func<FuncToString.Model, object>>>>;
namespace FuncToString;

public class LambdaPathTest
{
    public static IEnumerable<object[]> Data() =>
        new TupleList
            {
                new("FooBar", m => m.FooBar),
                new("foo.id", m => m.foo.id),
                new("NestedType.MoreNesting.EvenMore.Field", m => m.NestedType.MoreNesting.EvenMore.Field),
                new("bars[0].name", m => m.bars[0].name),
                new("NestedType.MoreNestingList[0].EvenMoreArray[0].Field", m => m.NestedType.MoreNestingList[0].EvenMoreArray[0].Field),
                new("bars[*].name", m => m.bars['*'].name),
                new("NestedType.MoreNestingList[*].EvenMoreArray[*].Field", m => m.NestedType.MoreNestingList[-1].EvenMoreArray['*'].Field),
                new("WithoutGetSet.MoreNesting.EvenMore.Field", m => m.WithoutGetSet.MoreNesting.EvenMore.Field),
            }
            .Select(t => new object[] { t.Item2, t.Item1 });


    [Theory]
    [MemberData(nameof(Data))]
    public void FuncToString_MatchesExpectations(Expression<Func<Model, object>> expression, string expected)
    {
        Assert.Equal(expected, LambdaPath<Model>.Get(expression));
    }

    
        
}