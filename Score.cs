using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePlay
{ 
        public class Score
        {
            [Key]
            public string UserName { get; set; }
            [ForeignKey("UserName")]
            public Users User { get; set; }
            public int Scores { get; set; }
            public override string ToString()
            {
                return "Username: " + UserName + "\nScore: " + Scores;
            }

        }
    
}
