using CommandLine;
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
    }
}
