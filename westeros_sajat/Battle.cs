using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace westeros_sajat
{
    class Battle
    {
        private string name;
        private int year;
        private string outcome;
        private string type;
        private int majordeath;
        private int majorcapture;
        private string season;
        private string location;
        private string region;
        private Attacker att;
        private Defender def;


        public string Name { get => name; set => name = value; }
        public int Year { get => year; set => year = value; }
        public string Outcome { get => outcome; set => outcome = value; }
        public string Type { get => type; set => type = value; }
        public int Majordeath { get => majordeath; set => majordeath = value; }
        public int Majorcapture { get => majorcapture; set => majorcapture = value; }
        public string Season { get => season; set => season = value; }
        public string Location { get => location; set => location = value; }
        public string Region { get => region; set => region = value; }
        public Attacker Att { get => att; set => att = value; }
        public Defender Def { get => def; set => def = value; }
    }
}
