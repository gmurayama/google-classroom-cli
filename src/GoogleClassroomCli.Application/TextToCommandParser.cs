using GoogleClassroomCli.Application.Commands;
using GoogleClassroomCli.Domain;
using GoogleClassroomCli.Infrastructure;
using GoogleClassroomCli.Infrastructure.GoogleClassroom;
using GoogleClassroomCli.Infrastructure.Models;
using System.Text.RegularExpressions;

namespace GoogleClassroomCli.Application
{
    public class TextToCommandParser
    {
        private Course _course;
        private CourseService _courseService;
        private Logger _logger;

        public TextToCommandParser(CourseService courseService)
        {
            _courseService = courseService;
        }

        public TextToCommandParser(CourseService courseService, Logger logger) : this(courseService)
        {
            _logger = logger;
        }

        public Command Parse(string textCommandLine)
        {
            var textCommand = RetrieveCommand(textCommandLine);

            Command command = null;

            switch (textCommand)
            {
                case "call-classroom":
                    command = GetCourse(textCommandLine);
                    break;
                case "add-classroom":
                    command = AddCourse(textCommandLine);
                    break;
                case "add-student":
                    command = AddStudent(textCommandLine);
                    break;
            }

            return command;
        }


        private string RetrieveCommand(string textCommandLine)
        {
            var arrayCommandLine = textCommandLine.Split(' ');
            return arrayCommandLine.Length > 0 ? arrayCommandLine[0] : null;
        }

        private Command GetCourse(string textCommandLine)
        {
            var split = textCommandLine.Split(' ');
            var courseId = split[split.Length - 1];

            return new Command(() =>
            {
                var getCourseCommand = new GetCourseCommand(_courseService, courseId);
                getCourseCommand.Execute();
                _course = getCourseCommand.Result;
                return _course;
            }, CommandType.GetCourse);
        }

        private Command AddCourse(string textCommandLine)
        {
            Regex regex = new Regex("\"(.*?)\"");
            var matches = regex.Matches(textCommandLine);
            var args = new string[matches.Count];

            for (int i = 0; i < args.Length; i++)
            {
                args[i] = matches[i].Value.Replace("\"", "");
            }

            var course = new Course
            {
                Name = args[0],
                Section = args[1],
                Room = args[2],
                PrimaryTeacher = new Teacher { Email = args[3] }
            };

            return new Command(() =>
            {
                var createCourseCommand = new CreateCourseCommand(_courseService, course);
                createCourseCommand.Execute();
                _course = createCourseCommand.Result;
                return _course;
            }, CommandType.AddCourse);
        }

        private Command AddStudent(string textCommandLine)
        {
            var split = textCommandLine.Split(' ');
            var studentEmail = split[split.Length - 1];

            return new Command(() =>
            {
                var addStudentToCourseCommand = new AddStudentToCourseCommand(_courseService, _course.Id, studentEmail);
                addStudentToCourseCommand.Execute();
                return addStudentToCourseCommand.Result;
            }, CommandType.AddStudent);
        }
    }
}
