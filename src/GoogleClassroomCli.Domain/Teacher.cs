using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleClassroomCli.Domain
{
    public class Teacher : IEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
