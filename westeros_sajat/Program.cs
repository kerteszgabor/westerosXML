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
            var q1_seged= new List<string>();
            foreach (Battle item in csatak)
            {
                foreach (string i in item.Att.HousesInString)
                {
                    q1_seged.Add(i);
                }
                foreach (string i in item.Def.HousesInString)
                {
                    q1_seged.Add(i);
                }
            }
                ;

            var q1 = q1_seged.Distinct().Count();

            var q2 = from x in csatak
                     where x.Type is "ambush"
                     select x;

            var q3 = from x in csatak
                     where x.Outcome is "defender" &&
                     x.Majorcapture != 0
                     select x;

            var q4 = (from x in csatak
                      where x.Outcome is "attacker" && x.Att.HousesInString.Contains("Stark") ||
                      x.Outcome is "defender" && x.Def.HousesInString.Contains("Stark")
                      select x).Count();

            var q5 = from x in csatak
                     let attackerCount = x.Att.Houses.Count
                     let defenderCount = x.Def.Houses.Count
                     let housesInBattleCounter = attackerCount + defenderCount   
                     where housesInBattleCounter > 2
                     select x;

            var q6 = (from x in csatak
                      let regionsCounted =
                         from y in csatak
                         group y by y.Region into g
                         select new
                         {
                             RegionName = g.Key,
                             RegionCount = g.Count()
                         }
                      select regionsCounted.OrderByDescending(t => t.RegionCount).Take(3)).First();

            var q7 = (from x in csatak
                     let regionsCounted =
                        from y in csatak
                        group y by y.Region into g
                        select new
                        {
                            RegionName = g.Key,
                            RegionCount = g.Count()
                        }
                     select regionsCounted.OrderByDescending(t => t.RegionCount).First()).First();

           // var q8 = q5.Join(q6,(t => t.Region),(t => t.RegionName), )
            //         where q6.Union(x. 
            //         ; 

            //var q8 = from x in csatak
            //         let housesWins =
            //            from y in csatak



            ;



        }
    }
}
