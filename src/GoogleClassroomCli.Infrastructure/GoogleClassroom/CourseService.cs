using GoogleClassroomCli.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleClassroomCli.Infrastructure.GoogleClassroom
{
    public class CourseService : GoogleClassroomService
    {
        public CourseService(string privateKeyPath) : base(privateKeyPath) { }

        public CourseService(string privateKeyPath, string emailToImpersonate) : base(privateKeyPath, emailToImpersonate) { }

        public CourseService(string privateKeyPath, string emailToImpersonate, params string[] scopes) : base(privateKeyPath, emailToImpersonate, scopes) { }

        public Course AddCourse(Course course)
        {
            var courseClassroom = new Google.Apis.Classroom.v1.Data.Course
            {
                Name = course.Name,
                Section = course.Section,
                Room = course.Room,
                OwnerId = course.PrimaryTeacher.Id ?? course.PrimaryTeacher.Email
            };

            var courseAddedToClassroom = Service.Courses.Create(courseClassroom).Execute();

            return CourseClassroomObjectToCourseDomainObject(courseAddedToClassroom);
        }

        public Course FindCourseById(string courseId)
        {
            var courseFromClassroom = Service.Courses.Get(courseId).Execute();

            return CourseClassroomObjectToCourseDomainObject(courseFromClassroom);
        }

        public Student AddStudentToCourse(string courseId, string studentId)
        {
            var studentClassroom = new Google.Apis.Classroom.v1.Data.Student
            {
                CourseId = courseId,
                UserId = studentId
            };

            var studentFromClassroom = Service.Courses.Students.Create(studentClassroom, courseId).Execute();

            return new Student
            {
                Id = studentFromClassroom.UserId,
                Email = studentFromClassroom.Profile.EmailAddress,
                Name = studentFromClassroom.Profile.Name.FullName
            };
        }

        public void RemoveStudentFromCourse(string courseId, string studentId)
        {
            Service.Courses.Students.Delete(courseId, studentId).Execute();
        }

        public Teacher GetTeacherProfile(string teacherId)
        {
            var teacherFromClassroom = Service.UserProfiles.Get(teacherId).Execute();

            return new Teacher
            {
                Id = teacherFromClassroom.Id,
                Email = teacherFromClassroom.EmailAddress,
                Name = teacherFromClassroom.Name.FullName
            };
        }

        public Student GetStudentProfile(string studentId)
        {
            var studentFromClassroom = Service.UserProfiles.Get(studentId).Execute();

            return new Student
            {
                Id = studentFromClassroom.Id,
                Email = studentFromClassroom.EmailAddress,
                Name = studentFromClassroom.Name.FullName
            };
        }

        private Course CourseClassroomObjectToCourseDomainObject(Google.Apis.Classroom.v1.Data.Course courseFromClassroom)
        {
            return new Course
            {
                Id = courseFromClassroom.Id,
                Name = courseFromClassroom.Name,
                Room = courseFromClassroom.Room,
                Section = courseFromClassroom.Section,
                PrimaryTeacher = new Teacher { Id = courseFromClassroom.OwnerId }
            };
        }
    }
}
