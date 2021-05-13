using VPG.Creator.BasicInteraction.Conditions;
using VPG.Creator.Core.Conditions;
using VPG.CreatorEditor.UI.StepInspector.Menu;

namespace VPG.CreatorEditor.BasicInteraction.UI.Conditions
{
    public class ReleasedMenuItem : MenuItem<ICondition>
    {
        public override string DisplayedName { get; } = "Release Object";

        public override ICondition GetNewItem()
        {
            return new ReleasedCondition();
        }
    }
}
