using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotTelegramHomework.Tables
{
    public class Defects
    {
        public long Id { get; set; }
        public int Qase_Id { get; set; }
        public int StepQase_Id { get; set;}
        public long Responsible_Id { get; set; }
        public long Author_Id { get; set; }
        public string Comment { get; set; }
    }
}
