using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleClassroomCli.Infrastructure.Models
{
    public class Node
    {
        public Node()
        {
            Children = new List<Node>();
        }

        public Node(Command command) : this()
        {
            Command = command;
        }

        public IList<Node> Children { get; private set; }
        public Command Command { get; set; }
    }
}
