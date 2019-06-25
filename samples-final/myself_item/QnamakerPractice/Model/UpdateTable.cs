
namespace Model_UpdateTable
{
    ////The class representative NEW_KB string forward to get some information that come from repond because update table massage;
    public class UpdateTable
    {
        public Add add { get; set; }
        public Update update { get; set; }
        public Delete delete { get; set; }
    }

    public class Add
    {
        public Qnalist[] qnaList { get; set; }
        public string[] urls { get; set; }
    }

    public class Qnalist
    {
        public int id { get; set; }
        public string answer { get; set; }
        public string source { get; set; }
        public string[] questions { get; set; }
        public object[] metadata { get; set; }
    }

    public class Update
    {
        public string name { get; set; }
    }

    public class Delete
    {
        public int[] ids { get; set; }
    }

}
