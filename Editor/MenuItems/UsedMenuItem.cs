using VPG.Creator.BasicInteraction.Conditions;
using VPG.Creator.Core.Conditions;
using VPG.CreatorEditor.UI.StepInspector.Menu;

namespace VPG.CreatorEditor.BasicInteraction.UI.Conditions
{
    public class UsedMenuItem : MenuItem<ICondition>
    {
        public override string DisplayedName { get; } = "Use Object";

        public override ICondition GetNewItem()
        {
            return new UsedCondition();
        }
    }
}