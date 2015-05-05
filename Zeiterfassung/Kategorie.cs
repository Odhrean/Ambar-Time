using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeiterfassung
{
    class Kategorie
    {
         public int ID { get; set; }
         public string Name { get; set; }

         public Kategorie()
         {
             ID = 0;
             Name = "";
         }
    }
}
