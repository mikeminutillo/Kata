
namespace Kata.Generators
{
    public partial class ProjectFileGenerator
    {
        protected readonly string KataName;

        public ProjectFileGenerator(string kataName)
        {
            KataName = kataName;
        }
    }
}
