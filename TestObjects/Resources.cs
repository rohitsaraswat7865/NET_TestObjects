namespace LocatorsRepo
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Configuration;
    using System.Xml;
    using OpenQA.Selenium;

    /// <summary>
    /// Parses xml comprising of 'page_element' name and retruns it as a Dictionary.
    /// </summary>
    public class ResourceManager
    {
        private Dictionary<string, By> locators = new Dictionary<string, By>();
        private Dictionary<string, string> configElements = new Dictionary<string, string>();
        public static string path = string.Empty;

        private ResourceManager(SupportedResourceType type)
        {
            try
            {
                AppDomain domain = AppDomain.CurrentDomain;
                string path = domain.BaseDirectory;
                ResourceManager.path = path;
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.Load(path + "Resource.xml");
                XmlNodeList list;
                if (!type.Equals(SupportedResourceType.config))
                {
                    if (type.Equals(SupportedResourceType.xpath))
                    {
                        list = xmlDoc.SelectNodes("//" + SupportedResourceType.locators.ToString() + "/" + SupportedResourceType.xpath.ToString());
                    }
                    else if (type.Equals(SupportedResourceType.id))
                    {
                        list = xmlDoc.SelectNodes("//" + SupportedResourceType.locators.ToString() + "/" + SupportedResourceType.id.ToString());
                    }
                    else
                    {
                        list = xmlDoc.SelectNodes("//" + SupportedResourceType.locators.ToString() + "/*");
                    }

                    foreach (XmlNode node in list)
                    {
                        if (node.Attributes[SupportedAttributes.page.ToString()].Value != null && node.Attributes[SupportedAttributes.name.ToString()].Value != null)
                        {
                            By value;
                            string key = node.Attributes[SupportedAttributes.page.ToString()].Value.ToString() + "_" + node.Attributes[SupportedAttributes.name.ToString()].Value.ToString();
                            if (node.Name.Equals(SupportedResourceType.xpath.ToString()) && node.InnerText.Length >= 1)
                            {
                                value = By.XPath(node.InnerText.ToString());
                            }
                            else if (node.Name.Equals(SupportedResourceType.id.ToString()) && node.InnerText.Length >= 1)
                            {
                                value = By.Id(node.InnerText.ToString());
                            }
                            else
                            {
                                throw new Exception("Resource.xml -> Empty locator value - Add locator for key - " + key);
                            }
                            this.locators.Add(key, value);
                        }
                        else
                        {
                            throw new Exception("Resource.xml -> Missing attributes for node - " + node.Name);

                        }
                    }
                }
                else
                {
                    list = xmlDoc.SelectNodes("//" + SupportedResourceType.config);
                    foreach (XmlNode node in list)
                    {
                        if (node.Attributes[ConfigAttributes.key.ToString()].Value != null && node.Attributes[ConfigAttributes.value.ToString()].Value != null && node.Attributes[ConfigAttributes.value.ToString()].Value != string.Empty)
                        {
                            string key = node.Attributes[ConfigAttributes.key.ToString()].Value.ToString();
                            string value = node.Attributes[ConfigAttributes.value.ToString()].Value.ToString();
                            this.configElements.Add(key, value);
                        }
                        else
                        {
                            throw new Exception("Resource.xml -> missing key or value in config node");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }
        /// <summary>
        /// Get list of locators withrespect to type. Without type it returns all functional locators in xml resource.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Dictionary<string, By></returns>
        public static Dictionary<string, By> GetLocators(SupportedResourceType type)
        {
            if (type.Equals(SupportedResourceType.config))
            {
                return null;
            }
            ResourceManager loc = new ResourceManager(type);
            return loc.locators;
        }
        /// <summary>
        /// Get the list of config elements from Resource.xml
        /// </summary>
        /// <returns>Dictionary<string,string></returns>
        public static Dictionary<string, string> GetConfiguration()
        {
            ResourceManager config = new ResourceManager(SupportedResourceType.config);
            return config.configElements;
        }
        /// <summary>
        /// Get specific locator from xml
        /// </summary>
        /// <param name="type"></param>
        /// <param name="specificKey"></param>
        /// <returns>By</returns>
        public By GetLocator(SupportedResourceType type, string specificKey)
        {
            By locator;
            ResourceManager loc = new ResourceManager(type);
            if (this.locators.TryGetValue(specificKey, out locator))//targets locator collection
            {
                return locator;
            }
            else
            {
                throw new Exception("Either resource not found or wrong key - " + specificKey + ". Refer Resource.xml ");
            }
        }


    }


    /// <summary>
    /// Supported locator types in Locators.xml
    /// </summary>
    public enum SupportedResourceType
    {
        /// <summary>
        /// locators
        /// </summary>
        locators,
        /// <summary>
        /// xpath
        /// </summary>
        xpath,
        /// <summary>
        /// id
        /// </summary>
        id,
        /// <summary>
        /// config
        /// </summary>
        config
    }

    /// <summary>
    /// Supported attributes types in Locators.xml
    /// </summary>
    internal enum SupportedAttributes
    {
        /// <summary>
        /// page
        /// </summary>
        page,
        /// <summary>
        /// name
        /// </summary>
        name
    }

    internal enum ConfigAttributes
    {
        /// <summary>
        /// key
        /// </summary>
        key,
        /// <summary>
        /// value
        /// </summary>
        value
    }
}