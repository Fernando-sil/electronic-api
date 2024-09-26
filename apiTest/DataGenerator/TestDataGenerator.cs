using System.Collections;
using api.Helpers;

namespace apiTest.DataGenerator;

public class TestDataGenerator : IEnumerable<object[]>
{
    private readonly List<object[]> _data = new List<object[]>{
        new object[]{new QueryHelper{name = "name1"}},
        new object[]{new QueryHelper{name = "name2"}},
        new object[]{new QueryHelper{name = "name3"}},
        new object[]{new QueryHelper{name = ""}},
    };

    public IEnumerator<object[]> GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

}