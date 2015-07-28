using System.Threading.Tasks;

namespace AppStudio.Common
{
    public static class TasksExtensions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "task")]
        public static void RunAndForget(this Task task)
        {
        }
    }
}
