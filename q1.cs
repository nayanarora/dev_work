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

        if (binder.Name.Equals("Text", StringComparison.OrdinalIgnoreCase))
        {
            result = xml.InnerXml;
            return true;
        }

        var childNode = xml[binder.Name];
        if (childNode == null)
            return false;

        bool hasElementChildren = false;
        foreach (XmlNode n in childNode.ChildNodes)
        {
            if (n.NodeType == XmlNodeType.Element)
            {
                hasElementChildren = true;
                break;
            }
        }

        if (hasElementChildren)
        {
            result = new XmlNavigator(childNode);
        }
        else
        {
            result = childNode.InnerText;
        }

        return true;
    }

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

        Console.WriteLine(xmlObj.State?.City?.Text ?? string.Empty);
    }
}
