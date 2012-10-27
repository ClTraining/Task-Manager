using System;
using EntitiesLibrary.CommandArguments;

namespace Specifications.ClientSpecifications
{
    public class ListTodayClientSpecification : IClientSpecification
    {
        public DateTime Date { get; set; }
        public bool IsSatisfied(ListTaskArgs listArgs)
        {
            return listArgs.DueDate == DateTime.Today && listArgs.Id == null;
        }
    }
}
