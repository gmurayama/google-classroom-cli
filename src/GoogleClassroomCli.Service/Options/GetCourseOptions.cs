using CommandLine;
using GoogleClassroomCli.Application.Commands;
using GoogleClassroomCli.Infrastructure.GoogleClassroom;
using System;

namespace GoogleClassroomCli.Service.Options
{
    [Verb("get-course", HelpText = "Get course information by it's Classroom ID")]
    public class GetCourseOptions
    {
        [Value(0, HelpText = "Google Classroom ID", Required = true)]
        public string Id { get; set; }

        public void ExecuteCommand(CourseService courseService)
        {
            var getCourseCommand = new GetCourseCommand(
                courseService: courseService, 
                courseId: Id);

            getCourseCommand.Execute();
            var course = getCourseCommand.Result;

            Console.WriteLine("ID: {0}", course.Id);
            Console.WriteLine("Name: {0}", course.Name);
            Console.WriteLine("Primary Teacher ID: {0}", course.PrimaryTeacher.Id);
            Console.WriteLine("Primary Teacher E-mail: {0}", course.PrimaryTeacher.Email);
            Console.WriteLine("Room: {0}", course.Room);
            Console.WriteLine("Section: {0}", course.Section);
        }
    }
}
