using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLab.Classes.Models
{
    public class TimerData
    {
        public int Time { get; set; }
        public int Attemps { get; set; }
        public bool CaptchaActive { get; set; }
    }
}
