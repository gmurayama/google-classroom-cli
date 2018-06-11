using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleClassroomCli.Domain
{
    public class Course : IEntity
    {
        public Course()
        {
            PrimaryTeacher = new Teacher();
            CoTeachers = new List<Teacher>();
            Students = new List<Student>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public Teacher PrimaryTeacher { get; set; }
        public IEnumerable<Teacher> CoTeachers { get; set; }
        public string Section { get; set; }
        public string Room { get; set; }
        public IEnumerable<Student> Students { get; set; }
    }
}
