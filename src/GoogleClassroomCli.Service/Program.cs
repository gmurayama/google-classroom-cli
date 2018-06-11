using CommandLine;
using GoogleClassroomCli.Application;
using GoogleClassroomCli.Application.Commands;
using GoogleClassroomCli.Infrastructure;
using GoogleClassroomCli.Infrastructure.GoogleClassroom;
using GoogleClassroomCli.Infrastructure.Models;
using GoogleClassroomCli.Service.Options;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Resources;
using static Google.Apis.Classroom.v1.ClassroomService;

namespace GoogleClassroomCli.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
            
            var courseService = new CourseService(
                privateKeyPath: configuration.GetSection("PrivateKeyPath").Value,
                emailToImpersonate: configuration.GetSection("AdminEmail").Value,
                scopes: new string[] { Scope.ClassroomCourses, Scope.ClassroomProfileEmails, Scope.ClassroomRosters }
            );

            Parser.Default.ParseArguments<AddStudentOptions,
                ExecuteCommandListOptions>
                (args)
                .WithParsed<AddStudentOptions>(opts =>
                {
                    var addStudentToCourseCommand = new AddStudentToCourseCommand(
                        courseService: courseService,
                        courseId: opts.CourseId,
                        studentEmail: opts.Email
                    );

                    addStudentToCourseCommand.Execute();
                })
                .WithParsed<ExecuteCommandListOptions>(opts =>
                {
                    var logger = new Logger();
                    var queue = new QueueProcessor(logger);

                    var textToCommandParser = new TextToCommandParser(courseService, logger);

                    using (var reader = new StreamReader(opts.Path))
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

                    if (queue.HasAnyNodeFailed && opts.Reprocess)
                    {
                        queue.ReprocessFailedQueue();
                    }
                })
                .WithNotParsed(errors =>
                {
                    foreach (var error in errors)
                    {
                        Console.WriteLine(error.ToString());
                    }
                });
        }
    }
}
