using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev.Commands;
using System.Windows.Forms;

namespace CommandGuardSamples.ContainerPipelines
{
    public class SingleGuardConditionPipeline : IContainerPipeline
    {
        public IContainer Pipe(IContainer container)
        {
            // Gets the assembly reference
            var currentAssembly = this.GetType().Assembly;

            container.AddRevitCommandGuardConditions(config =>
            {
                config.AddCondition()
                      .ForCommandsInAssembly(currentAssembly)
                      .CanExecute(info =>
                      {
                          var result = MessageBox.Show($"Can run Guard Condition 1?", "Guard Condition", MessageBoxButtons.YesNo);

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