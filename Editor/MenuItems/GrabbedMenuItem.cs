using VPG.Creator.BasicInteraction.Conditions;
using VPG.Creator.Core.Conditions;
using VPG.CreatorEditor.UI.StepInspector.Menu;

namespace VPG.CreatorEditor.BasicInteraction.UI.Conditions
{
    public class GrabbedMenuItem : MenuItem<ICondition>
    {
        public override string DisplayedName { get; } = "Grab Object";

        public override ICondition GetNewItem()
        {
            return new GrabbedCondition();
        }
    }
}