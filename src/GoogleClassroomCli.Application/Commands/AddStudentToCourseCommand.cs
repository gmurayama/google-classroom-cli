using GoogleClassroomCli.Domain;
using GoogleClassroomCli.Infrastructure.GoogleClassroom;

namespace GoogleClassroomCli.Application.Commands
{
    public class AddStudentToCourseCommand
    {
        private readonly CourseService _courseService;
        private readonly string _courseId;
        private readonly string _studentEmail;

        public Student Result { get; private set; }

        public AddStudentToCourseCommand(CourseService courseService, string courseId, string studentEmail)
        {
            _courseService = courseService;
            _courseId = courseId;
            _studentEmail = studentEmail;
        }

        public void Execute()
        {
            _courseService.ImpersonateEmail("api.classroom@direitosbc.br");
            var student = _courseService.AddStudentToCourse(_courseId, _studentEmail);
            Result = student;
        }
    }
}
