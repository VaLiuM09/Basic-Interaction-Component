using VPG.Creator.BasicInteraction.Conditions;
using VPG.Creator.Core.Conditions;
using VPG.CreatorEditor.UI.StepInspector.Menu;

namespace VPG.CreatorEditor.BasicInteraction.UI.Conditions
{
    public class SnappedMenuItem : MenuItem<ICondition>
    {
        public override string DisplayedName { get; } = "Snap Object";

        public override ICondition GetNewItem()
        {
            return new SnappedCondition();
        }
    }
}