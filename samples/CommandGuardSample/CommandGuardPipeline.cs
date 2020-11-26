using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands;
using System.Windows.Forms;

namespace CommandGuardSample
{
    public class CommandGuardPipeline : IContainerPipeline
    {
        public IContainer Pipe(IContainer container)
        {
            var currentAssembly = this.GetType().Assembly;

            container.AddRevitCommandGuardConditions(config =>
            {
                config.AddCondition()
                      .ForCommandsInAssembly(currentAssembly)
                      //.ForCommand<IndependentCommand>()
                      //.ExceptCommand<IndependentCommand>()
                      //.WhereCommandType(commandType => commandType.Name.Contains("Independent"))
                      .CanExecute(info =>
                      {
                          var commandData = info.GetCommandData();
                          var doc = commandData.Application.ActiveUIDocument.Document;

                          var result = MessageBox.Show($"Can run on {doc.Title}?", "Command Guard Conditon 1", MessageBoxButtons.YesNo);

                          if (result == DialogResult.Yes)
                          {
                              return true;
                          }
                          return false;
                      });

                config.AddCondition()
                      .ForCommandsInAssembly(currentAssembly)
                      //.ForCommand<IndependentCommand>()
                      //.ExceptCommand<IndependentCommand>()
                      //.WhereCommandType(commandType => commandType.Name.Contains("Independent"))
                      .CanExecute(info =>
                      {
                          var result = MessageBox.Show("Can run?", "Command Guard Conditon 2", MessageBoxButtons.YesNo);

                          if (result == DialogResult.Yes)
                          {
                              return true;
                          }
                          return false;
                      });
            });

            return container;
        }
    }
}