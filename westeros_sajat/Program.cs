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


            var q1 = (from y in csatak
                      let attackingHouses =
                         (from x in csatak
                          group x by x.Att.HousesInString into g
                          select g.Key).SelectMany(t => t)
                      let defendingHouses =
                         (from z in csatak
                          group z by z.Def.HousesInString into g
                          select g.Key).SelectMany(t => t)
                      select attackingHouses.Concat(defendingHouses)).SelectMany(t => t).Distinct().Count();

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

            var q8 = from x in q5
                     from y in q6
                     where x.Region == y.RegionName
                     select x
                     ;

            var q9 = (from x in csatak                   // a második allekérdezés nem lenne szükséges, tehát meg lehetne írni az egészet
                      let battleWinners =               //egy queryn belül, de sajnos olyan szerkezetem (lista=>class=>lista), hogy
                          (from p in csatak             //a SelectMany() miatt muszáj ilyen result segédekkel dolgozni. Biztos van optimálisabb megoldás, de működik. Same goes for q11
                           select p.Outcome == "attacker" ? p.Att.HousesInString : p.Def.HousesInString).SelectMany(t => t)
                      let results =
                      from y in battleWinners
                      group y by y into g
                      select new
                      {
                          House = g.Key,
                          NumberOfWins = g.Count()
                      }
                      select results).First();


            var q10 = from x in csatak
                      let legnagyobbSereg =
                        (from y in csatak
                         select (!y.Att.Size.HasValue && !y.Def.Size.HasValue ? null :
                         (y.Att.Size.HasValue && !y.Def.Size.HasValue ? y.Att.Size :
                         (!y.Att.Size.HasValue && y.Def.Size.HasValue ? y.Def.Size :
                         (y.Att.Size <= y.Def.Size ? y.Def.Size : y.Att.Size))))).OrderByDescending(t => t).First()
                      where x.Att.Size.Equals(legnagyobbSereg) || x.Def.Size.Equals(legnagyobbSereg)
                      select new { NameOfTheBattle = x.Name, SizeOfTheArmy = legnagyobbSereg };

            var q11 = (from x in csatak
                       let attackingCommanders =
                         (from y in csatak
                          group y by y.Att.CommandersInString into g
                          select g.Key).SelectMany(t => t)
                       let results =
                         (from z in attackingCommanders
                          group z by z into g
                          select new { CommanderName = g.Key, NumberOfAttacks = g.Count() })
                       select results).First().OrderByDescending(t => t.NumberOfAttacks).Take(3);

            ; //debug point :D 
        }
    }
}
