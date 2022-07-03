using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaMeetings
{
    enum Category
    {
        CodeMonkey,
        Hub,
        Short,
        TeamBuilding

    }


    enum Type
    {
        Live,
        InPerson
    }

    internal class Data
    {
        public List<string> Name { get; set; }
        public string ResponsiblePerson { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public Type Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
