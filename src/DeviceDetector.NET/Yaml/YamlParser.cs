using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace DeviceDetectorNET.Yaml
{
	public class YamlParser<T> : IParser<T>
		where T : class, IEnumerable// IParseLibrary
	{
		public T ParseFile(string file)
		{
			var thisAssembly = Assembly.GetExecutingAssembly();
			string filePath = "DeviceDetectorNET." + string.Join(".", file.Split('/')); 
			using (var stream = thisAssembly.GetManifestResourceStream(filePath))
			{
				using (var r = new StreamReader(stream))
				{
					var deserializer = new DeserializerBuilder().Build();
					var parser = new YamlDotNet.Core.Parser(r);

					// Consume the stream start event "manually"
					parser.Expect<StreamStart>();

					while (parser.Accept<DocumentStart>())
					// Deserialize the document
					{
						return deserializer.Deserialize<T>(parser);
					}
					return null;
				}
			}
		}
	}
}
