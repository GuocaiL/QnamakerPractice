using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model_AnswerType
{
    //This is a class that representative anser message type;
    public class Rootobject
    {
        public Answers[] answers { get; set; }
    }

    public class Answers
    {
        public string[] questions { get; set; }
        public string answer { get; set; }
        public float score { get; set; }
        public int id { get; set; }
        public string source { get; set; }
        public object[] metadata { get; set; }
    }

}
