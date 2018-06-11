using CommandLine;

namespace GoogleClassroomCli.Service.Options
{
    [Verb("exec-commands", HelpText = "Import a command list to be executed")]
    public class ExecuteCommandListOptions
    {
        [Value(0, HelpText = "Path to command list file", Required = true)]
        public string Path { get; set; }

        [Option('r', Default = false, HelpText = "Reprocess failed commands if there is any", Required = false)]
        public bool Reprocess { get; set; }
    }
}
