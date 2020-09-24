﻿using System;
using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.BasicInteraction.Conditions;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.CreatorEditor.CourseValidation;
using UnityEditor;

namespace Innoactive.CreatorEditor.BasicInteraction.CourseValidation
{
    /// <summary>
    /// Validates that no SnapZone Is highlighted.
    /// </summary>
    public class SnapzoneHighlightedValidator : CollisionValidator
    {
        protected override List<ValidationReportEntry> InternalValidate(IStep obj)
        {
            List<HighlightObjectBehavior> foundHighlights = new List<HighlightObjectBehavior>();
            List<ValidationReportEntry> reports = new List<ValidationReportEntry>();

            List<HighlightObjectBehavior> highlights = GetBehavior<HighlightObjectBehavior>(obj)
                .Where(behavior => behavior.Data.ObjectToHighlight.IsEmpty() == false).ToList();
            
            if (highlights.Count == 0)
            {
                return reports;
            }
            
            foreach (ITransition transition in obj.Data.Transitions.Data.Transitions)
            {
                List<SnappedCondition> snaps = GetCondition<SnappedCondition>(transition)
                    .Where(condition => condition.Data.ZoneToSnapInto.IsEmpty() == false).ToList();

                if (snaps.Count == 0)
                {
                    continue;
                }
                
                foreach (SnappedCondition snappedCondition in snaps)
                {
                    if (snappedCondition.Data.Target.IsEmpty())
                    {
                        continue;
                    }
                    
                    Guid snappedGuid = snappedCondition.Data.Target.Value.SceneObject.Guid;
                    foreach (HighlightObjectBehavior highlight in highlights)
                    {
                        if (highlight.Data.ObjectToHighlight.Value.SceneObject.Guid == snappedGuid && foundHighlights.Contains(highlight) == false)
                        {
                            foundHighlights.Add(highlight);
                            reports.Add(new ValidationReportEntry()
                            {
                                Context = new BehaviorContext(highlight, Context),
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