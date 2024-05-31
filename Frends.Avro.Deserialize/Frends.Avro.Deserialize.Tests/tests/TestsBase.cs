using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Frends.Avro.Deserialize.Tests.tests;

[TestClass]
public abstract class TestsBase
{
    protected string testFileParentPath;

    [TestInitialize]
    public void TestSetup()
    {
        var rootPath = Directory.GetCurrentDirectory();
        var projPath = Directory.GetParent(rootPath).Parent.Parent.FullName;
        testFileParentPath = Path.Combine(projPath, "data");
    }

    protected JArray ExpectedResult =
        new()
        {
            new JObject
            {
                { "name", "John Doe" },
                { "isHuman", true },
                { "age", null },
                { "height", 172.5 },
                { "city", "New York" },
                { "balance", 10241.31 },
                { "children", JToken.FromObject(new List<string> { "Junior", "Junior II" }) },
                { "partner.name", "Marry Jane" },
                { "partner.age", 20 }
            }
        };
}
