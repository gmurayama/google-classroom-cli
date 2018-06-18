using CommandLine;
using GoogleClassroomCli.Infrastructure.GoogleClassroom;
using GoogleClassroomCli.Service.Options;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
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
                .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
                .Build();
            
            var courseService = new CourseService(
                privateKeyPath: configuration.GetSection("PrivateKeyPath").Value,
                emailToImpersonate: configuration.GetSection("AdminEmail").Value,
                scopes: new string[] { Scope.ClassroomCourses, Scope.ClassroomProfileEmails, Scope.ClassroomRosters }
            );

            Parser.Default.ParseArguments<AddStudentOptions,
                GetCourseOptions,
                ExecuteCommandListOptions>
                (args)
                .WithParsed<AddStudentOptions>(addStudentOptions =>
                {
                    addStudentOptions.ExecuteCommand(courseService);
                })
                .WithParsed<GetCourseOptions>(getCourseOptions =>
                {
                    getCourseOptions.ExecuteCommand(courseService);
                })
                .WithParsed<ExecuteCommandListOptions>(executeCommandListOptions =>
                {
                    executeCommandListOptions.ExecuteCommand(courseService);
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
