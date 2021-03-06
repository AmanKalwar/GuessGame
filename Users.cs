using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePlay
{
    public class Users
    {
        [Key]
        public string User_ID { get; set; }
       // public string password { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public Score Score { get; set; }
        ICollection<WordsAssigned> Assigned { get; set; }
    }
}