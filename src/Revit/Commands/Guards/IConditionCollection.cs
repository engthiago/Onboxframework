namespace Onbox.Revit.VDev.Commands.Guards
{
    /// <summary>
    /// The Command Guard Conditions that will be applied to Revit Commands.
    /// </summary>
    public interface IConditionCollection
    {
        /// <summary>
        /// Creates a condition builder that has the ability to add Commands to guard.
        /// </summary>
        /// <returns>The condition builder.</returns>
        IConditionBuilder AddCondition();
    }
}
