using System.Collections.Generic;

namespace Onbox.Revit.VDev.Commands.Guards
{
    internal class ConditionCollection : IConditionCollection
    {
        readonly List<IConditionBuilderProvider> builders;

        public ConditionCollection()
        {
            this.builders = new List<IConditionBuilderProvider>();
        }

        public IConditionBuilder AddCondition()
        {
            var conditionBuilder = new ConditionBuilder();
            this.builders.Add(conditionBuilder);
            return conditionBuilder;
        }

        internal List<IConditionBuilderProvider> GetBuilders()
        {
            return this.builders;
        }
    }
}
