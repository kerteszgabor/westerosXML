using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace westeros_sajat
{
    class BattleParticipant
    {
        private string king;
        private List<XElement> commanders;
        private List<XElement> houses;
        private int? size;
        private List<string> commandersInString = new List<string>();
        private List<string> housesInString = new List<string>();


        public string King { get => king; set => king = value; }
        public List<XElement> Commanders { get => commanders; set {  commanders = value; ConvertXElementToList(commanders,commandersInString); } }
        public List<XElement> Houses { get => houses; set { houses = value; ConvertXElementToList(houses, housesInString); } }
        public int? Size { get => size; set => size = value; }
        public List<string> CommandersInString { get => commandersInString; set => commandersInString = value; }
        public List<string> HousesInString { get => housesInString; set => housesInString = value; }

        static void ConvertXElementToList(List<XElement> fieldToConvert, List<string> fieldToConvertTo)         //stringgé alakítom az XElement.Value-kat 
        {                                                                                                      //paraméterezhető, tehát újrafelhasználható kód
            if (fieldToConvert != null)
            {
                foreach (XElement item in fieldToConvert)
                {
                    fieldToConvertTo.Add(item.Value);
                }
            }
            
        }

    }
}
