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

        // If "Text" is requested, return the inner XML of the current node.
        if (binder.Name.Equals("Text", StringComparison.OrdinalIgnoreCase))
        {
            result = xml.InnerXml;
            return true;
        }

        // Try to get the child node by the requested name.
        var childNode = xml[binder.Name];
        if (childNode == null)
            return false;

        // Check if this child node has any element children.
        bool hasElementChildren = false;
        foreach (XmlNode n in childNode.ChildNodes)
        {
            if (n.NodeType == XmlNodeType.Element)
            {
                hasElementChildren = true;
                break;
            }
        }

        // If the child node has element children, return a new XmlNavigator so we can navigate into it.
        if (hasElementChildren)
        {
            result = new XmlNavigator(childNode);
        }
        else
        {
            // If it doesn't have element children, it's a leaf node with text content or inner markup.
            // Return the inner text for a leaf node (no child elements).
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
