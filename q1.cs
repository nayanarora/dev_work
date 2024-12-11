using System;
using System.Dynamic;
using System.Xml;

public class XmlNavigator : DynamicObject
{
    private XmlNode xml;

    public XmlNavigator(string doc)
    {
        var document = new XmlDocument();
        document.LoadXml(doc);
        this.xml = document.DocumentElement;
    }

    private XmlNavigator(XmlNode xml)
    {
        this.xml = xml;
    }

    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
        return false;
    }

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        result = null;

        if (xml == null)
            return false;

        var childNode = xml.SelectSingleNode(binder.Name);
        if (childNode != null)
        {
            result = childNode.HasChildNodes && childNode.FirstChild.NodeType == XmlNodeType.Element
                ? new XmlNavigator(childNode)
                : (object)childNode.InnerText;
            return true;
        }

        return false;
    }

    public string Text => xml?.OuterXml;
    static void Main(string[] args)
    {
        string xml =
            "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
            "<State>" +
            "<City>New York</City>" +
            "</State>";

        dynamic xmlObj = new XmlNavigator(xml);

        Console.WriteLine(xmlObj.Text ?? string.Empty);
        Console.WriteLine(xmlObj.State?.Text ?? string.Empty);
        Console.WriteLine(xmlObj.State?.City ?? string.Empty);
    }
}