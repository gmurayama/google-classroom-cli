using CommandLine;
using GoogleClassroomCli.Application;
using GoogleClassroomCli.Infrastructure;
using GoogleClassroomCli.Infrastructure.GoogleClassroom;
using GoogleClassroomCli.Infrastructure.Models;
using System.IO;
using System.Linq;

namespace GoogleClassroomCli.Service.Options
{
    [Verb("exec-commands", HelpText = "Import a command list to be executed")]
    public class ExecuteCommandListOptions
    {
        [Value(0, HelpText = "Path to command list file", Required = true)]
        public string Path { get; set; }

        [Option('r', Default = false, HelpText = "Reprocess failed commands if there is any", Required = false)]
        public bool Reprocess { get; set; }
        public void ExecuteCommand(CourseService courseService)
        {
            var logger = new Logger();
            var queue = new QueueProcessor(logger);

            var textToCommandParser = new TextToCommandParser(courseService, logger);

            using (var reader = new StreamReader(Path))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var command = textToCommandParser.Parse(line);
                    var node = new Node(command);

                    if (node.Command.CommandType == CommandType.AddStudent)
                    {
                        var queueLastNode = queue.Queue.Last();
                        queueLastNode.Children.Add(node);
                    }
                    else
                    {
                        queue.Enqueue(node);
                    }
                }
            }

            queue.ProcessQueue();

            if (queue.HasAnyNodeFailed && Reprocess)
            {
                queue.ReprocessFailedQueue();
            }
        }
    }
}
