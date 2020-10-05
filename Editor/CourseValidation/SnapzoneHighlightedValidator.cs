using System;
using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.BasicInteraction.Conditions;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.Core.Validation;
using Innoactive.CreatorEditor.CourseValidation;

namespace Innoactive.CreatorEditor.BasicInteraction.CourseValidation
{
    /// <summary>
    /// Validates that no SnapZone Is highlighted.
    /// </summary>
    public class SnapzoneHighlightedValidator : CollisionValidator
    {
        /// <inheritdoc/>
        protected override List<EditorReportEntry> InternalValidate(IStep step)
        {
            List<HighlightObjectBehavior> foundHighlights = new List<HighlightObjectBehavior>();
            List<EditorReportEntry> reports = new List<EditorReportEntry>();

            IEnumerable<HighlightObjectBehavior> highlights = GetBehavior<HighlightObjectBehavior>(step)
                .Where(behavior => behavior.Data.ObjectToHighlight.IsEmpty() == false);
            
            if (highlights.Any() == false)
            {
                return reports;
            }
            
            foreach (ITransition transition in step.Data.Transitions.Data.Transitions)
            {
                IEnumerable<SnappedCondition> snaps = GetCondition<SnappedCondition>(transition)
                    .Where(condition => condition.Data.ZoneToSnapInto.IsEmpty() == false);

                foreach (SnappedCondition snappedCondition in snaps)
                {
                    if (snappedCondition.Data.ZoneToSnapInto.IsEmpty())
                    {
                        continue;
                    }
                    
                    Guid snappedGuid = snappedCondition.Data.ZoneToSnapInto.Value.SceneObject.Guid;
                    foreach (HighlightObjectBehavior highlight in highlights)
                    {
                        if (highlight.Data.ObjectToHighlight.Value.SceneObject.Guid == snappedGuid && foundHighlights.Contains(highlight) == false)
                        {
                            foundHighlights.Add(highlight);
                            reports.Add(new EditorReportEntry()
                            {
                                Context = new BehaviorContext(highlight, Context),
                                Code = 3002,
                                Message = "A highlight is highlighting a SnapZone, which is automatically highlighted. The HighlightObjectBehavior is not required.",
                                ErrorLevel = ValidationErrorLevel.ERROR,
                                Validator = this,
                            });
                        }
                    }
                }
            }

            return reports;
        }
    }
}
