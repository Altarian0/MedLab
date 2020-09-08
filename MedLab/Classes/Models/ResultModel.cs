using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLab.Classes.Models
{
    public class ResultModel
    {
        public int WaitingCount { get; set; }
        public int BioMaterialSendCount { get; set; }
        public int AssistantPinnedCount { get; set; }
        public int DoneCount { get; set; }
    }
}
