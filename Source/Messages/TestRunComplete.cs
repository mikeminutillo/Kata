
namespace Kata.Messages
{
    class TestRunComplete : Message
    {
        public int Total { get; set; }
        public int Failed { get; set; }
        public int Skipped { get; set; }
        public string[] Failures { get; set; }
    }
}
