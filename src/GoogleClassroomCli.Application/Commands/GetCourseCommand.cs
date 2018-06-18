using GoogleClassroomCli.Domain;
using GoogleClassroomCli.Infrastructure.GoogleClassroom;

namespace GoogleClassroomCli.Application.Commands
{
    public class GetCourseCommand
    {
        private readonly CourseService _courseService;
        private string _courseId;

        public Course Result { get; private set; }

        public GetCourseCommand(CourseService courseService, string courseId)
        {
            _courseService = courseService;
            _courseId = courseId;
        }

        public void Execute()
        {
            _courseService.ImpersonateEmail("api.classroom@direitosbc.br");
            var course = _courseService.FindCourseById(_courseId);
            Result = course;
        }
    }
}
