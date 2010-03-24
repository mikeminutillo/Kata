
namespace Kata.Messages
{
    class ProjectCreated : Message
    {
        public string FullProjectPath { get; set; }
        public string FullSolutionPath { get; set; }
    }
}
