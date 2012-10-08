using System.Text.RegularExpressions;

namespace TaskManagerConsole.ConcreteHandlers
{
    public abstract class BaseHandler
    {
        protected string Pattern { get; set; }

        public virtual bool Matches(string input)
        {
            var regex = new Regex(Pattern);
            return regex.IsMatch(input);
        }
        public abstract void Execute(string input);
    }
    
}
