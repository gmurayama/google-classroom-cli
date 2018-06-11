using GoogleClassroomCli.Domain;
using GoogleClassroomCli.Infrastructure.GoogleClassroom;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleClassroomCli.Application.Commands
{
    public class CreateCourseCommand
    {
        private CourseService _courseService;
        private Course _course;

        public CreateCourseCommand(CourseService couseService, Course course)
        {
            _courseService = couseService;
        }

        public Course Result { get; private set; }

        public void Execute()
        {
            _courseService.ImpersonateEmail(_course.PrimaryTeacher.Email);
            _course = _courseService.AddCourse(_course);
            Result = _course;
        }
    }
}
