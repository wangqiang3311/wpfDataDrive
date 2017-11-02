using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace pdfResearch
{
    public class Tools
    {

        private static XmlNodeList SelectNodes(string xpath)
        {
            XmlDocument doc = new XmlDocument();
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            doc.Load(basePath + "xml/Config.xml");
            var node = doc.SelectNodes(xpath);
            return node;
        }


        public static List<BlockData> SelectList(string selectNode)
        {
            List<BlockData> items = new List<BlockData>();

            var nodes = SelectNodes(selectNode);

            if (nodes != null)
            {
                var type = typeof(BlockData);
                var properties = type.GetProperties().ToList();

                foreach (XmlNode node in nodes)
                {
                    BlockData config = new BlockData();

                    foreach (XmlAttribute a in node.Attributes)
                    {
                        string name = a.Name;
                        string value = a.Value;

                        var p = properties.FirstOrDefault(t => t.Name.ToLower() == name.ToLower());

                        if (p != null)
                        {
                            if (p.PropertyType.Name.ToLower() == "int32")
                            {
                                p.SetValue(config, int.Parse(value), null);
                            }
                            else if (p.PropertyType.Name.ToLower() == "boolean")
                            {
                                p.SetValue(config, bool.Parse(value), null);
                            }
                            else
                            {
                                p.SetValue(config, value, null);
                            }
                        }
                    }
                    items.Add(config);
                }
            }
            return items;
        }

        private static XmlNode SelectSingleNode(string xpath)
        {
            XmlDocument doc = new XmlDocument();
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            doc.Load(basePath + "xml/Config.xml");
            return doc.SelectSingleNode(xpath);

        }

        public static BlockData SelectSingle(string selectNode)
        {
            BlockData item = new BlockData();

            var node = SelectSingleNode(selectNode);

            if (node != null)
            {
                var type = item.GetType();
                var properties = type.GetProperties().ToList();

                foreach (XmlAttribute a in node.Attributes)
                {
                    string name = a.Name;
                    string value = a.Value;

                    var p = properties.FirstOrDefault(t => t.Name.ToLower() == name.ToLower());

                    if (p != null)
                    {
                        p.SetValue(item, value, null);
                    }
                }
            }
            return item;
        }
        public static void SaveXml(string xpath,List<BlockData> datas)
        {
            XmlDocument doc = new XmlDocument();
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            string path = basePath + "xml/Config.xml";
            doc.Load(path);
            var node = doc.SelectSingleNode(xpath);

            if (node != null)
            {
                var type = typeof(BlockData);
                var properties = type.GetProperties().ToList();

                foreach (var item in datas)
                {
                    var element = doc.CreateElement("Block");

                    foreach (var p in properties)
                    {
                       var value= p.GetValue(item, null);
                       element.SetAttribute(p.Name, value.ToString());
                    }
                    node.AppendChild(element);
                }
                doc.Save(path);
            }
        }
    }
}
