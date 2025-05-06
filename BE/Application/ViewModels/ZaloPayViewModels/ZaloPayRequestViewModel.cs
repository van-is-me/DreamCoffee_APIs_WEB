using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.ZaloPayViewModels
{
    public class ZaloPayRequestViewModel
    {
        public string app_id { get; set; }
        public string app_trans_id { get; set; }
        public string app_user { get; set; }
        public int amount { get; set; }
        public long app_time { get; set; }
        public string embed_data { get; set; }
        public string item { get; set; }
        public string description { get; set; }
        public string bank_code { get; set; }
        public string mac { get; set; }
    }
}
