using GoogleClassroomCli.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace GoogleClassroomCli.Infrastructure.Models
{
    public class Command
    {
        private static readonly int NumberOfExecutedCommandsKeptInHistory = 5;
        private static Tuple<CommandType, DateTime>[] LastExecutedCommands = new Tuple<CommandType, DateTime>[NumberOfExecutedCommandsKeptInHistory];

        private Func<IEntity> _function;

        public IEntity Result { get; private set; }
        public CommandType CommandType { get; private set; }

        public Command(Func<IEntity> function, CommandType commandType)
        {
            _function = function;
            CommandType = commandType;
        }

        public void Execute()
        {
            var timeBetweenCommands = TimeBetweenFirstAndLastCommandExecuted();

            if (timeBetweenCommands.TotalSeconds < 1)
            {
                var timeToWait = (1 - timeBetweenCommands.TotalSeconds) * 1.15;
                var timeToWaitInMilliseconds = Convert.ToInt32(timeToWait * 1000);
                Thread.Sleep(timeToWaitInMilliseconds);
            }

            try
            {
                Result = _function();
                AddCommandToHistory(this);
            }
            catch (Exception ex)
            {
                AddCommandToHistory(this);
                throw ex;
            }
        }

        private TimeSpan TimeBetweenFirstAndLastCommandExecuted()
        {
            var timeFirstCommandFinishedExecuting = LastExecutedCommands[0]?.Item2 ?? DateTime.Now;
            var timeLastCommandFinishedExecuting = LastExecutedCommands[LastExecutedCommands.Length - 1]?.Item2 ?? timeFirstCommandFinishedExecuting;

            return timeFirstCommandFinishedExecuting - timeLastCommandFinishedExecuting;
        }

        private static void AddCommandToHistory(Command command)
        {
            var tupleToAdd = new Tuple<CommandType, DateTime>(command.CommandType, DateTime.Now);

            for (int i = 0; i < LastExecutedCommands.Length; i++)
            {
                var aux = LastExecutedCommands[i];
                LastExecutedCommands[i] = tupleToAdd;
                tupleToAdd = aux;
            }
        }
    }
}
