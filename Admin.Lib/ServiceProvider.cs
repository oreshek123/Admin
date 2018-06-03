using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.Lib.Module;
using System.Xml;

namespace Admin.Lib
{
    public class ServiceProvider
    {
        public ServiceProvider() : this("") { }
        public ServiceProvider(string path)
        {
            if (string.IsNullOrEmpty(path))
                this.path_ = Path.Combine(@"E:\", "Providers.xml");
            else
                this.path_ = path;
        }
        public List<Provider> Providers = new List<Provider>();
        public List<int> Prefixes = new List<int>();
        private string path_ { get; set; }
        public void AddProvider()
        {
            Provider provider = new Provider();
            Console.WriteLine("Введите название компании");
            provider.NameCompany = Console.ReadLine();
            Console.WriteLine("Введите ссылку на логотип компании");
            provider.LogoUrl = Console.ReadLine();
            Console.WriteLine("Введите процентную ставку компании");
            Double.TryParse(Console.ReadLine(), out double per);
            provider.Percent = per;
            Console.WriteLine("Введите префиксы компании, для выхода нажмите ENTER");
            bool exit;
            do
            {
                exit = int.TryParse(Console.ReadLine(), out int pref);
                if (exit && IsExistsPrefix(pref))
                    provider.Prefix.Add(pref);
            } while (exit);

            if (IsExistsProvider(provider))
            {
                Providers.Add(provider);
                Prefixes.AddRange(provider.Prefix);
                AddProviderToXml(provider);
            }


        }

        private XmlElement Edit(XmlElement prov)
        {
            foreach (XmlNode item in prov.ChildNodes)
            {
                Console.WriteLine($"{item.Name} : ({item.InnerText}) - ");
                string cn = Console.ReadLine();
                if (!string.IsNullOrEmpty(cn))
                    item.InnerText = cn;
            }

            return prov;
        }
        public void SearchProviderByNameForEdit(string name)
        {
            XmlDocument xml = GetDocument();
            XmlElement root = xml.DocumentElement;
            bool find = false;
            
            foreach (XmlElement item in root.ChildNodes)
            {
                //find = false;
                foreach (XmlNode childNode in item.ChildNodes)
                {
                    if (childNode.Name == "NameCompany" && childNode.InnerText == name)
                        find = true;
                }

                if (find)
                {
                    XmlElement el = Edit(item);
                    break;
                }
            }
            if (find)
                xml.Save(path_);

        }
        public void EditProvider()
        {

            XmlDocument xml = GetDocument();
            XmlElement root = xml.DocumentElement;
            Console.WriteLine("------Список продайдеров-----------");
            foreach (XmlElement item in root.ChildNodes)
            {
                foreach (XmlNode c in item)
                {
                    if (c.Name == "NameCompany")
                        Console.WriteLine("-->" + c.InnerText);
                }
               
            }

            Console.WriteLine("-----------------------------------");
            Console.WriteLine("Введите наименование провайдера");
            SearchProviderByNameForEdit(Console.ReadLine());
        }
        public void DeleteProvider()
        {

        }
        private bool IsExistsProvider(Provider provider)
        {
            if (Providers.Where(pro => pro.NameCompany == provider.NameCompany).Count() > 0)
            {
                Console.WriteLine("Такой провайдер уже есть");
                return false;
            }

            return true;
        }
        private bool IsExistsPrefix(int prefix)
        {

            if (Prefixes.Where(pr => pr == prefix).Count() > 0)
                return false;
            return true;
        }
        private void AddProviderToXml(Provider provider)
        {
            XmlDocument doc = GetDocument();
            //если файл уже существует, то нужно считать файл и добавить в root element провайдера, иначе создать новый
            XmlElement element = doc.CreateElement("provider");

            XmlElement Logo = doc.CreateElement("LogoUrl");
            Logo.InnerText = provider.LogoUrl;


            XmlElement NameCompany = doc.CreateElement("NameCompany");
            NameCompany.InnerText = provider.NameCompany;

            XmlElement Percent = doc.CreateElement("Percent");
            Percent.InnerText = provider.Percent.ToString();

            XmlElement Pref = doc.CreateElement("Prefixes");
            foreach (int item in provider.Prefix)
            {
                XmlNode p = doc.CreateElement("Prefix");
                p.InnerText = item.ToString();
                Pref.AppendChild(p);
            }

            element.AppendChild(Logo);
            element.AppendChild(NameCompany);
            element.AppendChild(Percent);
            element.AppendChild(Pref);
            doc.DocumentElement.AppendChild(element);
            doc.Save(path_);


        }
        private XmlDocument GetDocument()
        {
            XmlDocument xml = new XmlDocument();
            //\\dc\Студенты\ПКО\SEB-171.2\C#\

            FileInfo file = new FileInfo(path_);
            if (file.Exists)
            {
                xml.Load(path_);
            }
            else
            {
                //FileStream fs = file.Create();
                //fs.Close();

                XmlElement element = xml.CreateElement("Providers");
                xml.AppendChild(element);
                xml.Save(path_);
            }

            return xml;
        }
    }
}
