using CommandLine;
using GoogleClassroomCli.Application.Commands;
using GoogleClassroomCli.Infrastructure.GoogleClassroom;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleClassroomCli.Service.Options
{
    [Verb("add-student", HelpText = "Add a student into a course")]
    public class AddStudentOptions
    {
        [Value(0, HelpText = "Student email", Required = true)]
        public string Email { get; set; }
        [Value(1, HelpText = "Course ID from Google Classroom", Required = true)]
        public string CourseId { get; set; }
        [Option(HelpText = "E-mail used to perform the action of add student to course")]
        public string ImpersonatedEmail { get; set; }
        public void ExecuteCommand(CourseService courseService)
        {
            var addStudentToCourseCommand = new AddStudentToCourseCommand(
                        courseService: courseService,
                        courseId: CourseId,
                        studentEmail: Email
                    );

            addStudentToCourseCommand.Execute();
            var student = addStudentToCourseCommand.Result;

            Console.WriteLine("ID: {0}", student.Id);
            Console.WriteLine("Name: {0}", student.Name);
            Console.WriteLine("E-mail: {0}", student.Email);
        }
    }
}
