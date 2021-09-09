/* file name：lce.provider.XmlExt.cs
* author：lynx lynx.kor@163.com @ 2020/05/11 11:00:47
* copyright (c) 2020 Copyright@lynxce.com
* desc：
* > add description for XmlExt
* revision：
*
*/

using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace lce.provider
{
    /// <summary>
    /// action：XmlExt
    /// </summary>
    public static class XmlExt
    {
        /// <summary>
        /// Model to the xml.
        /// </summary>
        /// <returns>The xml.</returns>
        /// <param name="model">Model.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static string ToXml<T>(this T model)
        {
            var stream = new MemoryStream();
            var xml = new XmlSerializer(typeof(T));
            var xws = new XmlWriterSettings
            {
                //序列化XML格式
                Indent = false,
                OmitXmlDeclaration = true,
                Encoding = new UTF8Encoding(false),
                NewLineOnAttributes = false
            };
            var xtw = XmlWriter.Create(stream, xws);
            var nsp = new XmlSerializerNamespaces();
            nsp.Add("", "");
            xml.Serialize(xtw, model, nsp);
            stream.Position = 0;
            var sr = new StreamReader(stream);
            var str = sr.ReadToEnd();
            sr.Dispose();
            stream.Dispose();
            return str;
        }

        /// <summary>
        /// Xml to model.
        /// </summary>
        /// <returns>The model.</returns>
        /// <param name="xml">Xml.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T XmlToModel<T>(this string xml)
        {
            using (var sr = new StringReader(xml))
            {
                var xmldes = new XmlSerializer(typeof(T));
                return (T)xmldes.Deserialize(sr);
            }
        }
    }
}