using Onbox.Mvc.V7;
using System;

namespace $rootnamespace$
{
    /// <summary>
    /// A component designed to be part of the MVC Framework
    /// </summary>
    public interface I$safeitemname$
    {
    }

    /// <summary>
    /// A component designed to be part of the MVC Framework
    /// </summary>
    public partial class $safeitemname$ : MvcComponentBase, I$safeitemname$
    {
        public $safeitemname$(INavigator navigator)
        {
            InitializeComponent();

	    // Hooks up this Navigator Component to the system
            navigator.Attach(this, this.Navigator);
        }
    }
}
