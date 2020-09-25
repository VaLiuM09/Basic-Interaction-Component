using System;
using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.BasicInteraction.Conditions;
using Innoactive.Creator.BasicInteraction.Properties;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Validation;
using Innoactive.CreatorEditor.CourseValidation;
using UnityEngine;

namespace Innoactive.CreatorEditor.BasicInteraction.CourseValidation
{
    /// <summary>
    /// Validates that no Grab- and SnapZoneCondition targets the same object in one Step.
    /// </summary>
    public class GrabAndSnapCollisionValidator : CollisionValidator
    {
        protected override List<ValidationReportEntry> InternalValidate(IStep obj)
        {
            List<ValidationReportEntry> reports = new List<ValidationReportEntry>();

            foreach (ITransition transition in obj.Data.Transitions.Data.Transitions)
            {
                List<GrabbedCondition> grabs = GetCondition<GrabbedCondition>(transition)
                    .Where(condition => condition.Data.GrabbableProperty.IsEmpty() == false).ToList();
                List<SnappedCondition> snaps = GetCondition<SnappedCondition>(transition)
                    .Where(condition => condition.Data.Target.IsEmpty() == false).ToList();

                if (grabs.Any() && snaps.Any())
                {
                    foreach (SnappedCondition snappedCondition in snaps)
                    {
                        ISnappableProperty property = snappedCondition.Data.Target.Value;
                        Guid guid = property.SceneObject.Guid;
                        foreach (GrabbedCondition grabbedCondition in grabs.Where(snap => snap.Data.GrabbableProperty.Value.SceneObject.Guid == guid))
                        {
                            reports.Add(new ValidationReportEntry()
                            {
                                Context = new ConditionContext(grabbedCondition, new TransitionContext(transition, Context)),
                                Message = "A SnappedCondition and GrabbedCondition is used for the same object. The GrabbedCondition is not required.",
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
