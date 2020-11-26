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

            container.AddRevitCommandGuard(config =>
            {
                config.AddCondition()
                    .ForCommandsInAssembly(currentAssembly)
                    //.ForCommand<IndependentCommand>()
                    //.ExceptCommand<IndependentCommand>()
                    .WhereCommandType(commandType => commandType.Name.Contains(""))
                    .CanExecute(info =>
                    {
                        var result = MessageBox.Show("Can run?", "Command Guard", MessageBoxButtons.YesNo);
                        
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