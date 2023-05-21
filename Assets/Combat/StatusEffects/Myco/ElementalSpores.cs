using System.IO;

public class ElementalSpores : IStatusEffect
{
    public int Duration { get; set; } = -1;
    public ElementalSpores() { }
    public ElementalSpores(Element element) => this.element = element;
    public Element element = Element.FUNGAL;

    byte[] IBinarySerializable.ByteData
    {
        get
        {
            var stream = new MemoryStream();
            stream.WriteEnum(element);
            return stream.GetAllBytes();
        }
        set
        {
            var stream = new MemoryStream(value);
            element = stream.ReadEnum<Element>();
        }
    }
}

