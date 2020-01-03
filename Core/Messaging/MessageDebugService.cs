﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Core.V1.Messaging
{
    public class MessageDebugService : IMessageService
    {
        private string title = "Message Log Service";
        public void Error(string message)
        {
            System.Diagnostics.Debug.WriteLine($"****** {title} Error ******");
            System.Diagnostics.Debug.WriteLine(message);
        }

        public bool Question(string message)
        {
            System.Diagnostics.Debug.WriteLine($"****** {title} Question ******");
            System.Diagnostics.Debug.WriteLine(message);
            System.Diagnostics.Debug.WriteLine($"****** Log service will always return true ******");
            return true;
        }

        public void SetTitle(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
            {
                Error("Can not set title to empty.");
                return;
            }
            System.Diagnostics.Debug.WriteLine($"****** {title} Set Title: {newTitle} ******");
            title = newTitle;
        }

        public void Show(string message)
        {
            System.Diagnostics.Debug.WriteLine($"****** {title} Show ******");
            System.Diagnostics.Debug.WriteLine(message);
        }

        public void Warning(string message)
        {
            System.Diagnostics.Debug.WriteLine($"****** {title} Warning ******");
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}
