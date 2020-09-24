using System.Collections.Generic;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.Core.Conditions;
using Innoactive.CreatorEditor.CourseValidation;

namespace Innoactive.CreatorEditor.BasicInteraction.CourseValidation
{
    public abstract class CollisionValidator : BaseValidator<IStep, StepContext>
    {
        protected List<T> GetBehavior<T>(IStep step)
        {
            List<T> result = new List<T>();
            foreach (IBehavior behavior in step.Data.Behaviors.Data.Behaviors)
            {
                if (behavior is T)
                {
                    result.Add((T)behavior);
                }
            }

            return result;
        }
        
        protected List<T> GetCondition<T>(ITransition transition)
        {
            List<T> result = new List<T>();
            foreach (ICondition condition in transition.Data.Conditions)
            {
                if (condition is T)
                {
                    result.Add((T)condition);
                }
            }

            return result;
        }
    }
}