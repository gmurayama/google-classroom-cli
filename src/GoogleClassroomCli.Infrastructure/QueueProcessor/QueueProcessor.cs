using GoogleClassroom.Infrastructure.Models;
using GoogleClassroomCli.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoogleClassroomCli.Infrastructure
{
    public class QueueProcessor
    {
        public IList<Node> Queue { get; private set; }

        public IList<Tuple<Node, Exception>> QueueToReprocess { get; private set; }

        public Logger Logger { get; private set; }

        public bool HasAnyNodeFailed { get; private set; }

        public QueueProcessor()
        {
            Queue = new List<Node>();
            QueueToReprocess = new List<Tuple<Node, Exception>>();
        }

        public QueueProcessor(Logger logger) : this()
        {
            Logger = logger;
        }

        public void ProcessQueue()
        {
            ProcessQueue(Queue);
        }

        public void ReprocessFailedQueue()
        {
            var queue = QueueToReprocess.Select(x => x.Item1).ToList();
            QueueToReprocess.Clear();
            ProcessQueue(queue);
        }

        private void ProcessQueue(IList<Node> queue)
        {
            HasAnyNodeFailed = false;

            for (int i = queue.Count - 1; i >= 0; i--)
            {
                var node = Dequeue(queue);
                bool processChildNodes = ExecuteNodeCommand(node);

                if (processChildNodes)
                {
                    for (int j = node.Children.Count - 1; j >= 0; j--)
                    {
                        var childNode = Dequeue(node.Children);
                        ExecuteNodeCommand(childNode);
                    }
                }
            }

            Logger.WriteLog();
        }

        public void Enqueue(Node node)
        {
            Queue.Add(node);
        }

        public Node Dequeue()
        {
            if (Queue.Count > 0)
            {
                var node = Queue[0];
                Queue.RemoveAt(0);
                return node;
            }
            else
                return null;
        }

        public Node GetLastNode()
        {
            return Queue[Queue.Count - 1];
        }

        private void Enqueue(IList<Node> queue, Node node)
        {
            queue.Add(node);
        }

        private Node Dequeue(IList<Node> queue)
        {
            if (queue.Count > 0)
            {
                var node = queue[0];
                queue.RemoveAt(0);
                return node;
            }
            else
                return null;
        }

        private bool ExecuteNodeCommand(Node node)
        {
            bool commandExecutedSuccessfully;

            try
            {
                node.Command.Execute();
                commandExecutedSuccessfully = true;
            }
            catch (Exception ex)
            {
                var nodeFailed = new Tuple<Node, Exception>(node, ex);
                QueueToReprocess.Add(nodeFailed);
                commandExecutedSuccessfully = false;
                HasAnyNodeFailed = true;

                Logger.AddLog(new Log
                {
                    Message = string.Format("Node command \"{0}\" failed", node.Command.CommandType.ToString()),
                    Level = LogLevel.Error,
                    ErrorDetails = string.Format("Exception: {0} ; InnerException: {1} ;", ex.Message, ex.InnerException?.Message),
                    Date = DateTime.Now
                });
            }

            return commandExecutedSuccessfully;
        }
    }
}
