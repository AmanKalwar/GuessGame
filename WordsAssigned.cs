using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePlay
{
    public class WordsAssigned
    {
        [Key, Column(Order = 0)]
        public int Word_ID { get; set; }
        [Key, Column(Order = 1)]
        public string UserName { get; set; }
        [ForeignKey("UserName")]
        public Users Users { get; set; }
        public string Word { get; set; }

    }
}

