namespace ProductShop.Utilities;
public interface IXmlHelper
{
    public T Deserialize<T>(string inputXml);

    public T[] DeserializeCollection<T>(string inputXml);

    public string Serialize<T>(T[] input,string rootName);

    public string Serialize<T>(T input, string rootName);
}
