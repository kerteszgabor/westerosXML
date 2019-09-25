using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace westeros_sajat
{
    class Program
    {
        static void Main(string[] args)
        {
            XDocument xdoc = new XDocument();
            xdoc = XDocument.Load("http://users.nik.uni-obuda.hu/prog3/_data/war_of_westeros.xml");

            Lekerdezesek(xdoc);

            Console.ReadLine();
        }

        public static void Lekerdezesek(XDocument xml)
        {
            List<Battle> csatak = new List<Battle>();
            foreach (XElement xe in xml.Descendants("battle"))
            {
                csatak.Add(new Battle()
                {
                    Name = xe.Element("name").Value,
                    Year = int.Parse(xe.Element("year").Value),
                    Outcome = xe.Element("outcome").Value,
                    Type = xe.Element("type").Value,
                    Majordeath = int.Parse(xe.Element("majordeath").Value),
                    Majorcapture = int.Parse(xe.Element("majorcapture").Value),
                    Season = xe.Element("season").Value,
                    Location = xe.Element("location").Value,
                    Region = xe.Element("region").Value,
                    Att = new Attacker()
                    {
                        King = xe.Element("attacker")?.Element("king")?.Value,
                        Commanders = xe.Element("attacker")?.Element("commanders")?.Elements("commander").ToList(), 
                        Houses = xe.Element("attacker")?.Descendants("house")?.ToList(),
                        Size = xe.Element("attacker").Descendants("size").Any() ? int.Parse(xe.Element("attacker").Element("size").Value) : (int?)null,
                    },
                    Def = new Defender()
                    {
                        King = xe.Element("defender")?.Element("king")?.Value,
                        Commanders = xe.Element("defender")?.Element("commanders")?.Elements("commander").ToList(),
                        Houses = xe.Element("defender")?.Descendants("house")?.ToList(),
                        Size = xe.Element("defender").Descendants("size").Any() ? int.Parse(xe.Element("defender").Element("size").Value) : (int?)null,
                    }
                });
            }
            ;



            //egyes
            var q1 = (from x in csatak
                      let y = x.Att.HousesInString
                      let z = x.Def.HousesInString
                      
                      select z);
                      

            var q2 = from x in csatak
                     where x.Type is "ambush"
                     select x;

            var q3 = from x in csatak
                     where x.Outcome is "defender" &&
                     x.Majorcapture != 0
                     select x;

            //var q4 = (from x in csatak
            //          where x.Outcome is "attacker" && x.Att.HousesInString is "Stark" ||
            //          x.Outcome is "defender" && x.Def.HousesInString is "Stark"
            //          select x).Count();

            // var q5 = from x in csatak

            ;



        }
    }
}
